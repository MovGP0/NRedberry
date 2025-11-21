namespace NRedberry.Tensors;

public sealed class PowerBuilder : TensorBuilder
{
    private Tensor Argument { get; set; }
    private Tensor Power { get; set; }

    public PowerBuilder()
    {
    }

    public PowerBuilder(Tensor argument, Tensor power)
    {
        Argument = argument;
        Power = power;
    }

    public Tensor Build()
    {
        if (Power == null)
            throw new InvalidOperationException("Power is not fully constructed.");
        return PowerFactory.Power(Argument, Power);
    }

    public void Put(Tensor tensor)
    {
        if(tensor == null)
            throw new ArgumentNullException(nameof(tensor));
        if (!TensorUtils.IsScalar(tensor))
            throw new ArgumentException("Non-scalar tensor on input of Power builder.");

        if (Argument is null)
        {
            Argument = tensor;
            return;
        }

        if(Power is null)
        {
            Power = tensor;
            return;
        }

        throw new InvalidOperationException("Power buider can not take more than two put() invocations.");
    }

    public TensorBuilder Clone()
    {
        return new PowerBuilder(Argument, Power);
    }
}
