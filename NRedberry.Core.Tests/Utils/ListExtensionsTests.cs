using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ListExtensionsTests
{
    [Fact]
    public void ShouldCloneListIndependently()
    {
        List<int> original = [1, 2, 3];

        List<int> clone = original.Clone();
        clone[0] = 9;

        Assert.Equal(new[] { 1, 2, 3 }, original);
        Assert.Equal(new[] { 9, 2, 3 }, clone);
    }

    [Fact]
    public void ShouldRemoveItemsAfterSpecifiedIndex()
    {
        List<int> values = [1, 2, 3, 4];

        values.RemoveAfter(1);

        Assert.Equal(new[] { 1, 2 }, values);
    }
}
