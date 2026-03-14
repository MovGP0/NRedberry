using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class THashSetTest
{
    [Fact]
    public void ShouldStoreUniqueValues()
    {
        THashSet<string> values = [];

        values.Add("a").ShouldBeTrue();
        values.Add("a").ShouldBeFalse();
        values.Add("b").ShouldBeTrue();
        values.ShouldContain("a");
        values.Count.ShouldBe(2);
    }
}
