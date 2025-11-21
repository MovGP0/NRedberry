using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.UnitaryTraceTransformation.
/// </summary>
public sealed class UnitaryTraceTransformation : TransformationToStringAble
{
    private readonly ITransformation unitarySimplifications = null!;
    private readonly Expression pairProduct = null!;
    private readonly Expression singleTrace = null!;
    private readonly int unitaryMatrix;
    private readonly IndexType matrixType;

    private static readonly Parser Parser;
    private static readonly ParseToken PairProductToken;
    private static readonly ParseToken SingleTraceToken;

    static UnitaryTraceTransformation()
    {
        Parser = null!;
        PairProductToken = null!;
        SingleTraceToken = null!;
    }

    public UnitaryTraceTransformation(UnitarySimplifyOptions options)
        : this(options.UnitaryMatrix, options.StructureConstant, options.SymmetricConstant, options.Dimension)
    {
        throw new NotImplementedException();
    }

    public UnitaryTraceTransformation(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
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

    private Tensor TraceOfProduct(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private static bool IsUnitaryMatrixOrOne(Tensor tensor, int unitaryMatrix)
    {
        throw new NotImplementedException();
    }

    private static void CheckUnitaryInput(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        throw new NotImplementedException();
    }
}
