using Shouldly;
using Xunit;

namespace NRedberry.Core.Utils.Tests;

public sealed class HashFunctionsTests
{
    [Fact(DisplayName = "Should expose HashFunctions type")]
    public void ShouldExposeHashFunctionsType()
    {
        // Arrange
        Type type = typeof(HashFunctions);

        // Act
        object? result = type;

        // Assert
        result.ShouldNotBeNull();
    }
}
