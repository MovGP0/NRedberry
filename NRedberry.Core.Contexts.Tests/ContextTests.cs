using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ContextTests
{
    [Fact(DisplayName = "Should expose Context type")]
    public void ShouldExposeContextType()
    {
        // Arrange
        Type type = typeof(Context);

        // Act
        object? result = type;

        // Assert
        result.ShouldNotBeNull();
    }
}
