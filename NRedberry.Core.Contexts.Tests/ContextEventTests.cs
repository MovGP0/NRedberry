using NRedberry.Contexts;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ContextEventTests
{
    [Fact(DisplayName = "Should create context event")]
    public void ShouldCreateContextEvent()
    {
        var evt = new ContextEvent();

        evt.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Should support value equality")]
    public void ShouldSupportValueEquality()
    {
        var first = new ContextEvent();
        var second = new ContextEvent();

        first.ShouldBe(second);
    }
}
