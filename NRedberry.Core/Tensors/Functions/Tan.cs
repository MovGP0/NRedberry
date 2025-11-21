using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public class Tan(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return new Cos(Argument).Pow(Complex.MinusTwo);
    }

    protected override string FunctionName() => "Tan";

    public override int GetHashCode() => 17 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(TanFactory.Factory);

    public override TensorFactory GetFactory() => TanFactory.Factory;
}
