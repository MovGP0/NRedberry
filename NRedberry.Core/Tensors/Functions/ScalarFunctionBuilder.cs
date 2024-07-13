using System;

namespace NRedberry.Core.Tensors.Functions;

public sealed class ScalarFunctionBuilder : TensorBuilder
{
    private readonly ScalarFunctionFactory factory;
    private Tensor arg;

    public ScalarFunctionBuilder(ScalarFunctionFactory factory)
    {
        this.factory = factory;
    }

    public ScalarFunctionBuilder(ScalarFunctionFactory factory, Tensor arg)
    {
        this.factory = factory;
        this.arg = arg;
    }

    public Tensor Build()
    {
        return factory.Create(arg);
    }

    public void Put(Tensor tensor)
    {
        if (arg != null)
            throw new InvalidOperationException("Argument already set.");
        if (tensor == null)
            throw new ArgumentNullException(nameof(tensor));
        if (!TensorUtils.IsScalar(tensor))
            throw new ArgumentException("Tensor must be scalar.");
        arg = tensor;
    }

    public TensorBuilder Clone()
    {
        return new ScalarFunctionBuilder(factory, arg);
    }
}
