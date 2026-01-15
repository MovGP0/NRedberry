namespace NRedberry.Tensors.Functions;

public sealed class ScalarFunctionBuilder : TensorBuilder
{
    private readonly ScalarFunctionFactory _factory;
    private Tensor? _arg;

    public ScalarFunctionBuilder(ScalarFunctionFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    public ScalarFunctionBuilder(ScalarFunctionFactory factory, Tensor? arg)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _arg = arg;
    }

    public Tensor Build()
    {
        if (_arg is null)
        {
            throw new InvalidOperationException("Argument is not set.");
        }

        return _factory.Create(_arg);
    }

    public void Put(Tensor tensor)
    {
        if (_arg is not null)
        {
            throw new InvalidOperationException("Argument already set.");
        }

        ArgumentNullException.ThrowIfNull(tensor);

        if (!TensorUtils.IsScalar(tensor))
        {
            throw new ArgumentException("Tensor must be scalar.");
        }

        _arg = tensor;
    }

    public TensorBuilder Clone()
    {
        return new ScalarFunctionBuilder(_factory, _arg);
    }
}
