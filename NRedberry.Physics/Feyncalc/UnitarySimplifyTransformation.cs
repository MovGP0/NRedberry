using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.UnitarySimplifyTransformation.
/// </summary>
public sealed class UnitarySimplifyTransformation : TransformationToStringAble
{
    private readonly ITransformation unitarySimplifications = null!;

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
    private static readonly ParseToken Triple1Token;
    private static readonly ParseToken Triple2Token;
    private static readonly ParseToken Triple3Token;
    private static readonly ParseToken Triple4Token;
    private static readonly ParseToken[] UnitarySimplificationsTokens;

    static UnitarySimplifyTransformation()
    {
        Parser = null!;
        Contraction1Token = null!;
        Contraction2Token = null!;
        SymmetricCombinationToken = null!;
        SymmetricTraceToken = null!;
        ASymmetricCombinationToken = null!;
        ASymmetricTraceToken = null!;
        SymmetrySimplificationToken = null!;
        NumberOfGeneratorsToken = null!;
        DimensionToken = null!;
        Triple1Token = null!;
        Triple2Token = null!;
        Triple3Token = null!;
        Triple4Token = null!;
        UnitarySimplificationsTokens = [];
    }

    public UnitarySimplifyTransformation(UnitarySimplifyOptions options)
        : this(options.UnitaryMatrix, options.StructureConstant, options.SymmetricConstant, options.Dimension)
    {
        throw new NotImplementedException();
    }

    public UnitarySimplifyTransformation(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    private static void CheckUnitaryInput(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        throw new NotImplementedException();
    }
}
