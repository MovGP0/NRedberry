using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class ArcTanFactory : ScalarFunctionFactory
{
    public static readonly ScalarFunctionFactory Factory = new ArcTanFactory();

    private ArcTanFactory()
    {
    }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is Tan)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Complex.Zero;
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.ArcTan((Complex)arg);
        return new ArcTan(arg);
    }
}
