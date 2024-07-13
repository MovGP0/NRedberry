using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class CotFactory : ScalarFunctionFactory
{
    public static readonly CotFactory Factory = new();

    private CotFactory() { }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is ArcCot)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Complex.ComplexPositiveInfinity;
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.Cot((Complex)arg);
        return new Cot(arg);
    }
}