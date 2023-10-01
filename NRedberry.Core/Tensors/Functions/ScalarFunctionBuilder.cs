using System;

namespace NRedberry.Core.Tensors.Functions;

public sealed class ScalarFunctionBuilder : TensorBuilder
{
    private readonly ScalarFunctionFactory _factory;
    private Tensor _arg;

    public ScalarFunctionBuilder(ScalarFunctionFactory factory)
    {
        _factory = factory;
    }

    public ScalarFunctionBuilder(ScalarFunctionFactory factory, Tensor arg)
    {
        _factory = factory;
        _arg = arg;
    }

    public Tensor Build()
    {
        return _factory.Create(_arg);
    }

    public void Put(Tensor tensor)
    {
        if (_arg != null)
            throw new InvalidOperationException("Argument already set.");
        if (tensor == null)
            throw new ArgumentNullException(nameof(tensor));
        if (!TensorUtils.IsScalar(tensor))
            throw new ArgumentException("Tensor must be scalar.");
        _arg = tensor;
    }

    public TensorBuilder Clone()
    {
        return new ScalarFunctionBuilder(_factory, _arg);
    }
}
