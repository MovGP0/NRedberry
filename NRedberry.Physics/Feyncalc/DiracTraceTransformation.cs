using NRedberry.Contexts;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.DiracTraceTransformation.
/// </summary>
public sealed class DiracTraceTransformation : AbstractFeynCalcTransformation
{
    private static readonly IIndicator<GraphType> TraceFilter = new TraceGraphIndicator();
    private static readonly Parser Parser;
    private static readonly ParseToken TraceOf4GammasWith5Token;
    private static readonly ParseToken ChiholmKahaneToken;
    private static readonly ParseToken ChiholmKahaneTokenReversed;

    private readonly ITransformation? _simplifyLeviCivita;
    private readonly bool _useCache;
    private readonly Dictionary<int, Expression> _cachedTraces = [];

    private Expression? _traceOf4GammasWith5;
    private Expression? _chiholmKahaneIdentity;
    private Expression? _chiholmKahaneIdentityReversed;

    static DiracTraceTransformation()
    {
        Parser = TensorCC.Current.ParseManager.Parser;
        TraceOf4GammasWith5Token = Parser.Parse("G_a^a'_b'*G_b^b'_c'*G_c^c'_d'*G_d^d'_e'*G5^e'_a' = -4*I*eps_abcd");
        ChiholmKahaneToken = Parser.Parse("G_a^a'_c'*G_b^c'_d'*G_c^d'_b' = g_ab*G_c^a'_b'-g_ac*G_b^a'_b'+g_bc*G_a^a'_b'-I*e_abcd*G5^a'_c'*G^dc'_b'");
        ChiholmKahaneTokenReversed = Parser.Parse("G5^a'_c'*G^dc'_b' = -I*e^abcd*G_a^a'_c'*G_b^c'_d'*G_c^d'_b'/(d^n_n-3)/(d^n_n-2)/(d^n_n-1)");
    }

    public DiracTraceTransformation(DiracOptions options)
        : base(DoLeviCivita(options), new SimplifyGamma5Transformation(options))
    {
        ArgumentNullException.ThrowIfNull(options);

        _simplifyLeviCivita = options.SimplifyLeviCivita;
        _useCache = options.Cache;
    }

    public new Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (!ContainsGammaOr5Matrices(tensor))
        {
            return tensor;
        }

