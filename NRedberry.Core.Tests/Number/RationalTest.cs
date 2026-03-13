using System.Numerics;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class RationalTest
{
    [Fact]
    public void ShouldComputeHashCode()
    {
        Rational positive = new(2, 3);
        Rational sameValueDifferentSigns = new(-2, -3);
        Rational negative = new(-2, 3);
        Rational hugePositive = new(new BigInteger(1231231231876239486L), BigInteger.Parse("123242342342342342331231231876239486"));
        Rational hugeEquivalent = new(new BigInteger(-1231231231876239486L), BigInteger.Parse("-123242342342342342331231231876239486"));
        Rational hugeNegative = new(new BigInteger(1231231231876239486L), BigInteger.Parse("-123242342342342342331231231876239486"));

        Assert.Equal(positive.GetHashCode(), sameValueDifferentSigns.GetHashCode());
        Assert.NotEqual(positive.GetHashCode(), negative.GetHashCode());
        Assert.Equal(hugePositive.GetHashCode(), hugeEquivalent.GetHashCode());
        Assert.NotEqual(hugePositive.GetHashCode(), hugeNegative.GetHashCode());
    }

    [Fact]
    public void ShouldComputeStaticHashCodes()
    {
        Assert.Equal(new Rational(0).GetHashCode(), Rational.Zero.GetHashCode());
        Assert.Equal(new Rational(1).GetHashCode(), Rational.One.GetHashCode());
        Assert.Equal(new Rational(-1).GetHashCode(), Rational.MinusOne.GetHashCode());
    }
}
