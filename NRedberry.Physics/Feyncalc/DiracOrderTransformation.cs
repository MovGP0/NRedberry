using System.Collections.Immutable;
using NRedberry.Contexts;
using NRedberry.Groups;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.DiracOrderTransformation.
/// </summary>
public sealed class DiracOrderTransformation : AbstractFeynCalcTransformation
{
    private readonly Dictionary<ImmutableArray<int>, Cached> _cache = new();

    public DiracOrderTransformation(DiracOptions options)
        : base(options, null)
    {
    }

    public static Tensor[] CreateArrayForTesting(DiracOptions options, int[] permutation)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(permutation);

        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(options.GammaMatrix);
        return CreateArrayCore(permutation, options.GammaMatrix.Name, types[0], types[1]);
    }

    public static Tensor? OrderArrayForTesting(DiracOptions options, params Tensor[] gammas)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(gammas);

        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(options.GammaMatrix);
        return OrderArrayCore(
            gammas,
            options.GammaMatrix.Name,
            types[0],
            types[1],
            new Dictionary<ImmutableArray<int>, Cached>());
    }

    public static int CompareGammasForTesting(Tensor gamma, int index, Tensor? contraction, Tensor otherGamma, int otherIndex, Tensor? otherContraction)
    {
        ArgumentNullException.ThrowIfNull(gamma);
        ArgumentNullException.ThrowIfNull(otherGamma);

        Gamma left = new(gamma, index, contraction);
        Gamma right = new(otherGamma, otherIndex, otherContraction);
        return left.CompareTo(right);
    }

    protected override Tensor? TransformLine(ProductOfGammas productOfGammas, List<int> modifiedElements)
    {
        ArgumentNullException.ThrowIfNull(productOfGammas);
        ArgumentNullException.ThrowIfNull(modifiedElements);

        if (productOfGammas.G5Positions.Count > 1
            || (productOfGammas.G5Positions.Count == 1 && productOfGammas.G5Positions[0] != productOfGammas.Length - 1))
        {
            throw new InvalidOperationException("G5s are not simplified.");
        }

        int length = productOfGammas.Length;
        if (productOfGammas.G5Positions.Count == 1)
        {
            --length;
        }

        if (length <= 1)
        {
            return null;
        }

        ProductContent productContent = productOfGammas.ProductContent;
        StructureOfContractions structure = productContent.StructureOfContractions;
        Gamma[] gammas = new Gamma[length];
        for (int i = 0; i < length; i++)
        {
            Tensor gamma = productContent[productOfGammas.GPositions[i]];
            gammas[i] = new Gamma(
                gamma,
                gamma.Indices[MetricType, 0],
                GetContraction(productOfGammas.GPositions[i], productContent, structure));
        }

        Tensor? ordered = OrderArray(gammas);
        if (ordered is null)
        {
            return null;
        }

        if (productOfGammas.G5Positions.Count == 1)
        {
            Tensor g5 = productContent[productOfGammas.GPositions[productOfGammas.G5Positions[0]]];
            ordered = ordered is Sum sum
                ? FastTensors.MultiplySumElementsOnFactorAndResolveDummies(sum, g5)
                : TensorApi.MultiplyAndRenameConflictingDummies(ordered, g5);
        }

        return ordered;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "DiracOrder";
    }

    private Tensor? GetContraction(int gamma, ProductContent productContent, StructureOfContractions structure)
    {
        Indices.Indices indices = productContent[gamma].Indices;
        int j = 0;
        for (; j < indices.Size(); ++j)
        {
            if (MetricType.GetType_() == IndicesUtils.GetType(indices[j]))
            {
                break;
            }
        }

        int to = StructureOfContractions.GetToTensorIndex(structure.contractions[gamma][j]);
        if (to == -1)
        {
            return null;
        }

        return productContent[to];
    }

    private Tensor[] CreateArray(int[] permutation)
    {
        return CreateArrayCore(permutation, GammaName, MetricType, MatrixType);
    }

    private Tensor? OrderArray(Tensor[] gammas)
    {
        return OrderArrayCore(gammas, GammaName, MetricType, MatrixType, _cache);
    }

    private Tensor? OrderArray(Gamma[] gammas)
    {
        return OrderArrayCore(gammas, GammaName, MetricType, MatrixType, _cache);
    }

    private Tensor OrderArray0(Tensor[] gammas)
    {
        return OrderArray0Core(gammas, MetricType, MatrixType);
    }

    private static Tensor[] CreateArrayCore(int[] permutation, int gammaName, IndexType metricType, IndexType matrixType)
    {
        int[] metricIndices = new int[permutation.Length];
        for (int i = 0; i < permutation.Length; ++i)
        {
            metricIndices[i] = IndicesUtils.SetType(metricType, i);
        }

        metricIndices = Permutations.Permute(metricIndices, Permutations.Inverse(permutation));
        Tensor[] gammas = new Tensor[permutation.Length];
        for (int i = 0; i < permutation.Length; ++i)
        {
            gammas[i] = TensorApi.SimpleTensor(
                gammaName,
                IndicesFactory.CreateSimple(
                    null,
                    IndicesUtils.CreateIndex(i, matrixType, true),
                    IndicesUtils.CreateIndex(i + 1, matrixType, false),
                    metricIndices[i]));
        }

        return gammas;
    }

    private static Tensor? OrderArrayCore(
        Tensor[] gammas,
        int gammaName,
        IndexType metricType,
        IndexType matrixType,
        Dictionary<ImmutableArray<int>, Cached> cache)
    {
        Gamma[] gammaData = new Gamma[gammas.Length];
        for (int i = 0; i < gammas.Length; ++i)
        {
            gammaData[i] = new Gamma(gammas[i], gammas[i].Indices[metricType, 0], null);
        }

        return OrderArrayCore(gammaData, gammaName, metricType, matrixType, cache);
    }

    private static Tensor? OrderArrayCore(
        Gamma[] gammas,
        int gammaName,
        IndexType metricType,
        IndexType matrixType,
        Dictionary<ImmutableArray<int>, Cached> cache)
    {
        int numberOfGammas = gammas.Length;
        int[] permutation = Permutations.CreateIdentityArray(numberOfGammas);

        Tensor firstGamma = gammas[0].GammaTensor;
        Tensor lastGamma = gammas[^1].GammaTensor;
        int[] metricIndices = new int[numberOfGammas];
        for (int i = 0; i < numberOfGammas; ++i)
        {
            metricIndices[i] = gammas[i].Index;
        }

        StableSort(gammas, permutation);
        if (Permutations.IsIdentity(permutation))
        {
            return null;
        }

        ImmutableArray<int> key = [..permutation];
        if (!cache.TryGetValue(key, out Cached? cached))
        {
            Tensor[] array = CreateArrayCore(permutation, gammaName, metricType, matrixType);
            Tensor ordered = EliminateMetricsTransformation.Eliminate(OrderArray0Core((Tensor[])array.Clone(), metricType, matrixType));
            cached = new Cached(array, ordered);
            cache[key] = cached;
        }

        int[] from = new int[numberOfGammas + 2];
        int[] to = new int[numberOfGammas + 2];
        Array.Copy(cached.GetOriginalIndices(metricType), from, numberOfGammas);
        Array.Copy(metricIndices, to, numberOfGammas);
        from[numberOfGammas] = cached.OriginalArray[0].Indices.GetOfType(matrixType).UpperIndices[0];
        to[numberOfGammas] = firstGamma.Indices.GetOfType(matrixType).UpperIndices[0];
        from[numberOfGammas + 1] = cached.OriginalArray[numberOfGammas - 1].Indices.GetOfType(matrixType).LowerIndices[0];
        to[numberOfGammas + 1] = lastGamma.Indices.GetOfType(matrixType).LowerIndices[0];

        return EliminateMetricsTransformation.Eliminate(
            ApplyIndexMapping.ApplyIndexMappingAutomatically(cached.Ordered, new Mapping(from, to)));
    }

    private static Tensor OrderArray0Core(Tensor[] gammas, IndexType metricType, IndexType matrixType)
    {
        SumBuilder sumBuilder = new();
        int swaps = 0;
        for (int i = 0; i < gammas.Length - 1; i++)
        {
            for (int j = 0; j < gammas.Length - i - 1; j++)
            {
                if (IndicesUtils.GetNameWithoutType(gammas[j].Indices[metricType, 0])
                    <= IndicesUtils.GetNameWithoutType(gammas[j + 1].Indices[metricType, 0]))
                {
                    continue;
                }

                Tensor metric = TensorApi.Multiply(
                    Complex.Two,
                    Context.Get().CreateMetricOrKronecker(
                        gammas[j].Indices[metricType, 0],
                        gammas[j + 1].Indices[metricType, 0]));
                Tensor[] cutAdjacent = CutAdj(gammas, j, matrixType);
                Tensor adjacent;
                if (cutAdjacent.Length == 0)
                {
                    adjacent = Context.Get().CreateMetricOrKronecker(
                        gammas[j].Indices.GetOfType(matrixType).UpperIndices[0],
                        gammas[j + 1].Indices.GetOfType(matrixType).LowerIndices[0]);
                }
                else if (cutAdjacent.Length == 1)
                {
                    adjacent = cutAdjacent[0];
                }
                else
                {
                    adjacent = OrderArray0Core(cutAdjacent, metricType, matrixType);
                }

                if (adjacent is Sum adjacentSum)
                {
                    adjacent = FastTensors.MultiplySumElementsOnFactor(adjacentSum, metric);
                }
                else
                {
                    adjacent = TensorApi.Multiply(adjacent, metric);
                }

                if ((swaps & 1) == 1)
                {
                    adjacent = TensorApi.Negate(adjacent);
                }

                sumBuilder.Put(adjacent);
                SwapAdj(gammas, j, matrixType);
                ++swaps;
            }
        }

        Tensor ordered = TensorApi.Multiply(gammas);
        if ((swaps & 1) == 1)
        {
            ordered = TensorApi.Negate(ordered);
        }

        sumBuilder.Put(ordered);
        return sumBuilder.Build();
    }

    private static void StableSort(Gamma[] gammas, int[] permutation)
    {
        for (int i = 1; i < gammas.Length; ++i)
        {
            Gamma gamma = gammas[i];
            int permutationValue = permutation[i];
            int j = i - 1;
            while (j >= 0 && gammas[j].CompareTo(gamma) > 0)
            {
                gammas[j + 1] = gammas[j];
                permutation[j + 1] = permutation[j];
                --j;
            }

            gammas[j + 1] = gamma;
            permutation[j + 1] = permutationValue;
        }
    }

    private static void SwapAdj(Tensor[] gammas, int index, IndexType matrixType)
    {
        Tensor tensor = gammas[index];
        gammas[index] = SetMatrixIndices((SimpleTensor)gammas[index + 1], gammas[index].Indices.GetOfType(matrixType));
        gammas[index + 1] = SetMatrixIndices((SimpleTensor)tensor, gammas[index + 1].Indices.GetOfType(matrixType));
    }

    private static Tensor[] CutAdj(Tensor[] original, int index, IndexType matrixType)
    {
        if (original.Length < 2)
        {
            return original;
        }

        Tensor[] result = new Tensor[original.Length - 2];
        Array.Copy(original, 0, result, 0, index);
        Array.Copy(original, index + 2, result, index, original.Length - index - 2);

        if (result.Length == 0)
        {
            return result;
        }

        int upper;
        int lower;
        if (index == 0)
        {
            index = 1;
            upper = original[0].Indices.GetOfType(matrixType).UpperIndices[0];
            lower = result[index - 1].Indices.GetOfType(matrixType).LowerIndices[0];
        }
        else if (index == original.Length - 2)
        {
            upper = result[index - 1].Indices.GetOfType(matrixType).UpperIndices[0];
            lower = original[^1].Indices.GetOfType(matrixType).LowerIndices[0];
        }
        else
        {
            upper = result[index - 1].Indices.GetOfType(matrixType).UpperIndices[0];
            lower = result[index].Indices.GetOfType(matrixType).UpperIndices[0];
        }

        result[index - 1] = SetMatrixIndices((SimpleTensor)result[index - 1], upper, lower);
        return result;
    }

    private static SimpleTensor SetMatrixIndices(SimpleTensor gamma, Indices.Indices matrixIndices)
    {
        return SetMatrixIndices(gamma, matrixIndices.UpperIndices[0], matrixIndices.LowerIndices[0]);
    }

    private static SimpleTensor SetMatrixIndices(SimpleTensor gamma, int matrixUpper, int matrixLower)
    {
        int[] indices = gamma.Indices.AllIndices.ToArray();
        for (int i = indices.Length - 1; i >= 0; --i)
        {
            if (Context.Get().IsMetric(IndicesUtils.GetType(indices[i])))
            {
                continue;
            }

            indices[i] = IndicesUtils.GetState(indices[i])
                ? IndicesUtils.CreateIndex(IndicesUtils.GetNameWithoutType(matrixUpper), IndicesUtils.GetType(indices[i]), true)
                : IndicesUtils.CreateIndex(IndicesUtils.GetNameWithoutType(matrixLower), IndicesUtils.GetType(indices[i]), false);
        }

        return TensorApi.SimpleTensor(gamma.Name, IndicesFactory.CreateSimple(null, indices));
    }

    public sealed class Cached
    {
        public Cached(Tensor[] originalArray, Tensor ordered)
        {
            OriginalArray = originalArray ?? throw new ArgumentNullException(nameof(originalArray));
            Ordered = ordered ?? throw new ArgumentNullException(nameof(ordered));
        }

        public Tensor[] OriginalArray { get; }

        public Tensor Ordered { get; }

        public int[] GetOriginalIndices(IndexType metricType)
        {
            int[] metricIndices = new int[OriginalArray.Length];
            for (int i = 0; i < OriginalArray.Length; ++i)
            {
                metricIndices[i] = OriginalArray[i].Indices[metricType, 0];
            }

            return metricIndices;
        }
    }

    public sealed class Gamma : IComparable<Gamma>
    {
        public Gamma(Tensor gamma, int index, Tensor? contraction)
        {
            GammaTensor = gamma ?? throw new ArgumentNullException(nameof(gamma));
            Index = index;
            Contraction = contraction;
        }

        public Tensor GammaTensor { get; }

        public int Index { get; }

        public Tensor? Contraction { get; }

        public bool Contracted()
        {
            return Contraction is SimpleTensor && Contraction.Indices.Size() == 1;
        }

        public int CompareTo(Gamma? other)
        {
            if (other is null)
            {
                return 1;
            }

            if (Contracted() && other.Contracted())
            {
                return string.CompareOrdinal(
                    ((SimpleTensor)Contraction!).GetStringName(),
                    ((SimpleTensor)other.Contraction!).GetStringName());
            }

            if (Contracted() && !other.Contracted())
            {
                return -1;
            }

            if (other.Contracted() && !Contracted())
            {
                return 1;
            }

            return IndicesUtils.GetNameWithoutType(Index).CompareTo(IndicesUtils.GetNameWithoutType(other.Index));
        }
    }
}
