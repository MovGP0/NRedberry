using System.Numerics;
using NRedberry.Numbers;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class NumberUtilsTest
{
    [Fact]
    public void ShouldComputeSquareRoot()
    {
        NumberUtils.Sqrt(new BigInteger(9)).ShouldBe(new BigInteger(3));
    }
}
