using NRedberry.Concurrent;

namespace NRedberry.Core.Tests.Concurrent;

#pragma warning disable CS0618
public sealed class SingletonTests
{
    [Fact]
    public void ShouldReturnElementOnce()
    {
        Singleton<string> singleton = new("value");

        string? first = singleton.Take();
        string? second = singleton.Take();

        first.ShouldBe("value");
        second.ShouldBeNull();
    }
}
#pragma warning restore CS0618
