using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
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

        Assert.Equal("1 + 3 * t + BigO(t^4)", sum.ToString());
        Assert.Equal("1 + t + BigO(t^4)", difference.ToString());
        Assert.Equal("t + 2 * t^2 + BigO(t^4)", shifted.ToString());
        Assert.Equal("3 + 6 * t + BigO(t^4)", scaled.ToString());
        Assert.Equal(0, first.Order());
        Assert.Equal(1, shifted.Order());
        Assert.Equal(1, first.Signum());
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

        Assert.Equal("1", inverse.Coefficient(0).ToString());
        Assert.Equal("-1", inverse.Coefficient(1).ToString());
        Assert.Equal("1", inverse.Coefficient(2).ToString());
        Assert.Equal(onePlusT.ToString(), quotient.ToString());
        Assert.Equal(tSquared.ToString(), tSquared.Gcd(tCubed).ToString());
        Assert.Equal("t + BigO(t^5)", remainder.ToString());
        Assert.True(ring.GetZero().IsZero());
        Assert.True(ring.GetONE().IsOne());
    }
}

file sealed class ArrayCoefficients : Coefficients<BigRational>
{
    private readonly BigRational[] _values;

    public ArrayCoefficients(BigRational[] values)
    {
        _values = values;
    }

    protected override BigRational Generate(int index)
    {
        if (index < _values.Length)
        {
            return _values[index];
        }

        return BigRational.Zero;
    }
}
