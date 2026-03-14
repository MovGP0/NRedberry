using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BigIntegerTests
{
    [Fact]
    public void ShouldParseAndApplyBasicArithmetic()
    {
        BigInteger left = BigInteger.Parse("42");
        BigInteger right = new(-12);

        left.Sum(right).ToString().ShouldBe("30");
        left.Subtract(right).ToString().ShouldBe("54");
        right.Negate().Multiply(new BigInteger(7)).ToString().ShouldBe("84");
        right.Abs().ToString().ShouldBe("12");
    }

    [Fact]
    public void ShouldComputeGreatestCommonDivisor()
    {
        BigInteger gcd = new BigInteger(84).Gcd(new BigInteger(30));

        gcd.ToString().ShouldBe("6");
    }
}
