using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class ExpFactory : ScalarFunctionFactory
{
    public static readonly ExpFactory Factory = new();

    private ExpFactory()
    {
    }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is Log)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Complex.One;
        return new Exp(arg);
    }
}
