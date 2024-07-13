using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class TanFactory : ScalarFunctionFactory
{
    public static readonly TanFactory Factory = new();

    private TanFactory() { }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is ArcTan)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Complex.Zero;
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.Tan((Complex)arg);
        return new Tan(arg);
    }
}