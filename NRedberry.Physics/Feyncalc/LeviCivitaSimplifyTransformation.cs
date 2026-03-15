using System.Collections.Generic;
using NRedberry.Core.Combinatorics;
using NRedberry.Contexts;
using NRedberry.Groups;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Options;
using NRedberry.Transformations.Symmetrization;
using Complex = NRedberry.Numbers.Complex;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.LeviCivitaSimplifyTransformation.
/// </summary>
public sealed class LeviCivitaSimplifyTransformation : TransformationToStringAble
{
    private const string DefaultLeviCivitaName = "eps";

    private static readonly object s_cacheLock = new();
    private static readonly Dictionary<int, ParseToken> s_cachedLeviCivitaSelfContractions = [];
    private static readonly Dictionary<int, Dictionary<Permutation, bool>> s_cachedLeviCivitaSymmetries = [];

    private readonly int _leviCivita;
    private readonly bool _minkowskiSpace;
    private readonly int _numberOfIndices;
    private readonly IndexType _typeOfLeviCivitaIndices;
    private readonly ChangeIndicesTypesAndTensorNames _tokenTransformer;
    private readonly ITransformation _simplifications;
    private readonly ITransformation _overallSimplifications;
    private readonly Expression[] _leviCivitaSimplifications;

    [Creator]
    public LeviCivitaSimplifyTransformation([Options] LeviCivitaSimplifyOptions options)
        : this(
            options.LeviCivita,
            options.MinkowskiSpace,
            options.Simplifications,
            options.OverallSimplifications)
    {
    }

    public LeviCivitaSimplifyTransformation(SimpleTensor leviCivita, bool minkowskiSpace)
        : this(leviCivita, minkowskiSpace, Transformation.Identity, Transformation.Identity)
    {
    }

    public LeviCivitaSimplifyTransformation(SimpleTensor leviCivita, bool minkowskiSpace, ITransformation simplifications)
        : this(leviCivita, minkowskiSpace, simplifications, Transformation.Identity)
    {
    }

