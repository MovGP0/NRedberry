using Shouldly;
using Xunit;

namespace NRedberry.Apache.Commons.Math.Tests;

public sealed class IFieldElementTests
{
    [Fact(DisplayName = "Should support arithmetic through interface")]
    public void ShouldSupportArithmeticThroughInterface()
    {
        IFieldElement<BigFraction> element = new BigFraction(1, 2);

        element.Add(new BigFraction(1, 3)).Equals(new BigFraction(5, 6)).ShouldBeTrue();
        element.Subtract(new BigFraction(1, 6)).Equals(new BigFraction(1, 3)).ShouldBeTrue();
        element.Multiply(new BigFraction(2, 3)).Equals(new BigFraction(1, 3)).ShouldBeTrue();
        element.Multiply(4).Equals(new BigFraction(2, 1)).ShouldBeTrue();
        element.Divide(new BigFraction(2, 5)).Equals(new BigFraction(5, 4)).ShouldBeTrue();
        element.Negate().Equals(new BigFraction(-1, 2)).ShouldBeTrue();
        element.Reciprocal().Equals(new BigFraction(2, 1)).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should expose field through interface")]
    public void ShouldExposeFieldThroughInterface()
    {
        IFieldElement<BigFraction> element = new BigFraction(1, 2);

        element.Field.ShouldBeOfType<BigFractionField>();
        element.Field.GetRuntimeClass().ShouldBe(typeof(BigFraction));
    }
}
