using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BigIntegerTests
{
    [Fact]
    public void ShouldParseAndApplyBasicArithmetic()
    {
        BigInteger left = BigInteger.Parse("42");
        BigInteger right = new(-12);

        Assert.Equal("30", left.Sum(right).ToString());
        Assert.Equal("54", left.Subtract(right).ToString());
        Assert.Equal("84", right.Negate().Multiply(new BigInteger(7)).ToString());
        Assert.Equal("12", right.Abs().ToString());
    }

    [Fact]
    public void ShouldComputeGreatestCommonDivisor()
    {
        BigInteger gcd = new BigInteger(84).Gcd(new BigInteger(30));

        Assert.Equal("6", gcd.ToString());
    }
}
