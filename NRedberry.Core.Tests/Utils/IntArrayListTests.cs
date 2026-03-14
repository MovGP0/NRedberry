using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IntArrayListTests
{
    [Fact]
    public void ShouldAddInsertRemoveAndClone()
    {
        IntArrayList list =
        [
            1,
            3
        ];
        list.Insert(1, 2);

        Assert.Equal(new[] { 1, 2, 3 }, list.ToArray());
        Assert.True(list.Remove(2));
        Assert.Equal(new[] { 1, 3 }, list.ToArray());

        IntArrayList clone = list.Clone();

        Assert.Equal(list, clone);
        Assert.Equal("[1, 3]", clone.ToString());
    }

    [Fact]
    public void ShouldRemoveAfterSpecifiedPoint()
    {
        IntArrayList list = new(new[] { 1, 2, 3, 4 });

        list.RemoveAfter(2);

        Assert.Equal(new[] { 1, 2 }, list.ToArray());
    }
}
