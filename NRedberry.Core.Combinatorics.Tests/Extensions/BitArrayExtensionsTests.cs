using System.Collections;
using NRedberry.Core.Combinatorics.Extensions;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Extensions;

public sealed class BitArrayExtensionsTests
{
    [Fact]
    public void ShouldReturnNearestSetBitSearchingForward()
    {
        BitArray bitArray = new(5)
        {
            [1] = true,
            [3] = true
        };

        bitArray.ShouldSatisfyAllConditions(
            ba => ba.NextTrailingBit(0).ShouldBe(1),
            ba => ba.NextTrailingBit(2).ShouldBe(3),
            ba => ba.NextTrailingBit(4).ShouldBe(-1));
    }
}
