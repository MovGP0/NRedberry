using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BigRationalTests
{
    [Fact]
    public void ShouldNormalizeFractionsAndParseDecimals()
    {
        BigRational normalized = new(2, 4);
        BigRational parsed = new("1.25");

        normalized.ToString().ShouldBe("1/2");
        parsed.ToString().ShouldBe("5/4");
    }

    [Fact]
    public void ShouldSupportArithmeticAndInverse()
    {
        BigRational left = new(1, 3);
        BigRational right = new(1, 6);

        left.Sum(right).ToString().ShouldBe("1/2");
        left.Multiply(right).ToString().ShouldBe("1/18");
        left.Inverse().ToString().ShouldBe("3");
    }
}
