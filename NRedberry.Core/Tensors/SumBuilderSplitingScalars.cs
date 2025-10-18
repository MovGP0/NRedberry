namespace NRedberry.Core.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/SumBuilderSplitingScalars.java
 */

public sealed class SumBuilderSplitingScalars : AbstractSumBuilder
{
    public SumBuilderSplitingScalars()
    {
    }

    public SumBuilderSplitingScalars(int initialCapacity)
        : base(initialCapacity)
    {
    }

    public override Tensor Build()
    {
        throw new NotImplementedException();
    }

    public override void Put(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    protected override Split Split(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public override TensorBuilder Clone()
    {
        throw new NotImplementedException();
    }
}
