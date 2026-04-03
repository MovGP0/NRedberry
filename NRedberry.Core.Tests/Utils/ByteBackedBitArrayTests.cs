#pragma warning disable CS0618
using NRedberry.Contexts;
using NRedberry.Core.Utils;

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

        array[0].ShouldBeTrue();
        array[31].ShouldBeFalse();
        array[69].ShouldBeTrue();
        array.BitCount().ShouldBe(2);
        array.GetBits().ShouldBe([0, 69]);
        array.ToString().ShouldBe("1" + new string('0', 68) + "1");
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

        andResult.GetBits().ShouldBe([10]);
        orResult.GetBits().ShouldBe([0, 10, 11]);
        xorResult.GetBits().ShouldBe([0, 11]);
        left.Intersects(right).ShouldBeTrue();
    }

    [Fact]
    public void ShouldCloneLoadAndCompareValues()
    {
        ByteBackedBitArray source = new([true, false, true, false], 4);
        ByteBackedBitArray clone = (ByteBackedBitArray)source.Clone();
        source.Clear(0);

        clone[0].ShouldBeTrue();
        source[0].ShouldBeFalse();
        clone.ShouldNotBe(source);

        ByteBackedBitArray target = new(4);
        target.LoadValueFrom(clone);

        target.ShouldBe(clone);
        target.GetHashCode().ShouldBe(clone.GetHashCode());
        target.Clone().ShouldBe(target);
    }

    [Fact]
    public void ShouldSetAllClearAllAndFindTrailingBits()
    {
        ByteBackedBitArray array = new(35);

        array.SetAll();
        array.BitCount().ShouldBe(35);

        array.ClearAll();
        array.Set(3);
        array.Set(34);

        array.NextTrailingBit(0).ShouldBe(3);
        array.NextTrailingBit(4).ShouldBe(34);
        array.NextTrailingBit(35).ShouldBe(-1);
    }

    [Fact]
    public void ShouldThrowForInvalidInputs()
    {
        ByteBackedBitArray array = new(4);

        Should.Throw<ArgumentOutOfRangeException>(() => array.Set(-1));
        Should.Throw<ArgumentOutOfRangeException>(() => array.Clear(4));
        Should.Throw<ArgumentOutOfRangeException>(() => array.NextTrailingBit(-1));
        Should.Throw<ArgumentException>(() => new ByteBackedBitArray([true], 2));
        Should.Throw<ArgumentException>(() => array.And(new ByteBackedBitArray(5)));
        Should.Throw<ArgumentException>(() => array.LoadValueFrom(new LongBackedBitArray(4)));
    }
}
#pragma warning restore CS0618
