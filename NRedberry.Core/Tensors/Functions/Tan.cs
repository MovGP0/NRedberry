using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public class Tan : ScalarFunction
{
    public Tan(Tensor argument)
        : base(argument)
    {
    }

    public override Tensor Derivative()
    {
        return new Cos(Argument).Pow(Complex.MinusTwo);
    }

    protected override string FunctionName() => "Tan";

    public override int GetHashCode() => 17 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(TanFactory.Factory);

    public override TensorFactory GetFactory() => TanFactory.Factory;
}
