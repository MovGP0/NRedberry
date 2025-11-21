namespace NRedberry.Tensors;

public static class TensorExtensions
{
    public static Tensor Pow(this Tensor argument, Tensor power)
    {
        var pb = new PowerBuilder();
        pb.Put(argument);
        pb.Put(power);
        return pb.Build();
    }

    public static Tensor Multiply(params Tensor[] factors)
    {
        return ProductFactory.Factory.Create(factors);
    }

    public static Tensor Multiply(this Tensor left, Tensor right)
    {
        return Multiply([left, right]);
    }

    public static Tensor Sum(params Tensor[] summands)
    {
        return SumFactory.Factory.Create(summands);
    }
}
