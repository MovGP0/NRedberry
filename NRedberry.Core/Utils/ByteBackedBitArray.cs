using System;

namespace NRedberry.Core.Utils;

[Obsolete("use System.Collections.BitArray instead")]
public sealed class ByteBackedBitArray : IBitArray, IEquatable<ByteBackedBitArray>
{
    public static ByteBackedBitArray Empty = new ByteBackedBitArray(0);

    public ByteBackedBitArray(params bool[] bits)
    {
        throw new NotSupportedException();
    }

    public ByteBackedBitArray(int size)
    {
        throw new NotSupportedException();
    }

    public ByteBackedBitArray(bool[] data, int size)
    {
        throw new NotSupportedException();
    }

    public bool this[int i]
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public void Clear(int i)
    {
        throw new NotSupportedException();
    }

    public void Set(ByteBackedBitArray ba)
    {
        throw new NotSupportedException();
    }

    public void SetAll()
    {
        throw new NotSupportedException();
    }

    public void Set(int i)
    {
        throw new NotSupportedException();
    }

    public int BitCount()
    {
        throw new NotSupportedException();
    }

    public void Or(IBitArray bitArray)
    {
        throw new NotSupportedException();
    }

    public void Xor(IBitArray bitArray)
    {
        throw new NotSupportedException();
    }

    public void And(IBitArray bitArray)
    {
        throw new NotSupportedException();
    }

    public bool Intersects(IBitArray bitArray)
    {
        throw new NotSupportedException();
    }

    public void LoadValueFrom(IBitArray bitArray)
    {
        throw new NotSupportedException();
    }

    public void ClearAll()
    {
        throw new NotSupportedException();
    }

    public IBitArray Clone()
    {
        throw new NotSupportedException();
    }

    public int[] GetBits()
    {
        throw new NotSupportedException();
    }

    public int Size => throw new NotSupportedException();

    public override bool Equals(object obj)
    {
        throw new NotSupportedException();
    }

    public bool Equals(ByteBackedBitArray other)
    {
        throw new NotSupportedException();
    }

    public override int GetHashCode()
    {
        throw new NotSupportedException();
    }

    public override string ToString()
    {
        throw new NotSupportedException();
    }

    public int NextTrailingBit(int position)
    {
        throw new NotSupportedException();
    }
}