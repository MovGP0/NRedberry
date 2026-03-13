using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BigComplexTests
{
    [Fact]
    public void ShouldSupportConjugationNormAndMultiplication()
    {
        BigComplex value = new(new BigRational(1, 2), new BigRational(2, 3));
        BigComplex conjugate = value.Conjugate();
        BigComplex product = value.Multiply(conjugate);

        Assert.Equal("1/2i-2/3", conjugate.ToString());
        Assert.Equal("25/36", value.Norm().ToString());
        Assert.Equal("25/36", product.ToString());
    }

    [Fact]
    public void ShouldInvertImaginaryUnit()
    {
        BigComplex inverse = BigComplex.ImaginaryUnit.Inverse();

        Assert.Equal("-1", inverse.GetIm().ToString());
        Assert.Equal("0", inverse.GetRe().ToString());
    }
}
