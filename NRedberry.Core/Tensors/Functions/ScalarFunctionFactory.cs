namespace NRedberry.Core.Tensors.Functions;

public abstract class ScalarFunctionFactory : TensorFactory
{
    public abstract Tensor Create(Tensor tensor);
}