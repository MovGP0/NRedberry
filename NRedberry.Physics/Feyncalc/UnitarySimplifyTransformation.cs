using System.Text.RegularExpressions;
using NRedberry.Contexts;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using ContextCC = NRedberry.Contexts.CC;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.UnitarySimplifyTransformation.
/// </summary>
public sealed class UnitarySimplifyTransformation : TransformationToStringAble
{
    private readonly ITransformation _unitarySimplifications;

    private static readonly Parser Parser;
    private static readonly ParseToken Contraction1Token;
    private static readonly ParseToken Contraction2Token;
    private static readonly ParseToken SymmetricCombinationToken;
    private static readonly ParseToken SymmetricTraceToken;
    private static readonly ParseToken ASymmetricCombinationToken;
    private static readonly ParseToken ASymmetricTraceToken;
    private static readonly ParseToken SymmetrySimplificationToken;
    private static readonly ParseToken NumberOfGeneratorsToken;
    private static readonly ParseToken DimensionToken;
    private static readonly ParseToken[] UnitarySimplificationsTokens;

    private static readonly Regex s_dimensionTokenPattern = new(
        $@"(?<![A-Za-z0-9_]){TraceUtils.DimensionName}(?![A-Za-z0-9_])",
        RegexOptions.Compiled);

    static UnitarySimplifyTransformation()
    {
        Parser = ContextCC.Current.ParseManager.Parser;
        Contraction1Token = Parser.Parse("T_a^a'_b'*T^ab'_c' = (N**2-1)/(2*N)*d^a'_c'");
        Contraction2Token = Parser.Parse("T_a^a'_b'*T_b^b'_c'*T^ac'_d' = -T_b^a'_d'/(2*N)");
        SymmetricCombinationToken = Parser.Parse("D_apq*D_b^pq = (N**2 - 4)/N * g_ab");
        SymmetricTraceToken = Parser.Parse("D_a^ab = 0");
        ASymmetricTraceToken = Parser.Parse("F_a^ab = 0");
        ASymmetricCombinationToken = Parser.Parse("F_apq*F_b^pq = N * g_ab");
        SymmetrySimplificationToken = Parser.Parse("F_apq*D_b^pq = 0");
        NumberOfGeneratorsToken = Parser.Parse("d^a_a = N**2-1");
        DimensionToken = Parser.Parse("d^a'_a' = N");
        UnitarySimplificationsTokens =
        [
            Contraction1Token,
            Contraction2Token,
            SymmetricCombinationToken,
            ASymmetricCombinationToken,
            SymmetricTraceToken,
            ASymmetricTraceToken,
            SymmetrySimplificationToken,
            NumberOfGeneratorsToken,
            DimensionToken
        ];
    }

    public UnitarySimplifyTransformation(UnitarySimplifyOptions options)
        : this(options.UnitaryMatrix, options.StructureConstant, options.SymmetricConstant, options.Dimension)
    {
    }

    public UnitarySimplifyTransformation(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        TraceUtils.CheckUnitaryInput(unitaryMatrix, structureConstant, symmetricConstant, dimension);
        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(unitaryMatrix);
        var tokenTransformer = new ChangeIndicesTypesAndTensorNames(
            new UnitaryTypesAndNamesTransformer(
                types[0],
                types[1],
                unitaryMatrix,
                structureConstant,
                symmetricConstant,
                dimension));
        _unitarySimplifications = CreateUnitarySimplifications(dimension, tokenTransformer);
    }

    internal UnitarySimplifyTransformation(Tensor dimension, ChangeIndicesTypesAndTensorNames tokenTransformer)
    {
        ArgumentNullException.ThrowIfNull(dimension);
        ArgumentNullException.ThrowIfNull(tokenTransformer);

        _unitarySimplifications = CreateUnitarySimplifications(dimension, tokenTransformer);
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Tensor current = tensor;
        while (true)
        {
            Tensor transformed = _unitarySimplifications.Transform(current);
            if (ReferenceEquals(transformed, current))
            {
                return transformed;
            }

            current = transformed;
        }
    }

    public string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "UnitarySimplify";
    }

    public override string ToString()
    {
        return ToString(ContextCC.DefaultOutputFormat);
    }

    private static void CheckUnitaryInput(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        TraceUtils.CheckUnitaryInput(unitaryMatrix, structureConstant, symmetricConstant, dimension);
    }

    private static ITransformation CreateUnitarySimplifications(Tensor dimension, ChangeIndicesTypesAndTensorNames tokenTransformer)
    {
        List<ITransformation> simplifications =
        [
            EliminateMetricsTransformation.Instance
        ];

        foreach (ParseToken substitution in UnitarySimplificationsTokens)
        {
            simplifications.Add(CreateTransformation(substitution, tokenTransformer, dimension));
        }

        return new TransformationCollection(simplifications);
    }

    private static ITransformation CreateTransformation(
        ParseToken substitution,
        ChangeIndicesTypesAndTensorNames tokenTransformer,
        Tensor dimension)
    {
        ArgumentNullException.ThrowIfNull(substitution);
        ArgumentNullException.ThrowIfNull(tokenTransformer);
        ArgumentNullException.ThrowIfNull(dimension);

        Tensor transformed = tokenTransformer.Transform(substitution).ToTensor();
        if (dimension is Complex)
        {
            string expression = transformed.ToString(OutputFormat.Redberry);
            string replacedExpression = s_dimensionTokenPattern.Replace(
                expression,
                dimension.ToString(OutputFormat.Redberry));
            transformed = TensorFactory.Parse(replacedExpression);
        }

        return (ITransformation)transformed;
    }
}

file sealed record UnitaryTypesAndNamesTransformer(
    IndexType MetricType,
    IndexType MatrixType,
    SimpleTensor UnitaryMatrix,
    SimpleTensor StructureConstant,
    SimpleTensor SymmetricConstant,
    Tensor Dimension)
    : TypesAndNamesTransformer
{
    public int NewIndex(int oldIndex, NameAndStructureOfIndices descriptor)
    {
        _ = descriptor;
        return oldIndex;
    }

    public IndexType NewType(IndexType oldType, NameAndStructureOfIndices descriptor)
    {
        _ = descriptor;

        return oldType switch
        {
            IndexType.LatinLower => MetricType,
            IndexType.Matrix1 => MatrixType,
            _ => oldType
        };
    }

    public string NewName(string oldName, NameAndStructureOfIndices descriptor)
    {
        return oldName switch
        {
            TraceUtils.UnitaryMatrixName => UnitaryMatrix.GetStringName(),
            TraceUtils.StructureConstantName => StructureConstant.GetStringName(),
            TraceUtils.SymmetricConstantName => SymmetricConstant.GetStringName(),
            TraceUtils.DimensionName when Dimension is not Complex => Dimension.ToString(OutputFormat.Redberry),
            _ => descriptor.Name
        };
    }
}
