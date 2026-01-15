namespace NRedberry.Tensors.Functions;

public abstract class ScalarFunctionFactory : TensorFactory
{
    public virtual Tensor Create(params Tensor[] tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor.Length != 1)
        {
            throw new ArgumentException(nameof(tensor));
        }

        ArgumentNullException.ThrowIfNull(tensor[0]);

        if (!TensorUtils.IsScalar(tensor[0]))
        {
            throw new ArgumentException(nameof(tensor));
        }

        return Create1(tensor[0]);
    }

    protected abstract Tensor Create1(Tensor tensor);
}
