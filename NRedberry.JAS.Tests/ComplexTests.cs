using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;
using PolyComplex = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly.Complex<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>;
using PolyComplexRing = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly.ComplexRing<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>;

namespace NRedberry.JAS.Tests;

public sealed class ComplexTests
{
    [Fact]
    public void ShouldSupportGenericComplexArithmetic()
    {
        PolyComplexRing ring = new(new BigRational());
        PolyComplex left = new(ring, new BigRational(1), new BigRational(2));
        PolyComplex right = new(ring, new BigRational(3), new BigRational(-1));

        Assert.Equal("4i1", left.Sum(right).ToString());
        Assert.Equal("5i5", left.Multiply(right).ToString());
        Assert.Equal("1i-2", left.Conjugate().ToString());
        Assert.Equal("5", left.Norm().ToString());
        Assert.Equal("0i-1", ring.Imag.Inverse().ToString());
        Assert.Equal("0i1", new PolyComplex(ring, 1).Sum(ring.Imag).Divide(new PolyComplex(ring, 1).Subtract(ring.Imag)).ToString());
    }

    [Fact]
    public void ShouldExposeImaginaryAndUnitSemantics()
    {
        PolyComplexRing ring = new(new BigRational());

        Assert.True(ring.One.IsOne());
        Assert.True(ring.Imag.IsImag());
        Assert.False(ring.Zero.IsUnit());
        Assert.True(ring.Imag.IsUnit());
    }
}
