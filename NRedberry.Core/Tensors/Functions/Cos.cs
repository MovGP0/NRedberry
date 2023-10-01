using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class Cos : ScalarFunction
{
    public Cos(Tensor argument) : base(argument)
    {
    }

    public override Tensor Derivative() => Complex.MinusOne.Multiply(new Sin(Argument));

    protected override int Hash() => 11 * Argument.GetHashCode();

    protected override string FunctionName() => "Cos";

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(CosFactory.Factory);

    public override TensorFactory GetFactory() => CosFactory.Factory;
}