namespace NRedberry.Core.Tensors
{
    public interface ITensorFactory
    {
        Tensor Create(params Tensor[] tensors);
    }
}
