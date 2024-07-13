using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class ArcSinFactory : ScalarFunctionFactory
{
    public static readonly ArcSinFactory Factory = new();

    private ArcSinFactory()
    {
    }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is Sin)
        {
            return arg[0];
        }

        if (TensorUtils.IsZero(arg))
        {
            return Complex.Zero;
        }

        if (TensorUtils.IsNumeric(arg))
        {
            return ComplexUtils.ArcSin((Complex)arg);
        }

        return new ArcSin(arg);
    }
}
