using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BigComplexTests
{
    [Fact(DisplayName = "Should expose BigComplex type")]
    public void ShouldExposeBigComplexType()
    {
        // Arrange
        Type type = typeof(BigComplex);

        // Act
        object? result = type;

        // Assert
        result.ShouldNotBeNull();
    }
}
