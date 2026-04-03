using NRedberry.Contexts;
using NRedberry.Contexts.Defaults;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class IContextFactoryTests
{
    [Fact(DisplayName = "Should allow custom context factory implementations")]
    public void ShouldAllowCustomContextFactoryImplementations()
    {
        // Arrange
        IContextFactory factory = new CustomContextFactory();

        // Act
        Context context = factory.CreateContext();

        // Assert
        context.ShouldNotBeNull();
    }

    private sealed class CustomContextFactory : IContextFactory
    {
        public Context CreateContext()
        {
            return new Context(DefaultContextSettings.Create());
        }
    }
}
