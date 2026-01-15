using NRedberry.Numbers;

namespace NRedberry.Tensors;

internal sealed class ComplexSumBuilder : TensorBuilder
{
    private Complex _complex = Complex.Zero;

    public ComplexSumBuilder()
    {
    }

    public ComplexSumBuilder(Complex complex)
    {
        _complex = complex;
    }

    public Tensor Build()
    {
        return _complex;
    }

    public void Put(Tensor tensor)
    {
        _complex = _complex.Add((Complex) tensor);
    }

    public TensorBuilder Clone()
    {
        return new ComplexSumBuilder(_complex);
    }
}
