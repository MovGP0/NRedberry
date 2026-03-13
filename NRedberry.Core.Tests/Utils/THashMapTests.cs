using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class THashMapTests
{
    [Fact]
    public void ShouldStoreAndLookupValues()
    {
        THashMap<string, int> map = new();
        map["a"] = 1;
        map["b"] = 2;

        Assert.Equal(1, map["a"]);
        Assert.True(map.ContainsKey("b"));
        Assert.Equal(2, map.Count);
    }
}
