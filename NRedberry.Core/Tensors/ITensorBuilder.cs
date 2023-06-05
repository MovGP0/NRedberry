namespace NRedberry.Core.Tensors;

public interface ITensorBuilder
{
    void Put(Tensor tensor);
    Tensor Build();
    ITensorBuilder Clone();
}