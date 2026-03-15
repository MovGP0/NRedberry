using NRedberry.Contexts;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using ContextCC = NRedberry.Contexts.CC;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.DiracSimplify0.
/// Simplifies contractions of gammas with matching one-index tensors like k^a*k^b*G_a*G_b.
/// </summary>
public sealed class DiracSimplify0 : AbstractFeynCalcTransformation
{
    private readonly Dictionary<int, Tensor> _cache = new();
    private readonly string _gammaStringName;

    public DiracSimplify0(DiracOptions options)
        : base(options, Transformation.Identity)
    {
        _gammaStringName = ContextCC.GetNameDescriptor(GammaName).GetName(null, OutputFormat.Redberry);
    }

    public string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "DiracSimplify0";
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

        ProductContent productContent = productOfGammas.ProductContent;
        StructureOfContractions structure = productContent.StructureOfContractions;

        int couplesCount = 0;
        List<DiracSimplify0Element> couples = [];
        for (int i = 0; i < length; ++i)
        {
            DiracSimplify0Element? contraction = GetContraction(i, productOfGammas.GPositions[i], productContent, structure);
            if (contraction is null)
            {
                continue;
            }

            Tensor contractedTensor = productContent[contraction.TensorIndex1];
            if (contractedTensor is not SimpleTensor || contractedTensor.Indices.Size() != 1)
            {
                continue;
            }

            bool coupled = false;
            foreach (DiracSimplify0Element couple in couples)
            {
                if (!Match(productContent, couple, contraction))
                {
                    continue;
                }

                if (!couple.IsCoupled)
                {
                    couple.Couple(contraction);
                    ++couplesCount;
                }

                coupled = true;
                break;
            }

            if (!coupled)
            {
                couples.Add(contraction);
            }
        }

        if (couplesCount == 0)
        {
            return null;
        }

        DiracSimplify0Element[] elements = new DiracSimplify0Element[couplesCount];
        int coupledIndex = couplesCount;
        foreach (DiracSimplify0Element couple in couples)
        {
            if (couple.IsCoupled)
            {
                elements[--coupledIndex] = couple;
            }
        }

        Array.Sort(elements);

        List<Tensor> ordered = [];
        int[] mask = new int[length];
        foreach (DiracSimplify0Element element in elements)
        {
            modifiedElements.Add(element.TensorIndex1);
            modifiedElements.Add(element.TensorIndex2);

            if (!element.Available(mask))
            {
                ordered.Add(productContent[element.TensorIndex1]);
                ordered.Add(productContent[element.TensorIndex2]);
                continue;
            }

            Tensor? simplified = Simplify(element, productOfGammas.GPositions, productContent);
            if (simplified is not null)
            {
                ordered.Add(simplified);
                element.Cover(mask);
            }
        }

        for (int i = 0; i < mask.Length; ++i)
        {
            if (mask[i] != -1)
            {
                ordered.Add(productContent[productOfGammas.GPositions[i]]);
            }
        }

        Tensor result = ExpandAndEliminate.Transform(TensorApi.MultiplyAndRenameConflictingDummies(ordered));
        if (productOfGammas.G5Positions.Count == 1)
        {
            Tensor g5 = productContent[productOfGammas.GPositions[productOfGammas.G5Positions[0]]];
            result = result is Sum sum
                ? FastTensors.MultiplySumElementsOnFactorAndResolveDummies(sum, g5)
                : TensorApi.MultiplyAndRenameConflictingDummies(result, g5);
        }

