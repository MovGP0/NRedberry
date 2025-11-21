using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public class ArcTan(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return Tensors.Sum(Complex.One, Argument.Pow(Complex.Two)).Pow(Complex.MinusOne);
    }

    protected override string FunctionName() => "ArcTan";

    public override int GetHashCode() => 2321 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(ArcTanFactory.Factory);

    public override TensorFactory GetFactory() => ArcTanFactory.Factory;
}
