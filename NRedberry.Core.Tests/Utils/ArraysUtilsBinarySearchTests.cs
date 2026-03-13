using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsBinarySearchTests
{
    [Fact]
    public void ShouldSearchIListUsingFrameworkBinarySearch()
    {
        int actual = ArraysUtils.BinarySearch([1, 3, 5, 7], 5);

        Assert.Equal(2, actual);
    }

    [Fact]
    public void ShouldReturnFirstMatchingIndexForDuplicates()
    {
        int actual = ArraysUtils.BinarySearch1([1, 2, 2, 2, 4], 2);

        Assert.Equal(1, actual);
    }

    [Fact]
    public void ShouldReturnInsertionIndexWhenValueIsMissing()
    {
        int actual = ArraysUtils.BinarySearch1([1, 2, 4, 4, 7], 3);

        Assert.Equal(2, actual);
    }
}
