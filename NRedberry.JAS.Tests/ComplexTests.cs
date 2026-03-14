using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Shouldly;
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

        left.Sum(right).ToString().ShouldBe("4i1");
        left.Multiply(right).ToString().ShouldBe("5i5");
        left.Conjugate().ToString().ShouldBe("1i-2");
        left.Norm().ToString().ShouldBe("5");
        ring.Imag.Inverse().ToString().ShouldBe("0i-1");
        new PolyComplex(ring, 1).Sum(ring.Imag).Divide(new PolyComplex(ring, 1).Subtract(ring.Imag)).ToString().ShouldBe("0i1");
    }

    [Fact]
    public void ShouldExposeImaginaryAndUnitSemantics()
    {
        PolyComplexRing ring = new(new BigRational());

        ring.One.IsOne().ShouldBeTrue();
        ring.Imag.IsImag().ShouldBeTrue();
        ring.Zero.IsUnit().ShouldBeFalse();
        ring.Imag.IsUnit().ShouldBeTrue();
    }
}
