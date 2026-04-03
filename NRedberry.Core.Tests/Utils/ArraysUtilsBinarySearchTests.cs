using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsBinarySearchTests
{
    [Fact]
    public void ShouldSearchIListUsingFrameworkBinarySearch()
    {
        int actual = ArraysUtils.BinarySearch([1, 3, 5, 7], 5);

        actual.ShouldBe(2);
    }

    [Fact]
    public void ShouldReturnFirstMatchingIndexForDuplicates()
    {
        int actual = ArraysUtils.BinarySearch1([1, 2, 2, 2, 4], 2);

        actual.ShouldBe(1);
    }

    [Fact]
    public void ShouldReturnInsertionIndexWhenValueIsMissing()
    {
        int actual = ArraysUtils.BinarySearch1([1, 2, 4, 4, 7], 3);

        actual.ShouldBe(2);
    }
}
