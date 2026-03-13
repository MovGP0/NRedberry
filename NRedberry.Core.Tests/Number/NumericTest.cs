using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class NumericTest
{
    [Fact]
    public void ShouldComputeHashCode()
    {
        Assert.Equal(new Numeric(2.3).GetHashCode(), -new Numeric(-2.3).GetHashCode());
        Assert.Equal(new Numeric(23).GetHashCode(), -new Numeric(-23).GetHashCode());
        Assert.Equal(new Numeric(23.324234e123).GetHashCode(), -new Numeric(-23.324234e123).GetHashCode());
    }

    [Fact]
    public void ShouldComputeStaticHashCodes()
    {
        Assert.Equal(0, Numeric.Zero.GetHashCode());
        Assert.Equal(1, Numeric.One.GetHashCode());
        Assert.Equal(-1, Numeric.MinusOne.GetHashCode());
    }
}
