using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IntArrayTests
{
    [Fact]
    public void ShouldExposeCopyAndEnumeration()
    {
        IntArray array = new([1, 2, 3]);

        array.Length().ShouldBe(3);
        array.Get(1).ShouldBe(2);
        array.Copy().ShouldBe(new[] { 1, 2, 3 });
        array.Copy(1, 3).ShouldBe(new[] { 2, 3 });
        array.ToArray().ShouldBe(new[] { 1, 2, 3 });
    }

    [Fact]
    public void ShouldCompareByContent()
    {
        IntArray left = new([4, 5, 6]);
        IntArray right = new([4, 5, 6]);
        IntArray different = new([4, 5]);

        left.Equals(right).ShouldBeTrue();
        (left == right).ShouldBeTrue();
        (left != right).ShouldBeFalse();
        left.Equals(different).ShouldBeFalse();
        left.ToString().ShouldBe("[4, 5, 6]");
    }
}
