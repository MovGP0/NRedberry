namespace NRedberry.Tensors;

public sealed class SimpleTensorBuilder(SimpleTensor tensor) : TensorBuilder
{
    private SimpleTensor Tensor { get; } = tensor ?? throw new ArgumentNullException(nameof(tensor));

    public Tensor Build()
    {
        return Tensor;
    }

    public void Put(Tensor tensor)
    {
        throw new NotSupportedException("Can not put to SimpleTensor builder!");
    }

    public TensorBuilder Clone()
    {
        return this;
    }
}
