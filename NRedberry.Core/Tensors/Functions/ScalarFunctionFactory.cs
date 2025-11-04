namespace NRedberry.Core.Tensors.Functions;

public abstract class ScalarFunctionFactory : TensorFactory
{
    public virtual Tensor Create(params Tensor[] tensor)
    {
        if (tensor.Length != 1)
        {
            throw new ArgumentException(nameof(tensor));
        }

        if (tensor[0] == null)
        {
            throw new ArgumentNullException(nameof(tensor));
        }

        if (!TensorUtils.IsScalar(tensor[0]))
        {
            throw new ArgumentException(nameof(tensor));
        }

        return Create1(tensor[0]);
    }

    protected abstract Tensor Create1(Tensor tensor);
}
