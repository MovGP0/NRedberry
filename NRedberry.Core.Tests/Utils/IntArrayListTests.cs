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

        list.ToArray().ShouldBe(new[] { 1, 2, 3 });
        list.Remove(2).ShouldBeTrue();
        list.ToArray().ShouldBe(new[] { 1, 3 });

        IntArrayList clone = list.Clone();

        clone.ShouldBe(list);
        clone.ToString().ShouldBe("[1, 3]");
    }

    [Fact]
    public void ShouldRemoveAfterSpecifiedPoint()
    {
        IntArrayList list = new(new[] { 1, 2, 3, 4 });

        list.RemoveAfter(2);

        list.ToArray().ShouldBe(new[] { 1, 2 });
    }
}
