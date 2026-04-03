using System;
using NRedberry.Contexts;
using NRedberry.Core.Utils;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class LongBackedBitArrayTests
{
    [Fact(DisplayName = "Should set and clear bits")]
    public void ShouldSetAndClearBits()
    {
        // Arrange
        var array = new LongBackedBitArray(130);

        // Act
        array.Set(0);
        array.Set(64);
        array.Set(129);
        array.Clear(64);

        // Assert
        array[0].ShouldBeTrue();
        array[64].ShouldBeFalse();
        array[129].ShouldBeTrue();
        array.BitCount().ShouldBe(2);
    }

    [Fact(DisplayName = "Should set all and clear all bits")]
    public void ShouldSetAllAndClearAllBits()
    {
        // Arrange
        var array = new LongBackedBitArray(130);

        // Act
        array.SetAll();

        // Assert
        array.BitCount().ShouldBe(130);
        array[0].ShouldBeTrue();
        array[129].ShouldBeTrue();

        // Act
        array.ClearAll();

        // Assert
        array.BitCount().ShouldBe(0);
        array[0].ShouldBeFalse();
        array[129].ShouldBeFalse();
    }

    [Fact(DisplayName = "Should build expected bit string")]
    public void ShouldBuildExpectedBitString()
    {
        // Arrange
        var array = new LongBackedBitArray(5);

        // Act
        array.Set(0);
        array.Set(2);
        array.Set(4);

        // Assert
        array.ToString().ShouldBe("10101");
    }

    [Fact(DisplayName = "Should return set bits in order")]
    public void ShouldReturnSetBitsInOrder()
    {
        // Arrange
        var array = new LongBackedBitArray(10);

        // Act
        array.Set(7);
        array.Set(0);
        array.Set(9);
        int[] bits = array.GetBits();

        // Assert
        bits.ShouldBe([0, 7, 9]);
    }

    [Fact(DisplayName = "Should compute and, or, xor operations")]
    public void ShouldComputeAndOrXorOperations()
    {
        // Arrange
        var left = new LongBackedBitArray(128);
        var right = new LongBackedBitArray(128);
        left.Set(0);
        left.Set(64);
        right.Set(1);
        right.Set(64);

        // Act
        var andResult = (LongBackedBitArray)left.Clone();
        andResult.And(right);

        var orResult = (LongBackedBitArray)left.Clone();
        orResult.Or(right);

        var xorResult = (LongBackedBitArray)left.Clone();
        xorResult.Xor(right);

        // Assert
        andResult.GetBits().ShouldBe([64]);
        orResult.GetBits().ShouldBe([0, 1, 64]);
        xorResult.GetBits().ShouldBe([0, 1]);
    }

    [Fact(DisplayName = "Should clone and load values from another array")]
    public void ShouldCloneAndLoadValues()
    {
        // Arrange
        var source = new LongBackedBitArray(70);
        source.Set(1);
        source.Set(68);

        // Act
        var clone = (LongBackedBitArray)source.Clone();
        source.Clear(1);

        // Assert
        clone[1].ShouldBeTrue();
        clone[68].ShouldBeTrue();
        source[1].ShouldBeFalse();

        // Act
        var target = new LongBackedBitArray(70);
        target.LoadValueFrom(source);

        // Assert
        target[1].ShouldBeFalse();
        target[68].ShouldBeTrue();
    }

    [Fact(DisplayName = "Should detect intersections")]
    public void ShouldDetectIntersections()
    {
        // Arrange
        var left = new LongBackedBitArray(64);
        var right = new LongBackedBitArray(64);

        // Act
        left.Set(10);
        right.Set(20);

        // Assert
        left.Intersects(right).ShouldBeFalse();

        // Act
        right.Set(10);

        // Assert
        left.Intersects(right).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should find next trailing bit")]
    public void ShouldFindNextTrailingBit()
    {
        // Arrange
        var array = new LongBackedBitArray(130);
        array.Set(0);
        array.Set(3);
        array.Set(65);

        // Act
        int first = array.NextTrailingBit(0);
        int second = array.NextTrailingBit(1);
        int third = array.NextTrailingBit(4);
        int none = array.NextTrailingBit(66);

        // Assert
        first.ShouldBe(0);
        second.ShouldBe(3);
        third.ShouldBe(65);
        none.ShouldBe(-1);
    }

    [Fact(DisplayName = "Should throw for invalid trailing bit input")]
    public void ShouldThrowForInvalidTrailingBitInput()
    {
        // Arrange
        var array = new LongBackedBitArray(10);

        // Act + Assert
        Should.Throw<ArgumentOutOfRangeException>(() => array.NextTrailingBit(-1));
    }

    [Fact(DisplayName = "Should throw for size mismatch or invalid array type")]
    public void ShouldThrowForSizeMismatchOrInvalidArrayType()
    {
        // Arrange
        var left = new LongBackedBitArray(10);
        var right = new LongBackedBitArray(11);
        var other = new FakeBitArray();

        // Act + Assert
        Should.Throw<ArgumentException>(() => left.And(right));
        Should.Throw<ArgumentException>(() => left.Or(right));
        Should.Throw<ArgumentException>(() => left.Xor(right));
        Should.Throw<ArgumentException>(() => left.Intersects(right));
        Should.Throw<ArgumentException>(() => left.LoadValueFrom(right));
        Should.Throw<ArgumentException>(() => left.LoadValueFrom(other));
    }

    [Fact(DisplayName = "Should return indexed bit for Get")]
    public void ShouldReturnIndexedBitForGet()
    {
        // Arrange
        var array = new LongBackedBitArray(10);
        array.Set(1);

        // Act + Assert
        array.Get(1).ShouldBeTrue();
        array.Get(2).ShouldBeFalse();
    }

#pragma warning disable CS0618
    private sealed class FakeBitArray : IBitArray
    {
        public int Size => 0;

        public void And(IBitArray bitArray)
        {
        }

        public int BitCount()
        {
            return 0;
        }

        public void Clear(int i)
        {
        }

        public void ClearAll()
        {
        }

        public IBitArray Clone()
        {
            return this;
        }

        public bool this[int i]
        {
            get => false;
            set
            {
            }
        }

        public void Set(int i)
        {
        }

        public int[] GetBits()
        {
            return [];
        }

        public bool Intersects(IBitArray bitArray)
        {
            return false;
        }

        public void LoadValueFrom(IBitArray bitArray)
        {
        }

        public void Or(IBitArray bitArray)
        {
        }

        public void SetAll()
        {
        }

        public void Xor(IBitArray bitArray)
        {
        }

        public int NextTrailingBit(int position)
        {
            return -1;
        }

        public bool Get(byte type)
        {
            return false;
        }
    }
#pragma warning restore CS0618
}
