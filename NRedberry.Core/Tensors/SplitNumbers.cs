namespace NRedberry.Core.Tensors;

internal class SplitNumbers : Split
{
    public SplitNumbers(Tensor factor, Tensor summand)
        : base(factor, summand)
    {
    }

    public override TensorBuilder GetBuilder()
    {
        TensorBuilder builder = new ComplexSumBuilder();
        builder.Put(Summand);
        return builder;
    }
}
