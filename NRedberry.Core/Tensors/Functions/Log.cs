using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public class Log(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return Argument.Pow(Complex.MinusOne);
    }

    protected override string FunctionName() => "Log";

    public override int GetHashCode() => 13 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(LogFactory.Factory);

    public override TensorFactory GetFactory() => LogFactory.Factory;
}
