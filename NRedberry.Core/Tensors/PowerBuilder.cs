namespace NRedberry.Tensors;

public sealed class PowerBuilder : TensorBuilder
{
    private Tensor? _argument;
    private Tensor? _power;

    public PowerBuilder()
    {
    }

    private PowerBuilder(Tensor? argument, Tensor? power)
    {
        _argument = argument;
        _power = power;
    }

    public Tensor Build()
    {
        if (_argument is null || _power is null)
        {
            throw new InvalidOperationException("Power is not fully constructed.");
        }

        return PowerFactory.Power(_argument, _power);
    }

    public void Put(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (!TensorUtils.IsScalar(tensor))
        {
            throw new ArgumentException("Non-scalar tensor on input of Power builder: " + tensor);
        }

        if (_argument is null)
        {
            _argument = tensor;
            return;
        }

        if (_power is null)
        {
            _power = tensor;
            return;
        }

        throw new InvalidOperationException("Power builder can not take more than two put() invocations.");
    }

    public TensorBuilder Clone()
    {
        return new PowerBuilder(_argument, _power);
    }
}
