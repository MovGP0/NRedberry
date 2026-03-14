using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.UnitarySimplifyOptions.
/// </summary>
public sealed class UnitarySimplifyOptions
{
    public SimpleTensor UnitaryMatrix { get; set; } = TensorFactory.ParseSimple("T_A");

    public SimpleTensor StructureConstant { get; set; } = TensorFactory.ParseSimple("f_ABC");

    public SimpleTensor SymmetricConstant { get; set; } = TensorFactory.ParseSimple("d_ABC");

    public Tensor Dimension { get; set; } = TensorFactory.Parse("N");
}
