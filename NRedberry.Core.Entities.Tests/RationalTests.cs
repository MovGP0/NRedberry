using Shouldly;
using Xunit;

namespace NRedberry.Core.Entities.Tests;

public sealed class RationalTests
{
    [Fact(DisplayName = "Should expose Rational type")]
    public void ShouldExposeRationalType()
    {
        // Arrange
        Type type = typeof(Rational);

        // Act
        object? result = type;

        // Assert
        result.ShouldNotBeNull();
    }
}
