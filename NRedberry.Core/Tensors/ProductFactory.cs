namespace NRedberry.Core.Tensors;

public sealed class ProductFactory : TensorFactory
{
    public static ProductFactory Factory = new();

    public Tensor Create(params Tensor[] tensors)
    {
        throw new System.NotImplementedException();
    }

    public Tensor Create(Tensor tensor)
    {
        throw new System.NotImplementedException();
    }
}