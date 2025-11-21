namespace NRedberry.Tensors.Functions;

public class Exp(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative() => this;

    protected override string FunctionName() => "Exp";

    public override int GetHashCode() => 3 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(ExpFactory.Factory);

    public override TensorFactory GetFactory() => ExpFactory.Factory;
}
