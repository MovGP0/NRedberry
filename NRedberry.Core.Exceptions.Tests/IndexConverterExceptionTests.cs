using Shouldly;
using Xunit;

namespace NRedberry.Core.Exceptions.Tests;

public sealed class IndexConverterExceptionTests
{
    [Fact(DisplayName = "Should expose exception type")]
    public void ShouldExposeExceptionType()
    {
        // Arrange
        Type type = typeof(IndexConverterException);

        // Act
        object? result = type;

        // Assert
        result.ShouldNotBeNull();
    }
}