        tensor = ExpandDiracStructures(tensor);
        tensor = new SubstitutionTransformation([TraceOfOne, DeltaTrace]).Transform(tensor);
        return base.Transform(tensor);
    }

    public new string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "DiracTrace";
    }

    protected override IIndicator<GraphType> GraphFilter()
    {
        return TraceFilter;
    }

    protected override Tensor? TransformLine(ProductOfGammas productOfGammas, List<int> modifiedElements)
    {
        ArgumentNullException.ThrowIfNull(productOfGammas);
        ArgumentNullException.ThrowIfNull(modifiedElements);

        if (productOfGammas.G5Positions.Count != 0
            && (productOfGammas.G5Positions.Count != 1
                || productOfGammas.G5Positions[0] != productOfGammas.Length - 1))
        {
            throw new InvalidOperationException("G5s are not simplified.");
        }

        if (productOfGammas.GraphType != GraphType.Cycle)
        {
            throw new InvalidOperationException("Dirac traces can only be calculated on cycles.");
        }

        Tensor product = productOfGammas.ToProduct();
        return productOfGammas.G5Positions.Count == 0
            ? TraceWithout5(product, productOfGammas.Length)
            : TraceWith5(product, productOfGammas.Length);
    }

    private static DiracOptions DoLeviCivita(DiracOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.SimplifyLeviCivita is null)
        {
            return options;
        }

        DiracOptions prepared = options.Clone();
        prepared.ExpandAndEliminate = new TransformationCollection(
            options.ExpandAndEliminate,
            options.SimplifyLeviCivita);
        return prepared;
    }

    private Tensor ExpandDiracStructures(Tensor tensor)
    {
        FromChildToParentIterator iterator = new(tensor);
        Tensor? current;
        while ((current = iterator.Next()) != null)
        {
            if (current is not Product product)
            {
                continue;
            }

            if (!ContainsGammaOr5Matrices(current))
            {
                continue;
            }

            PrimitiveSubgraph[] partition = PrimitiveSubgraphPartition.CalculatePartition(product.Content, MatrixType);
            bool containsTraces = false;
            foreach (PrimitiveSubgraph subgraph in partition)
            {
                if (subgraph.GraphType == GraphType.Cycle)
                {
                    containsTraces = true;
                    break;
                }
            }

            if (!containsTraces)
            {
                continue;
            }

            iterator.Set(
                TensorApi.Multiply(
                    GetIndexlessSubProduct(product),
                    ExpandAndEliminate.Transform(GetDataSubProduct(product))));
        }

        return iterator.Result();
    }

    private Expression GetTraceSubstitution(int length)
    {
        if (_useCache && _cachedTraces.TryGetValue(length, out Expression? cached))
        {
            return cached;
        }

        Tensor[] data = new Tensor[length];
        int matrixIndex = IndicesUtils.SetType(MatrixType, 0) - 1;
        int metricIndex = -1;
        int firstUpper = ++matrixIndex;
        int upper = firstUpper;

        for (int i = 0; i < length; ++i)
        {
            data[i] = TensorApi.SimpleTensor(
                GammaName,
                IndicesFactory.CreateSimple(
                    null,
                    upper | unchecked((int)0x80000000),
                    i == length - 1 ? firstUpper : (upper = ++matrixIndex),
                    IndicesUtils.SetType(MetricType, ++metricIndex)));
        }

        Tensor rhs = TraceOfArray(data);
        rhs = ExpandAndEliminate.Transform(rhs);
        Expression trace = TensorApi.Expression(TensorApi.Multiply(data), rhs);
        if (_useCache)
        {
            _cachedTraces[length] = trace;
        }

        return trace;
    }

    private Tensor TraceOfArray(Tensor[] data)
    {
        if (data.Length == 1)
        {
            return Complex.Zero;
        }

        if (data.Length == 2)
        {
            return TensorApi.Multiply(
                TraceOfOne[1],
                Context.Get().CreateMetricOrKronecker(
                    data[0].Indices[MetricType, 0],
                    data[1].Indices[MetricType, 0]));
        }

        if ((data.Length & 1) == 1)
        {
            return Complex.Zero;
        }

        SumBuilder sumBuilder = new();
        for (int i = 0; i < data.Length - 1; ++i)
        {
            Tensor term = TensorApi.Multiply(
                Complex.Two,
                Context.Get().CreateMetricOrKronecker(
                    data[i].Indices[MetricType, 0],
                    data[i + 1].Indices[MetricType, 0]),
                TraceOfArray(SubArray(data, i, i + 1)));
            if ((i & 1) == 1)
            {
                term = TensorApi.Negate(term);
            }

            sumBuilder.Put(term);
            Swap(data, i, i + 1);
        }

        return TensorApi.Multiply(Complex.OneHalf, sumBuilder.Build());
    }

    private Tensor TraceWithout5(Tensor product, int numberOfGammas)
    {
        product = GetTraceSubstitution(numberOfGammas).Transform(product);
        product = EliminateMetricsTransformation.Eliminate(product);
        product = DeltaTrace.Transform(product);
        product = TraceOfOne.Transform(product);
        return product;
    }

    private Tensor TraceWith5(Tensor product, int numberOfGammas)
    {
        EnsureGamma5Transformations();

        if (numberOfGammas == 5)
        {
            product = _traceOf4GammasWith5!.Transform(product);
        }
        else
        {
            product = _chiholmKahaneIdentityReversed!.Transform(product);
            product = GetTraceSubstitution(numberOfGammas + 1).Transform(product);
        }

        product = ExpandAndEliminate.Transform(product);
        product = DeltaTrace.Transform(product);
        product = TraceOfOne.Transform(product);

        if (_simplifyLeviCivita is not null)
        {
            product = _simplifyLeviCivita.Transform(product);
            product = DeltaTrace.Transform(product);
            product = TraceOfOne.Transform(product);
        }

        return product;
    }

    private void EnsureGamma5Transformations()
    {
        if (_traceOf4GammasWith5 is not null)
        {
            return;
        }

        _traceOf4GammasWith5 = (Expression)TokenTransformer.Transform(TraceOf4GammasWith5Token).ToTensor();
        _chiholmKahaneIdentity = (Expression)TokenTransformer.Transform(ChiholmKahaneToken).ToTensor();
        _chiholmKahaneIdentityReversed = (Expression)TokenTransformer.Transform(ChiholmKahaneTokenReversed).ToTensor();
        _chiholmKahaneIdentityReversed = (Expression)DeltaTrace.Transform(_chiholmKahaneIdentityReversed);
    }

    private static Tensor[] SubArray(Tensor[] array, int first, int second)
    {
        Tensor[] result = new Tensor[array.Length - 2];
        int index = 0;
        for (int i = 0; i < array.Length; ++i)
        {
            if (i == first || i == second)
            {
                continue;
            }

            result[index++] = array[i];
        }

        return result;
    }

    private static void Swap(Tensor[] array, int first, int second)
    {
        Tensor temp = array[first];
        array[first] = array[second];
        array[second] = temp;
    }

    private static Tensor GetIndexlessSubProduct(Product product)
    {
        if (product.IndexlessData.Length == 0)
        {
            return product.Factor;
        }

        if (product.Factor == Complex.One && product.IndexlessData.Length == 1)
        {
            return product.IndexlessData[0];
        }

        List<Tensor> factors = [];
        if (product.Factor != Complex.One)
        {
            factors.Add(product.Factor);
        }

        factors.AddRange(product.IndexlessData);
        return TensorApi.Multiply(factors);
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

        return TensorApi.Multiply(product.Data);
    }
}

file sealed class TraceGraphIndicator : IIndicator<GraphType>
{
    public bool Is(GraphType @object)
    {
        return @object == GraphType.Cycle;
    }
}
