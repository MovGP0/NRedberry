using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public sealed class ArcCosFactory : ScalarFunctionFactory
{
    public static readonly ArcCosFactory Factory = new();

    private ArcCosFactory()
    {
    }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is Cos)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Tensors.Parse("pi/2");
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.ArcCos((Complex)arg);
        return new ArcCos(arg);
    }
}
