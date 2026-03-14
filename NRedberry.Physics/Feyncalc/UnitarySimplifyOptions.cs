using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.UnitarySimplifyOptions.
/// </summary>
public sealed class UnitarySimplifyOptions
{
    public SimpleTensor UnitaryMatrix
    {
        get;
        set;
    }

    public SimpleTensor StructureConstant
    {
        get;
        set;
    }

    public SimpleTensor SymmetricConstant
    {
        get;
        set;
    }

    public Tensor Dimension
    {
        get;
        set;
    }

    public UnitarySimplifyOptions()
    {
        UnitaryMatrix = TensorFactory.ParseSimple("T_A");
        StructureConstant = TensorFactory.ParseSimple("f_ABC");
        SymmetricConstant = TensorFactory.ParseSimple("d_ABC");
        Dimension = TensorFactory.Parse("N");
    }
}
