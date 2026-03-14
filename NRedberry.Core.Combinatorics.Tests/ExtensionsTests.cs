using System.Collections;
using NRedberry.Core.Combinatorics.Extensions;
using Shouldly;
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

        bitArray.NextTrailingBit(4).ShouldBe(3);
        bitArray.NextTrailingBit(2).ShouldBe(1);
        bitArray.NextTrailingBit(0).ShouldBe(-1);
    }
}

public sealed class EnumeratorExtensionsTests
{
    [Fact]
    public void ShouldWrapSingleElementAsEnumerator()
    {
        IEnumerator<int> enumerator = 7.GetEnumerator();

        enumerator.MoveNext().ShouldBeTrue();
        enumerator.Current.ShouldBe(7);
        enumerator.MoveNext().ShouldBeFalse();
    }
}
