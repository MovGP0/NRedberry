using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using ContextCC = NRedberry.Contexts.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.DiracSimplify1.
/// </summary>
public sealed class DiracSimplify1 : AbstractFeynCalcTransformation
{
    private readonly Expression[] _substitutionsCache;
    private readonly Dictionary<int, Expression> _generatedSubstitutions = [];

    private static readonly Parser Parser;
    private static readonly ParseToken S1;
    private static readonly ParseToken S2;
    private static readonly ParseToken S3;
    private static readonly ParseToken S4;

    static DiracSimplify1()
    {
        Parser = ContextCC.Current.ParseManager.Parser;
        S1 = Parser.Parse("G_a^a'_b'*G^a^b'_c' = d^z_z*d^a'_c'");
        S2 = Parser.Parse("G_a^a'_b'*G_b^b'_c'*G^ac'_d' = -(d^z_z-2)*G_b^a'_d'");
        S3 = Parser.Parse("G_a^a'_b'*G_b^b'_c'*G_c^c'_d'*G^ad'_e' = 4*g_bc*d^a'_e' - (4-d^z_z)*G_b^a'_b'*G_c^b'_e'");
        S4 = Parser.Parse("G_a^a'_b'*G_b^b'_c'*G_c^c'_d'*G_d^d'_e'*G^ae'_f' = -2*G_d^a'_b'*G_c^b'_c'*G_b^c'_f' + (4-d^z_z)*G_b^a'_b'*G_c^b'_c'*G_d^c'_f'");
    }

    public DiracSimplify1(DiracOptions options)
        : base(options, Transformation.Identity)
    {
        ParseToken[] substitutions =
        [
            S1,
            S2,
            S3,
            S4
        ];

        _substitutionsCache = new Expression[substitutions.Length];
        for (int i = 0; i < substitutions.Length; ++i)
        {
            _substitutionsCache[i] = (Expression)DeltaTrace.Transform(TokenTransformer.Transform(substitutions[i]).ToTensor());
        }
    }

    public new string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "DiracSimplify1";
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
        List<int> lengths = [];
        for (int i = 0; i < length - 1; ++i)
        {
            Tensor gamma1 = productContent[productOfGammas.GPositions[i]];
            for (int j = i + 1; j < length; ++j)
            {
                Tensor gamma2 = productContent[productOfGammas.GPositions[j]];
                if (IndicesUtils.AreContracted(gamma1.Indices[MetricType, 0], gamma2.Indices[MetricType, 0]))
                {
                    lengths.Add(j - i + 1);
                }
            }
        }

        if (lengths.Count == 0)
        {
            return null;
        }

        lengths.Sort();

        ITransformation[] overall = new ITransformation[lengths.Count + 3];
        for (int i = lengths.Count - 1; i >= 0; --i)
        {
            overall[i] = CreateSubstitution(lengths[i]);
        }

        overall[lengths.Count] = ExpandAndEliminate;
        overall[lengths.Count + 1] = DeltaTrace;
        overall[lengths.Count + 2] = TraceOfOne;

        return Transform(
            Transformation.ApplyUntilUnchanged(
                TensorApi.Multiply(productOfGammas.ToArray()),
                overall));
    }

    private Expression CreateSubstitution(int length)
    {
        if (length <= 2)
        {
            return _substitutionsCache[length - 2];
        }

        if (_generatedSubstitutions.TryGetValue(length, out Expression? expression))
        {
            return expression;
        }

        Tensor[] line = CreateLine(length);
        line[length - 1] = SetMetricIndex(
            (SimpleTensor)line[length - 1],
            IndicesUtils.InverseIndexState(line[0].Indices[MetricType, 0]));
        expression = TensorApi.Expression(TensorApi.Multiply(line), CreateSubstitution0(line));
        _generatedSubstitutions[length] = expression;
        return expression;
    }

    private Tensor CreateSubstitution0(Tensor[] gammas)
    {
        gammas = Del(gammas, 0);
        gammas = Del(gammas, gammas.Length - 1);

        int length = gammas.Length;
        SumBuilder sumBuilder = new();
        if ((length & 1) == 1)
        {
            sumBuilder.Put(
                TensorApi.Multiply(
                    TensorApi.Subtract(Complex.Four, DeltaTrace[1]),
                    TensorApi.Multiply(gammas)));

            int[] indices = new int[length];
            for (int i = 0; i < length; ++i)
            {
                indices[i] = gammas[i].Indices[MetricType, 0];
            }

            for (int i = 0; i < length; ++i)
            {
                gammas[i] = SetMetricIndex((SimpleTensor)gammas[i], indices[length - i - 1]);
            }

            sumBuilder.Put(TensorApi.Multiply(Complex.MinusTwo, TensorApi.Multiply(gammas)));
        }
        else
        {
            sumBuilder.Put(
                TensorApi.Multiply(
                    TensorApi.Subtract(DeltaTrace[1], Complex.Four),
                    TensorApi.Multiply(gammas)));

            int[] indices = new int[length];
            for (int i = 0; i < length; ++i)
            {
                indices[i] = gammas[i].Indices[MetricType, 0];
            }

            Tensor[] shifted = (Tensor[])gammas.Clone();
            for (int i = 0; i < length - 1; ++i)
            {
                shifted[i + 1] = SetMetricIndex((SimpleTensor)gammas[i + 1], indices[i]);
            }

            shifted[0] = SetMetricIndex((SimpleTensor)gammas[0], indices[length - 1]);
            sumBuilder.Put(TensorApi.Multiply(Complex.Two, TensorApi.Multiply(shifted)));

            for (int i = 0; i < length - 1; ++i)
            {
                gammas[i] = SetMetricIndex((SimpleTensor)gammas[i], indices[length - i - 2]);
            }

            sumBuilder.Put(TensorApi.Multiply(Complex.Two, TensorApi.Multiply(gammas)));
        }

        return sumBuilder.Build();
    }
}
