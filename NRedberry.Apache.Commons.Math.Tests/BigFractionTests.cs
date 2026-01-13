using System.Numerics;
using Shouldly;
using Xunit;

namespace NRedberry.Apache.Commons.Math.Tests;

public sealed class BigFractionTests
{
    [Fact(DisplayName = "Should throw when denominator is zero")]
    public void ShouldThrowWhenDenominatorIsZero()
    {
        // Arrange
        BigInteger numerator = BigInteger.One;
        BigInteger denominator = BigInteger.Zero;

        // Act
        Action action = () => new BigFraction(numerator, denominator);

        // Assert
        action.ShouldThrow<ArgumentException>();
    }
}
