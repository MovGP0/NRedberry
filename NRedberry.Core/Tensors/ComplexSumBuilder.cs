using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors;

internal class ComplexSumBuilder : TensorBuilder
{
    private Complex Complex = Complex.Zero;

    public ComplexSumBuilder()
    {
    }

    public ComplexSumBuilder(Complex complex)
    {
        Complex = complex;
    }

    public Tensor Build()
    {
        return Complex;
    }

    public void Put(Tensor tensor)
    {
        Complex = Complex.Add((Complex) tensor);
    }

    public TensorBuilder Clone()
    {
        return new ComplexSumBuilder(Complex);
    }
}
