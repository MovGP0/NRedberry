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

        map["a"].ShouldBe(1);
        map.ContainsKey("b").ShouldBeTrue();
        map.Count.ShouldBe(2);
    }
}
