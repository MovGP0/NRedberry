using System.Numerics;

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

        sameValueDifferentSigns.GetHashCode().ShouldBe(positive.GetHashCode());
        negative.GetHashCode().ShouldNotBe(positive.GetHashCode());
        hugeEquivalent.GetHashCode().ShouldBe(hugePositive.GetHashCode());
        hugeNegative.GetHashCode().ShouldNotBe(hugePositive.GetHashCode());
    }

    [Fact]
    public void ShouldComputeStaticHashCodes()
    {
        Rational.Zero.GetHashCode().ShouldBe(new Rational(0).GetHashCode());
        Rational.One.GetHashCode().ShouldBe(new Rational(1).GetHashCode());
        Rational.MinusOne.GetHashCode().ShouldBe(new Rational(-1).GetHashCode());
    }
}
