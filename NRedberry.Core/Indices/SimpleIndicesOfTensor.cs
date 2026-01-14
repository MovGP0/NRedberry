namespace NRedberry.Indices;

public sealed class SimpleIndicesOfTensor : SimpleIndicesAbstract
{
    public SimpleIndicesOfTensor(int[] data, IndicesSymmetries? symmetries)
        : base(data, symmetries!)
    {
    }

    public SimpleIndicesOfTensor(bool notResort, int[] data, IndicesSymmetries? symmetries)
        : base(notResort, data, symmetries)
    {
    }

    protected override SimpleIndices Create(int[] data, IndicesSymmetries? symmetries)
    {
        return new SimpleIndicesOfTensor(true, data, symmetries);
    }

    public new IndicesSymmetries Symmetries
    {
        get => symmetries ?? throw new InvalidOperationException("Symmetries are not set.");
        set => throw new NotSupportedException();
    }
}
