using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public class Cot : ScalarFunction
{
    public Cot(Tensor argument)
        : base(argument)
    {
    }

    public override Tensor Derivative()
    {
        return new Sin(Argument).Pow(Complex.MinusTwo);
    }

    protected override string FunctionName() => "Cot";

    public override int GetHashCode() => 19 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(CotFactory.Factory);

    public override TensorFactory GetFactory() => CotFactory.Factory;
}
