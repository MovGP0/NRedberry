using NRedberry.Tensors;

namespace NRedberry.Numbers;

internal sealed class ComplexBuilder : TensorBuilder
{
    private readonly Complex complex;

    public ComplexBuilder(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        this.complex = complex;
    }

    public Tensor Build()
    {
        return complex;
    }

    public void Put(Tensor tensor)
    {
        throw new InvalidOperationException("Can not put to Complex tensor builder!");
    }

    public TensorBuilder Clone()
    {
        return this;
    }
}
