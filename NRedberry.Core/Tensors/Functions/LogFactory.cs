using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class LogFactory : ScalarFunctionFactory
{
    public static readonly LogFactory Factory = new();

    private LogFactory() { }

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is Exp) // TODO: Log[Power[E,x]] = x
            return arg[0];
        if (TensorUtils.IsOne(arg))
            return Complex.Zero;
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.Log((Complex)arg);
        return new Log(arg);
    }
}