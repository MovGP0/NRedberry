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

        right.ShouldBe(left);
    }

    [Fact]
    public void ShouldProduceOrderIndependentRangeHash()
    {
        object?[] values = ["a", 1, null, 2];

        int left = ArraysUtils.CommutativeHashCode(values, 0, 3);
        int right = ArraysUtils.CommutativeHashCode([1, null, "a"], 0, 3);

        right.ShouldBe(left);
    }

    [Fact]
    public void ShouldCompareIntegerArraysByValue()
    {
        ArraysUtils.Equals([1, 2, 3], [1, 2, 3]).ShouldBeTrue();
        ArraysUtils.Equals([1, 2, 3], [1, 2, 4]).ShouldBeFalse();
        ArraysUtils.Equals([1, 2, 3], null).ShouldBeFalse();
    }
}
