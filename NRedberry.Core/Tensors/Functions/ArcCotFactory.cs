using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class ArcCotFactory : ScalarFunctionFactory
{
    public static readonly ArcCotFactory Factory = new();

    protected override Tensor Create1(Tensor arg)
    {
        if (arg is Cot)
            return arg[0];
        if (TensorUtils.IsZero(arg))
            return Tensors.Parse("pi/2");
        if (TensorUtils.IsNumeric(arg))
            return ComplexUtils.ArcCot((Complex)arg);
        return new ArcCot(arg);
    }
}