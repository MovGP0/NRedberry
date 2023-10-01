namespace NRedberry.Core.Tensors;

public interface TensorFactory
{
    Tensor Create(Tensor tensor);
}