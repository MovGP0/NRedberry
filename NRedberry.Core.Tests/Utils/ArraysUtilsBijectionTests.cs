using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsBijectionTests
{
    [Fact]
    public void ShouldReturnIdentityBijectionForEqualArrays()
    {
        int[]? actual = ArraysUtils.Bijection([1, 3, 1], [1, 3, 1]);

        actual.ShouldBe([0, 1, 2]);
    }

    [Fact]
    public void ShouldReturnBijectionForPermutedDuplicates()
    {
        int[]? actual = ArraysUtils.Bijection([1, 3, 1], [3, 1, 1]);

        actual.ShouldBe([1, 0, 2]);
    }

    [Fact]
    public void ShouldReturnNullWhenLengthsDiffer()
    {
        int[]? actual = ArraysUtils.Bijection([1, 2], [1, 2, 3]);

        actual.ShouldBeNull();
    }
}
