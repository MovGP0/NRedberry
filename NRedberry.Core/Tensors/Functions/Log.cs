using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public class Log : ScalarFunction
{
    public Log(Tensor argument)
        : base(argument)
    {
    }

    public override Tensor Derivative()
    {
        return Argument.Pow(Complex.MinusOne);
    }

    protected override string FunctionName() => "Log";

    public override int GetHashCode() => 13 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(LogFactory.Factory);

    public override TensorFactory GetFactory() => LogFactory.Factory;
}
