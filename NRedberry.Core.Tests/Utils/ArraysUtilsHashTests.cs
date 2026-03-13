using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsHashTests
{
    [Fact]
    public void ShouldProduceOrderIndependentCommutativeHash()
    {
        int left = ArraysUtils.CommutativeHashCode("a", 1, null);
        int right = ArraysUtils.CommutativeHashCode(1, null, "a");

        Assert.Equal(left, right);
    }

    [Fact]
    public void ShouldProduceOrderIndependentRangeHash()
    {
        object?[] values = ["a", 1, null, 2];

        int left = ArraysUtils.CommutativeHashCode(values, 0, 3);
        int right = ArraysUtils.CommutativeHashCode([1, null, "a"], 0, 3);

        Assert.Equal(left, right);
    }

    [Fact]
    public void ShouldCompareIntegerArraysByValue()
    {
        Assert.True(ArraysUtils.Equals([1, 2, 3], [1, 2, 3]));
        Assert.False(ArraysUtils.Equals([1, 2, 3], [1, 2, 4]));
        Assert.False(ArraysUtils.Equals([1, 2, 3], null));
    }
}
