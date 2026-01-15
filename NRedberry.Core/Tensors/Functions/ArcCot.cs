using NRedberry.Numbers;

namespace NRedberry.Tensors.Functions;

public class ArcCot(Tensor argument) : ScalarFunction(argument)
{
    public override Tensor Derivative()
    {
        return Tensors.Sum(Complex.One, Argument.Pow(Complex.Two)).Pow(Complex.MinusOne).Multiply(Complex.MinusOne);
    }

    protected override string FunctionName() => "ArcCot";

    public override int GetHashCode() => 2311 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(ArcCotFactory.Factory);

    public override TensorFactory GetFactory() => ArcCotFactory.Factory;
}
