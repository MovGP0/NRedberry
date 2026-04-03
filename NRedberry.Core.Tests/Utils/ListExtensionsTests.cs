using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class ListExtensionsTests
{
    [Fact]
    public void ShouldCloneListIndependently()
    {
        List<int> original = [1, 2, 3];

        List<int> clone = original.Clone();
        clone[0] = 9;

        original.ShouldBe(new[] { 1, 2, 3 });
        clone.ShouldBe(new[] { 9, 2, 3 });
    }

    [Fact]
    public void ShouldRemoveItemsAfterSpecifiedIndex()
    {
        List<int> values = [1, 2, 3, 4];

        values.RemoveAfter(1);

        values.ShouldBe(new[] { 1, 2 });
    }
}
