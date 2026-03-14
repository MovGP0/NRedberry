using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IntArrayTests
{
    [Fact]
    public void ShouldExposeCopyAndEnumeration()
    {
        IntArray array = new([1, 2, 3]);

        Assert.Equal(3, array.Length());
        Assert.Equal(2, array.Get(1));
        Assert.Equal(new[] { 1, 2, 3 }, array.Copy());
        Assert.Equal(new[] { 2, 3 }, array.Copy(1, 3));
        Assert.Equal(new[] { 1, 2, 3 }, array.ToArray());
    }

    [Fact]
    public void ShouldCompareByContent()
    {
        IntArray left = new([4, 5, 6]);
        IntArray right = new([4, 5, 6]);
        IntArray different = new([4, 5]);

        Assert.True(left.Equals(right));
        Assert.True(left == right);
        Assert.False(left != right);
        Assert.False(left.Equals(different));
        Assert.Equal("[4, 5, 6]", left.ToString());
    }
}
