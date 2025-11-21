namespace NRedberry.Tensors;

public interface TensorFactory
{
    Tensor Create(params Tensor[] tensor);
}
