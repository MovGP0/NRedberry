using NRedberry.Contexts;
using NRedberry.Contexts.Defaults;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class DefaultContextFactoryTests
{
    [Fact(DisplayName = "Should expose singleton instance")]
    public void ShouldExposeSingletonInstance()
    {
        DefaultContextFactory.Instance.ShouldBeSameAs(DefaultContextFactory.Instance);
    }

    [Fact(DisplayName = "Should create new contexts with default settings")]
    public void ShouldCreateNewContextsWithDefaultSettings()
    {
        // Act
        Context first = DefaultContextFactory.Instance.CreateContext();
        Context second = DefaultContextFactory.Instance.CreateContext();

        // Assert
        first.ShouldNotBeNull();
        second.ShouldNotBeNull();
        first.ShouldNotBeSameAs(second);
    }
}
