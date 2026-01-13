using Shouldly;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class CombinatoricsTests
{
    [Fact(DisplayName = "Should expose combinatorics type")]
    public void ShouldExposeCombinatoricsType()
    {
        // Arrange
        Type type = typeof(Combinatorics);

        // Act
        object? result = type;

        // Assert
        result.ShouldNotBeNull();
    }
}
