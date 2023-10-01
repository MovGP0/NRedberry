namespace NRedberry.Core.Tensors.Functions;

public sealed class Sin : ScalarFunction
{
    public Sin(Tensor argument) : base(argument) { }

    public override Tensor Derivative()
    {
        return new Cos(Argument);
    }

    protected override int Hash()
    {
        return 7 * Argument.GetHashCode();
    }

    protected override string FunctionName()
    {
        return "Sin";
    }

    public override TensorBuilder GetBuilder()
    {
        return new ScalarFunctionBuilder(SinFactory.Factory);
    }

    public override TensorFactory GetFactory()
    {
        return SinFactory.Factory;
    }
}