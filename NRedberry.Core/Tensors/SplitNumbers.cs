namespace NRedberry.Tensors;

internal class SplitNumbers(Tensor factor, Tensor summand) : Split(factor, summand)
{
    public override TensorBuilder GetBuilder()
    {
        TensorBuilder builder = new ComplexSumBuilder();
        builder.Put(Summand);
        return builder;
    }
}