        return Transform(result);
    }

    private Tensor? Simplify(DiracSimplify0Element element, List<int> gammaPositions, ProductContent productContent)
    {
        Tensor[] gammas = new Tensor[element.GammaIndex2 - element.GammaIndex1 + 1];
        for (int i = element.GammaIndex1; i <= element.GammaIndex2; ++i)
        {
            Tensor gamma = productContent[gammaPositions[i]];
            if (!IsGammaTensor(gamma))
            {
                return null;
            }

            gammas[i - element.GammaIndex1] = gamma;
        }

        Tensor ordered = Order(gammas);
        Tensor momentumProduct = TensorApi.Multiply(productContent[element.TensorIndex1], productContent[element.TensorIndex2]);
        Tensor result = ordered is Sum sum
            ? FastTensors.MultiplySumElementsOnFactor(sum, momentumProduct)
            : TensorApi.Multiply(ordered, momentumProduct);

        result = ExpandAndEliminate.Transform(result);
        result = TraceOfOne.Transform(result);
        result = DeltaTrace.Transform(result);
        return result;
    }

    private Tensor Metric(Tensor[] gammas)
    {
        return TensorApi.Multiply(
            Context.Get().CreateMetricOrKronecker(
                gammas[0].Indices[MetricType, 0],
                gammas[1].Indices[MetricType, 0]),
            Context.Get().CreateMetricOrKronecker(
                gammas[0].Indices.GetOfType(MatrixType).UpperIndices[0],
                gammas[1].Indices.GetOfType(MatrixType).LowerIndices[0]));
    }

    private Tensor Order(Tensor[] gammas)
    {
        int numberOfGammas = gammas.Length;
        if (!_cache.TryGetValue(numberOfGammas, out Tensor? canonical))
        {
            canonical = Order0(CreateLine(numberOfGammas));
            _cache[numberOfGammas] = canonical;
        }

        int[] from = new int[numberOfGammas + 2];
        int[] to = new int[numberOfGammas + 2];
        for (int i = 0; i < numberOfGammas; ++i)
        {
            from[i] = IndicesUtils.SetType(MetricType, i);
            to[i] = gammas[i].Indices[MetricType, 0];
        }

        from[numberOfGammas] = IndicesUtils.CreateIndex(0, MatrixType, true);
        to[numberOfGammas] = gammas[0].Indices.GetOfType(MatrixType).UpperIndices[0];
        from[numberOfGammas + 1] = IndicesUtils.CreateIndex(numberOfGammas, MatrixType, false);
        to[numberOfGammas + 1] = gammas[numberOfGammas - 1].Indices.GetOfType(MatrixType).LowerIndices[0];

        return EliminateMetricsTransformation.Eliminate(
            ApplyIndexMapping.Apply(canonical, new Mapping(from, to)));
    }

    private Tensor Order0(Tensor[] gammas)
    {
        if (gammas.Length == 1)
        {
            return gammas[0];
        }

        if (gammas.Length == 2)
        {
            return Metric(gammas);
        }

        Tensor[] left = (Tensor[])gammas.Clone();
        left[0] = Complex.Two;
        left[1] = Metric(gammas);

        Tensor[] swapped = (Tensor[])gammas.Clone();
        SwapAdj(swapped, 0);

        Tensor right = Order(swapped[1..]);
        if (right is Sum sum)
        {
            Tensor[] pair = TensorApi.ResolveDummy(sum, swapped[0]);
            right = pair[0] is Sum resolvedSum
                ? FastTensors.MultiplySumElementsOnFactor(resolvedSum, pair[1])
                : FastTensors.MultiplySumElementsOnFactor((Sum)pair[1], pair[0]);
        }
        else
        {
            right = TensorApi.MultiplyAndRenameConflictingDummies(right, swapped[0]);
        }

        return ExpandAndEliminate.Transform(TensorApi.Subtract(TensorApi.Multiply(left), right));
    }

    private static bool Match(ProductContent productContent, DiracSimplify0Element left, DiracSimplify0Element right)
    {
        return IndexMappings.AnyMappingExists(
            productContent[left.TensorIndex1],
            productContent[right.TensorIndex1]);
    }

    private DiracSimplify0Element? GetContraction(
        int gammaIndex,
        int gammaPosition,
        ProductContent productContent,
        StructureOfContractions structure)
    {
        Indices.Indices indices = productContent[gammaPosition].Indices;
        int indexPosition = 0;
        for (; indexPosition < indices.Size(); ++indexPosition)
        {
            if (MetricType.GetType_() == IndicesUtils.GetType(indices[indexPosition]))
            {
                break;
            }
        }

        int tensorIndex = StructureOfContractions.GetToTensorIndex(structure.contractions[gammaPosition][indexPosition]);
        if (tensorIndex == -1)
        {
            return null;
        }

        return new DiracSimplify0Element(tensorIndex, gammaIndex);
    }

    private bool IsGammaTensor(Tensor tensor)
    {
        return tensor is SimpleTensor simpleTensor
            && string.Equals(
                ContextCC.GetNameDescriptor(simpleTensor.Name).GetName(null, OutputFormat.Redberry),
                _gammaStringName,
                StringComparison.Ordinal);
    }
}

internal sealed class DiracSimplify0Element : IComparable<DiracSimplify0Element>
{
    public DiracSimplify0Element(int tensorIndex1, int gammaIndex1)
    {
        TensorIndex1 = tensorIndex1;
        GammaIndex1 = gammaIndex1;
        TensorIndex2 = -1;
        GammaIndex2 = -1;
    }

    public int TensorIndex1 { get; }

    public int GammaIndex1 { get; }

    public int TensorIndex2 { get; private set; }

    public int GammaIndex2 { get; private set; }

    public bool IsCoupled => TensorIndex2 != -1;

    public void Couple(DiracSimplify0Element other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (IsCoupled)
        {
            throw new InvalidOperationException("Element is already coupled.");
        }

        TensorIndex2 = other.TensorIndex1;
        GammaIndex2 = other.GammaIndex1;
    }

    public void Cover(int[] mask)
    {
        ArgumentNullException.ThrowIfNull(mask);

        for (int i = GammaIndex1; i <= GammaIndex2; ++i)
        {
            mask[i] = -1;
        }
    }

    public bool Available(int[] mask)
    {
        ArgumentNullException.ThrowIfNull(mask);

        for (int i = GammaIndex1; i <= GammaIndex2; ++i)
        {
            if (mask[i] != 0)
            {
                return false;
            }
        }

        return true;
    }

    public int CompareTo(DiracSimplify0Element? other)
    {
        if (other is null)
        {
            return 1;
        }

        return (GammaIndex2 - GammaIndex1).CompareTo(other.GammaIndex2 - other.GammaIndex1);
    }
}
