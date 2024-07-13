using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class SinFactory : ScalarFunctionFactory
{
    public static readonly SinFactory Factory = new();

    private SinFactory() { }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is ArcSin arcSin)
            return arcSin[0];

        if (TensorUtils.IsZero(arg))
            return Complex.Zero;

        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.Sin((Complex)arg);
        return new Sin(arg);
    }
}