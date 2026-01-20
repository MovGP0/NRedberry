using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Solver.Frobenius;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorOps = NRedberry.Tensors.Tensors;

namespace NRedberry.TensorGenerators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensorgenerator/TensorGenerator.java
 */

public sealed class TensorGenerator
{
    private readonly Tensor[] _samples;
    private readonly int[] _lowerArray;
    private readonly int[] _upperArray;
    private readonly List<SimpleTensor> _coefficients = [];
    private readonly bool _symmetricForm;
    private readonly SimpleIndices _indices;
    private readonly bool _withCoefficients;
    private Tensor _result = null!;

    private TensorGenerator(SimpleIndices indices, Tensor[] samples, bool symmetricForm, bool withCoefficients, bool raiseLowerSamples)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(samples);

        _samples = raiseLowerSamples ? ExpandSamples(samples) : samples;
        _indices = indices;
        _symmetricForm = symmetricForm;
        _lowerArray = indices.LowerIndices.ToArray();
        _upperArray = indices.UpperIndices.ToArray();
        _withCoefficients = withCoefficients;

        Array.Sort(_lowerArray);
        Array.Sort(_upperArray);
        Generate();
    }

    public static Tensor Generate(
        SimpleIndices indices,
        Tensor[] samples,
        bool symmetricForm,
        bool withCoefficients,
        bool raiseLower)
    {
        return new TensorGenerator(indices, samples, symmetricForm, withCoefficients, raiseLower).Result();
    }

    public static GeneratedTensor GenerateStructure(
        SimpleIndices indices,
        Tensor[] samples,
        bool symmetricForm,
        bool withCoefficients,
        bool raiseLower)
    {
        TensorGenerator generator = new(indices, samples, symmetricForm, withCoefficients, raiseLower);
        SimpleTensor[] generatedCoefficients = TensorUtils.GetAllSymbols(generator.Result()).ToArray();
        return new GeneratedTensor(generatedCoefficients, generator.Result());
    }

    private static Tensor[] ExpandSamples(Tensor[] samples)
    {
        ArgumentNullException.ThrowIfNull(samples);

        HashSet<Wrapper> set = [];
        foreach (Tensor sample in samples)
        {
            set.Add(new Wrapper(sample));
        }

        List<Tensor> result = [];
        foreach (Wrapper wrapper in set)
        {
            int freeSize = wrapper.Tensor.Indices.GetFree().Size();
            int capacity = freeSize == 0 ? 1 : (int)Math.Pow(2, freeSize);
            result.EnsureCapacity(result.Count + capacity);
            result.AddRange(TensorGeneratorUtils.AllStatesCombinations(wrapper.Tensor));
        }

        return result.ToArray();
    }

    private void Generate()
    {
        int totalLowCount = _lowerArray.Length;
        int[] lowCounts = new int[_samples.Length + 1];
        for (int i = 0; i < _samples.Length; ++i)
        {
            lowCounts[i] = _samples[i].Indices.GetFree().LowerIndices.Length;
        }

        lowCounts[^1] = totalLowCount;

        int totalUpCount = _upperArray.Length;
        int[] upCounts = new int[_samples.Length + 1];
        for (int i = 0; i < _samples.Length; ++i)
        {
            upCounts[i] = _samples[i].Indices.GetFree().UpperIndices.Length;
        }

        upCounts[^1] = totalUpCount;

        FrobeniusSolver fbSolver = new(lowCounts, upCounts);
        int[]? combination;
        SumBuilder result = new();
        while ((combination = fbSolver.Take()) is not null)
        {
            List<Tensor> tCombination = [];
            int u = 0;
            int l = 0;

            for (int i = 0; i < combination.Length; ++i)
            {
                for (int j = 0; j < combination[i]; ++j)
                {
                    Tensor temp = _samples[i];
                    Indices.Indices termLow = temp.Indices.GetFree().LowerIndices.Length == 0
                        ? IndicesFactory.EmptyIndices
                        : IndicesFactory.Create(temp.Indices.GetFree().LowerIndices.ToArray());
                    Indices.Indices termUp = temp.Indices.GetFree().UpperIndices.Length == 0
                        ? IndicesFactory.EmptyIndices
                        : IndicesFactory.Create(temp.Indices.GetFree().UpperIndices.ToArray());

                    int[] oldIndices = new int[termUp.Size() + termLow.Size()];
                    int[] newIndices = (int[])oldIndices.Clone();
                    for (int k = 0; k < termUp.Size(); ++k)
                    {
                        oldIndices[k] = termUp[k];
                        newIndices[k] = _upperArray[u++];
                    }

                    for (int k = 0; k < termLow.Size(); ++k)
                    {
                        oldIndices[k + termUp.Size()] = termLow[k];
                        newIndices[k + termUp.Size()] = _lowerArray[l++];
                    }

                    temp = ApplyIndexMapping.Apply(
                        temp,
                        new Mapping(oldIndices, newIndices),
                        _indices.AllIndices.ToArray());
                    tCombination.Add(temp);
                }
            }

            Tensor[] prodArray = tCombination.ToArray();
            TensorOps.ResolveAllDummies(prodArray);
            Tensor term = SymmetrizeUpperLowerIndicesITransformation.SymmetrizeUpperLowerIndices(
                TensorOps.MultiplyAndRenameConflictingDummies(prodArray));

            if (_symmetricForm || term is not Sum)
            {
                Tensor coefficient;
                if (_withCoefficients)
                {
                    coefficient = TensorCC.GenerateNewSymbol();
                    _coefficients.Add((SimpleTensor)coefficient);
                }
                else
                {
                    coefficient = Complex.One;
                }

                Tensor normalization = term is Sum ? new Complex(new Rational(1, term.Size)) : Complex.One;
                term = TensorOps.Multiply(coefficient, term, normalization);
            }
            else if (_withCoefficients)
            {
                term = FastTensors.MultiplySumElementsOnFactors((Sum)term);
            }

            result.Put(term);
        }

        Tensor built = result.Build();
        _result = _indices.Symmetries.IsTrivial() ? built : Symmetrize(built);
    }

    private Tensor Symmetrize(Tensor result)
    {
        Tensor transformed = new SymmetrizeITransformation(_indices, false).Transform(result);
        transformed = ExpandTransformation.Expand(transformed);

        if (transformed is not Sum sum)
        {
            return transformed;
        }

        Dictionary<int, List<Tensor[]>> coefficients = new();
        TensorBuilder rebuild = sum.GetBuilder();
        foreach (Tensor tensor in sum)
        {
            if (tensor is not Product product)
            {
                continue;
            }

            Tensor[] scalars = GetAllScalarsWithoutFactor(product);
            if (scalars.Length == 0)
            {
                continue;
            }

            Tensor oldCoefficient = scalars[0];
            if (!coefficients.TryGetValue(oldCoefficient.GetHashCode(), out List<Tensor[]>? list))
            {
                list = [];
                coefficients[oldCoefficient.GetHashCode()] = list;
            }

            Mapping? match = null;
            Tensor? newCoefficient = null;
            foreach (Tensor[] transformedPair in list)
            {
                match = IndexMappings.GetFirst(transformedPair[0], oldCoefficient);
                if (match is not null)
                {
                    newCoefficient = match.GetSign() ? TensorOps.Negate(transformedPair[1]) : transformedPair[1];
                    break;
                }
            }

            if (match is null)
            {
                if (oldCoefficient is SimpleTensor simple)
                {
                    newCoefficient = simple;
                }
                else if (_withCoefficients)
                {
                    newCoefficient = TensorCC.GenerateNewSymbol();
                    _coefficients.Add((SimpleTensor)newCoefficient);
                    foreach (SimpleTensor symbol in TensorUtils.GetAllSymbols(oldCoefficient))
                    {
                        _coefficients.Remove(symbol);
                    }
                }
                else
                {
                    newCoefficient = oldCoefficient;
                }

                list.Add([oldCoefficient, newCoefficient]);
            }

            rebuild.Put(TensorOps.Multiply(product.Factor, newCoefficient!, GetDataSubProduct(product)));
        }

        return rebuild.Build();
    }

    private Tensor Result()
    {
        return _result;
    }

    private static Tensor[] GetAllScalarsWithoutFactor(Product product)
    {
        Tensor[] scalars = product.Content.Scalars;
        Tensor[] allScalars = new Tensor[product.IndexlessData.Length + scalars.Length];
        Array.Copy(product.IndexlessData, 0, allScalars, 0, product.IndexlessData.Length);
        Array.Copy(scalars, 0, allScalars, product.IndexlessData.Length, scalars.Length);
        return allScalars;
    }

    private static Tensor GetDataSubProduct(Product product)
    {
        if (product.Data.Length == 0)
        {
            return Complex.One;
        }

        if (product.Data.Length == 1)
        {
            return product.Data[0];
        }

        return new Product(product.Indices, Complex.One, [], product.Data);
    }

    private sealed class Wrapper : IEquatable<Wrapper>
    {
        public Wrapper(Tensor tensor)
        {
            Tensor = tensor ?? throw new ArgumentNullException(nameof(tensor));
            FreeIndices = StructureOfIndices.Create(IndicesFactory.CreateSimple(null, tensor.Indices.GetFree()));
        }

        public Tensor Tensor { get; }

        private StructureOfIndices FreeIndices { get; }

        public bool Equals(Wrapper? other)
        {
            if (other is null)
            {
                return false;
            }

            return FreeIndices.Equals(other.FreeIndices)
                && IndexMappings.AnyMappingExists(Tensor, other.Tensor);
        }

        public override bool Equals(object? obj)
        {
            return obj is Wrapper other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Tensor.GetHashCode();
        }
    }
}
