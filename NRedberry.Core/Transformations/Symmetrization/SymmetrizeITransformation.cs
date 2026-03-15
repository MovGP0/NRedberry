using BigInteger = System.Numerics.BigInteger;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Options;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Port of cc.redberry.core.transformations.symmetrization.SymmetrizeTransformation.
/// </summary>
public sealed class SymmetrizeITransformation : ITransformation
{
    private static readonly BigInteger SmallOrderMaxValue = new(1_000);

    private readonly SimpleIndices indices;
    private readonly int[] indicesArray;
    private readonly int[] sortedIndicesNames;
    private readonly bool multiplyBySymmetryFactor;
    private readonly PermutationGroup indicesGroup;

    public SymmetrizeITransformation(SimpleIndices indices, bool multiplyBySymmetryFactor)
    {
        ArgumentNullException.ThrowIfNull(indices);

        this.indices = indices;
        indicesArray = indices.AllIndices.ToArray();
        sortedIndicesNames = IndicesUtils.GetIndicesNames(indices);
        Array.Sort(sortedIndicesNames);
        indicesGroup = indices.Symmetries.PermutationGroup;
        this.multiplyBySymmetryFactor = multiplyBySymmetryFactor;
    }

    [Creator(HasArgs = true)]
    public SymmetrizeITransformation(SimpleIndices indices, [Options] SymmetrizeOptions options)
        : this(indices, options?.MultiplyBySymmetryFactor ?? throw new ArgumentNullException(nameof(options)))
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor.Indices.Size() == 0)
        {
            return tensor;
        }

        if (!ContainsSubIndices(tensor.Indices, indices))
        {
            throw new ArgumentException(
                "Indices of specified tensor do not contain indices that should be symmetrized.",
                nameof(tensor));
        }

        IEnumerable<Permutation> cosetRepresentatives;
        BigInteger factor;
        if (tensor is SimpleTensor simpleTensor)
        {
            PermutationGroup tensorGroup = ConjugatedSymmetriesOfSubIndices(simpleTensor.SimpleIndices);
            PermutationGroup union = tensorGroup.Union(indicesGroup);
            Permutation[] representatives = union.LeftCosetRepresentatives(tensorGroup);
            cosetRepresentatives = representatives;
            factor = representatives.Length;
        }
        else if (indicesGroup.Order.CompareTo(SmallOrderMaxValue) < 0)
        {
            cosetRepresentatives = indicesGroup;
            factor = indicesGroup.Order;
        }
        else
        {
            PermutationGroup tensorGroup = PermutationGroup.CreatePermutationGroup(
                TensorUtils.FindIndicesSymmetries(indices, tensor));
            PermutationGroup union = tensorGroup.Union(indicesGroup);
            Permutation[] representatives = union.LeftCosetRepresentatives(tensorGroup);
            cosetRepresentatives = representatives;
            factor = representatives.Length;
        }

        SumBuilder sumBuilder = new();
        foreach (Permutation permutation in cosetRepresentatives)
        {
            sumBuilder.Put(
                ApplyIndexMapping.ApplyIndexMappingAutomatically(
                    tensor,
                    new Mapping(indicesArray, permutation.Permute(indicesArray), permutation.IsAntisymmetry)));
        }

        Tensor result = sumBuilder.Build();
        if (!multiplyBySymmetryFactor)
        {
            return result;
        }

        Complex factorTensor = new(new Rational(BigInteger.One, factor));
        if (result is Sum sum)
        {
            return FastTensors.MultiplySumElementsOnFactor(sum, factorTensor);
        }

        return Tensors.Tensors.Multiply(factorTensor, result);
    }

    private static bool ContainsSubIndices(Indices.Indices indices, Indices.Indices subIndices)
    {
        int[] indicesNames = IndicesUtils.GetIndicesNames(indices);
        Array.Sort(indicesNames);
        for (int i = 0, size = subIndices.Size(); i < size; ++i)
        {
            if (Array.BinarySearch(indicesNames, IndicesUtils.GetNameWithType(subIndices[i])) < 0)
            {
                return false;
            }
        }

        return true;
    }

    private PermutationGroup ConjugatedSymmetriesOfSubIndices(SimpleIndices allIndices)
    {
        int[] stabilizedPoints = new int[allIndices.Size() - indices.Size()];
        int[] mapping = new int[indices.Size()];
        int stabilizedPointer = 0;
        int mappingPointer = 0;
        for (int s = 0; s < allIndices.Size(); ++s)
        {
            int index = Array.BinarySearch(sortedIndicesNames, IndicesUtils.GetNameWithType(allIndices[s]));
            if (index < 0)
            {
                stabilizedPoints[stabilizedPointer++] = s;
            }
            else
            {
                mapping[mappingPointer++] = index;
            }
        }

        PermutationGroup result = allIndices.Symmetries.PermutationGroup
            .PointwiseStabilizerRestricted(stabilizedPoints);
        return result.Conjugate(Permutations.CreatePermutation(mapping));
    }

    public sealed class SymmetrizeOptions
    {
        [Option(Name = "SymmetryFactor", Index = 0)]
        public bool MultiplyBySymmetryFactor = true;
    }
}
