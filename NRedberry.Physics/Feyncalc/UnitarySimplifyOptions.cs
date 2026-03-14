using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.UnitarySimplifyOptions.
/// </summary>
public sealed class UnitarySimplifyOptions
{
    private static readonly SimpleTensor DefaultUnitaryMatrix = TensorFactory.ParseSimple("T_A");
    private static readonly SimpleTensor DefaultStructureConstant = TensorFactory.ParseSimple("f_ABC");
    private static readonly SimpleTensor DefaultSymmetricConstant = TensorFactory.ParseSimple("d_ABC");
    private static readonly Tensor DefaultDimension = TensorFactory.Parse("N");

    public SimpleTensor UnitaryMatrix { get; set; } = DefaultUnitaryMatrix;

    public SimpleTensor StructureConstant { get; set; } = DefaultStructureConstant;

    public SimpleTensor SymmetricConstant { get; set; } = DefaultSymmetricConstant;

    public Tensor Dimension { get; set; } = DefaultDimension;
}
