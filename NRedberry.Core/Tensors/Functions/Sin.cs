namespace NRedberry.Tensors.Functions;

public sealed class Sin(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return new Cos(Argument);
    }

    public override int GetHashCode()
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
