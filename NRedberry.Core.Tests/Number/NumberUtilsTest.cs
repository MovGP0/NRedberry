using System.Numerics;
using NRedberry.Numbers;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class NumberUtilsTest
{
    [Fact]
    public void ShouldComputeSquareRoot()
    {
        Assert.Equal(new BigInteger(3), NumberUtils.Sqrt(new BigInteger(9)));
    }
}
