using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class UnivPowerSeriesTests
{
    [Fact]
    public void ShouldSupportLazyArithmeticShiftAndFormatting()
    {
        UnivPowerSeriesRing<BigRational> ring = new(new BigRational(), 4, "t");
        UnivPowerSeries<BigRational> first = new(ring, new ArrayCoefficients([new BigRational(1), new BigRational(2)]));
        UnivPowerSeries<BigRational> second = new(ring, new ArrayCoefficients([new BigRational(0), new BigRational(1)]));

        UnivPowerSeries<BigRational> sum = first.Sum(second);
        UnivPowerSeries<BigRational> difference = first.Subtract(second);
        UnivPowerSeries<BigRational> shifted = first.Shift(1);
        UnivPowerSeries<BigRational> scaled = first.Multiply(new BigRational(3));

        sum.ToString().ShouldBe("1 + 3 * t + BigO(t^4)");
        difference.ToString().ShouldBe("1 + t + BigO(t^4)");
        shifted.ToString().ShouldBe("t + 2 * t^2 + BigO(t^4)");
        scaled.ToString().ShouldBe("3 + 6 * t + BigO(t^4)");
        first.Order().ShouldBe(0);
        shifted.Order().ShouldBe(1);
        first.Signum().ShouldBe(1);
    }

    [Fact]
    public void ShouldSupportInverseDivisionGcdAndRemainder()
    {
        UnivPowerSeriesRing<BigRational> ring = new(new BigRational(), 5, "t");
        UnivPowerSeries<BigRational> onePlusT = new(ring, new ArrayCoefficients([new BigRational(1), new BigRational(1)]));
        UnivPowerSeries<BigRational> inverse = onePlusT.Inverse();
        UnivPowerSeries<BigRational> dividend = onePlusT.Multiply(onePlusT);
        UnivPowerSeries<BigRational> quotient = dividend.Divide(onePlusT);
        UnivPowerSeries<BigRational> tSquared = ring.GetONE().Shift(2);
        UnivPowerSeries<BigRational> tCubed = ring.GetONE().Shift(3);
        UnivPowerSeries<BigRational> remainder = ring.GetONE().Shift(1).Remainder(tSquared);

        inverse.Coefficient(0).ToString().ShouldBe("1");
        inverse.Coefficient(1).ToString().ShouldBe("-1");
        inverse.Coefficient(2).ToString().ShouldBe("1");
        quotient.ToString().ShouldBe(onePlusT.ToString());
        tSquared.Gcd(tCubed).ToString().ShouldBe(tSquared.ToString());
        remainder.ToString().ShouldBe("t + BigO(t^5)");
        ring.GetZero().IsZero().ShouldBeTrue();
        ring.GetONE().IsOne().ShouldBeTrue();
    }
}

file sealed class ArrayCoefficients(BigRational[] values) : Coefficients<BigRational>
{
    protected override BigRational Generate(int index)
    {
        if (index < values.Length)
        {
            return values[index];
        }

        return BigRational.Zero;
    }
}
