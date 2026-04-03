namespace NRedberry.Core.Tests.Number;

public sealed class NumericTest
{
    [Fact]
    public void ShouldComputeHashCode()
    {
        (-new Numeric(-2.3).GetHashCode()).ShouldBe(new Numeric(2.3).GetHashCode());
        (-new Numeric(-23).GetHashCode()).ShouldBe(new Numeric(23).GetHashCode());
        (-new Numeric(-23.324234e123).GetHashCode()).ShouldBe(new Numeric(23.324234e123).GetHashCode());
    }

    [Fact]
    public void ShouldComputeStaticHashCodes()
    {
        Numeric.Zero.GetHashCode().ShouldBe(0);
        Numeric.One.GetHashCode().ShouldBe(1);
        Numeric.MinusOne.GetHashCode().ShouldBe(-1);
    }
}
