namespace NRedberry.Core.Tensors;

public interface TensorFactory
{
    Tensor Create(params Tensor[] tensor);
}