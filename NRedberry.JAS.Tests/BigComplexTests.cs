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

        conjugate.ToString().ShouldBe("1/2i-2/3");
        value.Norm().ToString().ShouldBe("25/36");
        product.ToString().ShouldBe("25/36");
    }

    [Fact]
    public void ShouldInvertImaginaryUnit()
    {
        BigComplex inverse = BigComplex.ImaginaryUnit.Inverse();

        inverse.GetIm().ToString().ShouldBe("-1");
        inverse.GetRe().ToString().ShouldBe("0");
    }
}
