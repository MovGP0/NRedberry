using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BigRationalTests
{
    [Fact]
    public void ShouldNormalizeFractionsAndParseDecimals()
    {
        BigRational normalized = new(2, 4);
        BigRational parsed = new("1.25");

        Assert.Equal("1/2", normalized.ToString());
        Assert.Equal("5/4", parsed.ToString());
    }

    [Fact]
    public void ShouldSupportArithmeticAndInverse()
    {
        BigRational left = new(1, 3);
        BigRational right = new(1, 6);

        Assert.Equal("1/2", left.Sum(right).ToString());
        Assert.Equal("1/18", left.Multiply(right).ToString());
        Assert.Equal("3", left.Inverse().ToString());
    }
}
