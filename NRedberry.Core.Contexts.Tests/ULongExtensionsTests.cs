using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ULongExtensionsTests
{
    [Theory(DisplayName = "Should count set bits in unsigned longs")]
    [InlineData(0UL, 0)]
    [InlineData(1UL, 1)]
    [InlineData(3UL, 2)]
    [InlineData(11UL, 3)]
    [InlineData(0xF0F0F0F0F0F0F0F0UL, 32)]
    [InlineData(ulong.MaxValue, 64)]
    public void ShouldCountSetBits(ulong value, int expected)
    {
        // Act
        int result = ULongExtensions.BitCount(value);

        // Assert
        result.ShouldBe(expected);
    }
}
