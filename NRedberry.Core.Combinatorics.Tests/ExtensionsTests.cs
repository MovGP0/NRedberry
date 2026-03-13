using System.Collections;
using NRedberry.Core.Combinatorics.Extensions;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class BitArrayExtensionsTests
{
    [Fact]
    public void ShouldReturnNearestSetBitSearchingBackwards()
    {
        BitArray bitArray = new(5);
        bitArray[1] = true;
        bitArray[3] = true;

        Assert.Equal(3, bitArray.NextTrailingBit(4));
        Assert.Equal(1, bitArray.NextTrailingBit(2));
        Assert.Equal(-1, bitArray.NextTrailingBit(0));
    }
}

public sealed class EnumeratorExtensionsTests
{
    [Fact]
    public void ShouldWrapSingleElementAsEnumerator()
    {
        IEnumerator<int> enumerator = 7.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(7, enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }
}