    public LeviCivitaSimplifyTransformation(SimpleTensor leviCivita, bool minkowskiSpace, ITransformation simplifications, ITransformation overallSimplifications)
    {
        ArgumentNullException.ThrowIfNull(leviCivita);
        ArgumentNullException.ThrowIfNull(simplifications);
        ArgumentNullException.ThrowIfNull(overallSimplifications);

        CheckLeviCivita(leviCivita);

        _simplifications = simplifications;
        _overallSimplifications = overallSimplifications;
        _leviCivita = leviCivita.Name;
        _minkowskiSpace = minkowskiSpace;
        _numberOfIndices = leviCivita.SimpleIndices.Size();
        _typeOfLeviCivitaIndices = IndicesUtils.GetTypeEnum(leviCivita.SimpleIndices[0]);

        string leviCivitaName = TensorCC.NameManager.GetNameDescriptor(leviCivita.Name)
            .GetName(null, OutputFormat.Redberry);

        _tokenTransformer = new ChangeIndicesTypesAndTensorNames(
            new LeviCivitaTypesAndNamesTransformer(leviCivitaName, _typeOfLeviCivitaIndices));
        _leviCivitaSimplifications = GetLeviCivitaSubstitutions();
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        var iterator = new FromChildToParentIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) != null)
        {
            if (current is SimpleTensor simpleTensor
                && simpleTensor.Name == _leviCivita
                && current.Indices.Size() != current.Indices.GetFree().Size())
            {
                iterator.Set(Complex.Zero);
            }
            else if (current is Product product)
            {
                iterator.Set(SimplifyProduct(product));
            }
        }

        return iterator.Result();
    }

    public string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "LeviCivitaSimplify";
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    private Tensor SimplifyProduct(Product product)
    {
        ProductContent content = product.Content;
        List<int> epsilonPositions = [];
        for (int i = 0; i < content.Size; ++i)
        {
            if (IsLeviCivita(content[i], _leviCivita))
            {
                epsilonPositions.Add(i);
            }
        }

        if (epsilonPositions.Count == 0)
        {
            return product;
        }

        StructureOfContractions contractions = content.StructureOfContractions;
        HashSet<Tensor> epsilonComponent = new(_numberOfIndices);

        for (int i = 0; i < epsilonPositions.Count; ++i)
        {
            foreach (long contraction in contractions.contractions[epsilonPositions[i]])
            {
                int toIndex = StructureOfContractions.GetToTensorIndex(contraction);
                if (toIndex == -1)
                {
                    continue;
                }

                Tensor target = content[toIndex];
                if (IsLeviCivita(target, _leviCivita))
                {
                    continue;
                }

                epsilonComponent.Add(target);
            }

            if (epsilonComponent.Count == 0)
            {
                continue;
            }

            Tensor contractedProduct = TensorApi.Multiply([.. epsilonComponent]);
            epsilonComponent.Clear();

            int[] indices = contractedProduct.Indices.GetFree().AllIndices.ToArray();
            if (indices.Length == 1)
            {
                continue;
            }

            Tensor epsilon = content[epsilonPositions[i]];
            int[] epsilonIndices = epsilon.Indices.GetFree().AllIndices.ToArray();
            List<int> nonPermutablePositions = [];

            for (int b = 0; b < indices.Length; ++b)
            {
                bool contract = false;
                for (int a = 0; a < epsilonIndices.Length; ++a)
                {
                    if (indices[b] == IndicesUtils.InverseIndexState(epsilonIndices[a]))
                    {
                        contract = true;
                        break;
                    }
                }

                if (!contract)
                {
                    nonPermutablePositions.Add(b);
                }
            }

            IReadOnlyDictionary<Permutation, bool> symmetries = GetEpsilonSymmetries(indices.Length);
            MappingsPort port = IndexMappings.CreatePort(contractedProduct, contractedProduct);
            Mapping? mapping;
            while ((mapping = port.Take()) != null)
            {
                Permutation symmetry = TensorUtils.GetSymmetryFromMapping(indices, mapping);
                if (!CheckNonPermutingPositions(symmetry, [.. nonPermutablePositions]))
                {
                    continue;
                }

                if (symmetries.TryGetValue(symmetry.ToSymmetry(), out bool antisymmetry)
                    && symmetry.IsAntisymmetry != antisymmetry)
                {
                    return Complex.Zero;
                }
            }
        }

        if (epsilonPositions.Count == 1)
        {
            return product;
        }

        int offset = GetIndexlessOffset(product);
        int[] epsilonPositionsInProduct = new int[epsilonPositions.Count];
        for (int i = 0; i < epsilonPositions.Count; ++i)
        {
            epsilonPositionsInProduct[i] = epsilonPositions[i] + offset;
        }

        Tensor epsilonSubProduct = product.Select(epsilonPositionsInProduct);
        Tensor remnant = product.Remove(epsilonPositionsInProduct);

        foreach (Expression expression in _leviCivitaSimplifications)
        {
            epsilonSubProduct = expression.Transform(epsilonSubProduct);
        }

        epsilonSubProduct = _simplifications.Transform(
            EliminateMetricsTransformation.Eliminate(
                ExpandTransformation.Expand(
                    epsilonSubProduct,
                    EliminateMetricsTransformation.Instance,
                    _simplifications)));
        epsilonSubProduct = _leviCivitaSimplifications[1].Transform(epsilonSubProduct);

        return _overallSimplifications.Transform(TensorApi.Multiply(epsilonSubProduct, remnant));
    }

    private Expression GetLeviCivitaSelfContraction()
    {
        ParseToken substitutionToken;
        lock (s_cacheLock)
        {
            if (!s_cachedLeviCivitaSelfContractions.TryGetValue(_numberOfIndices, out substitutionToken!))
            {
                int[] lower = new int[_numberOfIndices];
                int[] upper = new int[_numberOfIndices];

                for (int i = 0; i < _numberOfIndices; ++i)
                {
                    lower[i] = i;
                    upper[i] = IndicesUtils.InverseIndexState(_numberOfIndices + i);
                }

                SimpleTensor eps1 = TensorApi.SimpleTensor(
                    DefaultLeviCivitaName,
                    IndicesFactory.CreateSimple(null, lower));
                SimpleTensor eps2 = TensorApi.SimpleTensor(
                    DefaultLeviCivitaName,
                    IndicesFactory.CreateSimple(null, upper));
                Tensor lhs = TensorApi.Multiply(eps1, eps2);

                Tensor[][] matrix = new Tensor[_numberOfIndices][];
                for (int i = 0; i < _numberOfIndices; ++i)
                {
                    matrix[i] = new Tensor[_numberOfIndices];
                    for (int j = 0; j < _numberOfIndices; ++j)
                    {
                        matrix[i][j] = Context.Get().CreateKronecker(lower[i], upper[j]);
                    }
                }

                Tensor rhs = TensorUtils.Det(matrix);
                substitutionToken = ParseUtils.TensorToAst(TensorApi.Expression(lhs, rhs));
                s_cachedLeviCivitaSelfContractions[_numberOfIndices] = substitutionToken;
            }
        }

        var substitution = (Expression)_tokenTransformer.Transform(substitutionToken).ToTensor();
        if (_minkowskiSpace && _numberOfIndices % 2 == 0)
        {
            substitution = TensorApi.Expression(substitution[0], TensorApi.Negate(substitution[1]));
        }

        return substitution;
    }

    private Expression[] GetLeviCivitaSubstitutions()
    {
        return
        [
            GetLeviCivitaSelfContraction(),
            TensorApi.Expression(
                Context.Get().CreateKronecker(
                    IndicesUtils.SetType(_typeOfLeviCivitaIndices, 0),
                    IndicesUtils.SetType(_typeOfLeviCivitaIndices, unchecked((int)0x80000000))),
                new Complex(_numberOfIndices))
        ];
    }

    private static IReadOnlyDictionary<Permutation, bool> GetEpsilonSymmetries(int indicesSize)
    {
        lock (s_cacheLock)
        {
            if (s_cachedLeviCivitaSymmetries.TryGetValue(indicesSize, out Dictionary<Permutation, bool>? symmetries))
            {
                return symmetries;
            }

            symmetries = [];
            PermutationGroup group = PermutationGroup.AntisymmetricGroup(indicesSize);
            foreach (Permutation symmetry in group)
            {
                symmetries[symmetry.ToSymmetry()] = symmetry.IsAntisymmetry;
            }

            s_cachedLeviCivitaSymmetries[indicesSize] = symmetries;
            return symmetries;
        }
    }

    private static bool CheckNonPermutingPositions(Permutation permutation, int[] nonPermutablePositions)
    {
        foreach (int position in nonPermutablePositions)
        {
            if (permutation.NewIndexOf(position) != position)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsLeviCivita(Tensor tensor, int leviCivitaName)
    {
        return tensor is SimpleTensor simpleTensor && simpleTensor.Name == leviCivitaName;
    }

    private static int GetIndexlessOffset(Product product)
    {
        return product.IndexlessData.Length + (product.Factor == Complex.One ? 0 : 1);
    }

    private static void CheckLeviCivita(SimpleTensor leviCivita)
    {
        ArgumentNullException.ThrowIfNull(leviCivita);

        SimpleIndices indices = leviCivita.SimpleIndices;
        if (indices.Size() <= 1)
        {
            throw new ArgumentException("Levi-Civita cannot be a scalar.");
        }

        byte type = IndicesUtils.GetType_(indices[0]);
        for (int i = 1; i < indices.Size(); ++i)
        {
            if (type != IndicesUtils.GetType_(indices[i]))
            {
                throw new ArgumentException("Levi-Civita have indices with different types.");
            }
        }
    }
}

file sealed record LeviCivitaTypesAndNamesTransformer(string LeviCivitaName, IndexType TypeOfLeviCivitaIndices)
    : TypesAndNamesTransformer
{
    public int NewIndex(int oldIndex, NameAndStructureOfIndices descriptor)
    {
        _ = descriptor;
        return oldIndex;
    }

    public IndexType NewType(IndexType oldType, NameAndStructureOfIndices descriptor)
    {
        _ = oldType;
        _ = descriptor;
        return TypeOfLeviCivitaIndices;
    }

    public string NewName(string oldName, NameAndStructureOfIndices descriptor)
    {
        _ = descriptor;
        return oldName == DefaultLeviCivitaNames.Name ? LeviCivitaName : oldName;
    }
}

file static class DefaultLeviCivitaNames
{
    public const string Name = "eps";
}
