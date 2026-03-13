using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class THashSetTest
{
    [Fact]
    public void ShouldStoreUniqueValues()
    {
        THashSet<string> values = new();

        Assert.True(values.Add("a"));
        Assert.False(values.Add("a"));
        Assert.True(values.Add("b"));
        Assert.Contains("a", values);
        Assert.Equal(2, values.Count);
    }
}
