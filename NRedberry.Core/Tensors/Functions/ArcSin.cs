using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public class ArcSin(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return TensorExtensions.Sum(Complex.One, Argument.Pow(Complex.Two)).Pow(Complex.MinusOneHalf);
    }

    protected override string FunctionName()
    {
        return "ArcSin";
    }

    public override int GetHashCode()
    {
        return 92837 * Argument.GetHashCode();
    }

    public override TensorBuilder GetBuilder()
    {
        return new ScalarFunctionBuilder(ArcSinFactory.Factory);
    }

    public override TensorFactory GetFactory()
    {
        return ArcSinFactory.Factory;
    }
}
