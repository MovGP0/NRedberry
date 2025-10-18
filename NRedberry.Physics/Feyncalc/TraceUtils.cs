using NRedberry.Core.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.TraceUtils.
/// </summary>
public static class TraceUtils
{
    public const string UnitaryMatrixName = "T";
    public const string StructureConstantName = "F";
    public const string SymmetricConstantName = "D";
    public const string DimensionName = "N";

    public static IndexType[] ExtractTypesFromMatrix(SimpleTensor matrix)
    {
        throw new NotImplementedException();
    }

    public static void CheckUnitaryInput(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        throw new NotImplementedException();
    }
}
