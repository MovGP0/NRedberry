using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class CosFactory : ScalarFunctionFactory
{
    public static readonly CosFactory Factory = new();

    private CosFactory()
    {
    }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is ArcCos)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Complex.One;
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.Cos((Complex)arg);
        return new Cos(arg);
    }
}
