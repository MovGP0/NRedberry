using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsBijectionTests
{
    [Fact]
    public void ShouldReturnIdentityBijectionForEqualArrays()
    {
        int[]? actual = ArraysUtils.Bijection([1, 3, 1], [1, 3, 1]);

        Assert.Equal([0, 1, 2], actual);
    }

    [Fact]
    public void ShouldReturnBijectionForPermutedDuplicates()
    {
        int[]? actual = ArraysUtils.Bijection([1, 3, 1], [3, 1, 1]);

        Assert.Equal([1, 0, 2], actual);
    }

    [Fact]
    public void ShouldReturnNullWhenLengthsDiffer()
    {
        int[]? actual = ArraysUtils.Bijection([1, 2], [1, 2, 3]);

        Assert.Null(actual);
    }
}
