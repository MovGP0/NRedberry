using NRedberry.Contexts;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class LongExtensionsTests
{
    [Theory(DisplayName = "Should count set bits in long values")]
    [InlineData(0L, 0)]
    [InlineData(1L, 1)]
    [InlineData(3L, 2)]
    [InlineData(11L, 3)]
    [InlineData(unchecked((long)0xF0F0F0F0F0F0F0F0UL), 32)]
    [InlineData(long.MaxValue, 63)]
    [InlineData(long.MinValue, 1)]
    [InlineData(-1L, 64)]
    public void ShouldCountSetBits(long value, int expected)
    {
        // Act
        int result = value.BitCount();

        // Assert
        result.ShouldBe(expected);
    }

    [Theory(DisplayName = "Should count trailing zero bits in long values")]
    [InlineData(0L, 64)]
    [InlineData(1L, 0)]
    [InlineData(2L, 1)]
    [InlineData(8L, 3)]
    [InlineData(40L, 3)]
    [InlineData(long.MinValue, 63)]
    public void ShouldCountTrailingZeros(long value, int expected)
    {
        // Act
        int result = value.NumberOfTrailingZeros();

        // Assert
        result.ShouldBe(expected);
    }
}
