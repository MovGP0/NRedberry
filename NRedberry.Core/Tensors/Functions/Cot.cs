using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public class Cot(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return new Sin(Argument).Pow(Complex.MinusTwo);
    }

    protected override string FunctionName() => "Cot";

    public override int GetHashCode() => 19 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(CotFactory.Factory);

    public override TensorFactory GetFactory() => CotFactory.Factory;
}
