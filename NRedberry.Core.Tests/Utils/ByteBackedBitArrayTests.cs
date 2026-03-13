#pragma warning disable CS0618
using NRedberry.Contexts;
using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ByteBackedBitArrayTests
{
    [Fact]
    public void ShouldSetClearAndEnumerateBits()
    {
        ByteBackedBitArray array = new(70);

        array.Set(0);
        array.Set(31);
        array.Set(69);
        array.Clear(31);

        Assert.True(array[0]);
        Assert.False(array[31]);
        Assert.True(array[69]);
        Assert.Equal(2, array.BitCount());
        Assert.Equal([0, 69], array.GetBits());
        Assert.Equal("1" + new string('0', 68) + "1", array.ToString());
    }

    [Fact]
    public void ShouldApplyLogicalOperations()
    {
        ByteBackedBitArray left = new(64);
        ByteBackedBitArray right = new(64);
        left.Set(0);
        left.Set(10);
        right.Set(10);
        right.Set(11);

        ByteBackedBitArray andResult = (ByteBackedBitArray)left.Clone();
        andResult.And(right);

        ByteBackedBitArray orResult = (ByteBackedBitArray)left.Clone();
        orResult.Or(right);

        ByteBackedBitArray xorResult = (ByteBackedBitArray)left.Clone();
        xorResult.Xor(right);

        Assert.Equal([10], andResult.GetBits());
        Assert.Equal([0, 10, 11], orResult.GetBits());
        Assert.Equal([0, 11], xorResult.GetBits());
        Assert.True(left.Intersects(right));
    }

    [Fact]
    public void ShouldCloneLoadAndCompareValues()
    {
        ByteBackedBitArray source = new([true, false, true, false], 4);
        ByteBackedBitArray clone = (ByteBackedBitArray)source.Clone();
        source.Clear(0);

        Assert.True(clone[0]);
        Assert.False(source[0]);
        Assert.NotEqual(source, clone);

        ByteBackedBitArray target = new(4);
        target.LoadValueFrom(clone);

        Assert.Equal(clone, target);
        Assert.Equal(clone.GetHashCode(), target.GetHashCode());
        Assert.Equal(target, target.Clone());
    }

    [Fact]
    public void ShouldSetAllClearAllAndFindTrailingBits()
    {
        ByteBackedBitArray array = new(35);

        array.SetAll();
        Assert.Equal(35, array.BitCount());

        array.ClearAll();
        array.Set(3);
        array.Set(34);

        Assert.Equal(3, array.NextTrailingBit(0));
        Assert.Equal(34, array.NextTrailingBit(4));
        Assert.Equal(-1, array.NextTrailingBit(35));
    }

    [Fact]
    public void ShouldThrowForInvalidInputs()
    {
        ByteBackedBitArray array = new(4);

        Assert.Throws<ArgumentOutOfRangeException>(() => array.Set(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => array.Clear(4));
        Assert.Throws<ArgumentOutOfRangeException>(() => array.NextTrailingBit(-1));
        Assert.Throws<ArgumentException>(() => new ByteBackedBitArray([true], 2));
        Assert.Throws<ArgumentException>(() => array.And(new ByteBackedBitArray(5)));
        Assert.Throws<ArgumentException>(() => array.LoadValueFrom(new LongBackedBitArray(4)));
    }
}
#pragma warning restore CS0618
