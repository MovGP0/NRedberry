namespace NRedberry.Core.Tensors;

public interface TensorBuilder
{
    void Put(Tensor tensor);
    Tensor Build();
    TensorBuilder Clone();
}