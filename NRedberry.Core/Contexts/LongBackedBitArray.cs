using System;
using System.Linq;
using NRedberry.Core.Utils;

namespace NRedberry.Contexts;

public sealed class LongBackedBitArray : IBitArray
{
    private long[] Data { get; }
    public int Size { get; }

    public LongBackedBitArray(int size)
    {
        Size = size;
        Data = new long[(size + 63) >> 6];
    }

    private LongBackedBitArray(long[] data, int size)
    {
        Data = data;
        Size = size;
    }

    public void And(IBitArray bitArray)
    {
        var longBackedBitArray = (LongBackedBitArray)bitArray;
        if (Size != longBackedBitArray.Size) throw new ArgumentException();

        for (var i = 0; i < Data.Length; ++i)
            Data[i] &= longBackedBitArray.Data[i];
    }

    public int BitCount()
    {
        return Data.Sum(value => value.BitCount());
    }

    public void Clear(int i)
    {
        Data[i >> 6] &= ~(1L << (i & 0x3F));
    }

    public void ClearAll()
    {
        Data.Fill(0, Data.Length, 0);
    }

    public IBitArray Clone()
    {
        return new LongBackedBitArray((long[])Data.Clone(), Size);
    }

    public bool this[int i]
    {
        get => (Data[i >> 6] & (1L << (i & 0x3F))) != 0;
        set
        {
            if (value)
            {
                Set(i);
            }
            else
            {
                Clear(i);
            }
        }
    }

    public void Set(int i)
    {
        Data[i >> 6] |= (1L << (i & 0x3F));
    }

    public int[] GetBits()
    {
        var bits = new int[BitCount()];
        var n = 0;
        for (var i = 0; i < Size; ++i)
        {
            if (this[i])
            {
                bits[n++] = i;
            }
        }

        return bits;
    }

    public bool Intersects(IBitArray bitArray)
    {
        var longBackedBitArray = (LongBackedBitArray)bitArray;
        if (longBackedBitArray.Size != Size) throw new ArgumentException();
        return Data.Where((t, i) => (t & longBackedBitArray.Data[i]) != 0).Any();
    }

    public void LoadValueFrom(IBitArray bitArray)
    {
        if (!(bitArray is LongBackedBitArray longBackedBitArray))
        {
            throw new ArgumentException();
        }

        if (Size != longBackedBitArray.Size) throw new ArgumentException();
        Array.Copy(longBackedBitArray.Data, 0, Data, 0, Data.Length);
    }

    public void Or(IBitArray bitArray)
    {
        var longBackedBitArray = (LongBackedBitArray)bitArray;
        if (Size != longBackedBitArray.Size) throw new ArgumentException();

        for (var i = 0; i < Data.Length; ++i)
            Data[i] |= longBackedBitArray.Data[i];
    }

    public void SetAll()
    {
        Data.Fill(0, Data.Length, long.MaxValue);
        Data[Data.Length - 1] &= long.MaxValue >> ((Data.Length << 6) - Size);
    }

    public void Xor(IBitArray bitArray)
    {
        var longBackedBitArray = (LongBackedBitArray)bitArray;
        if (Size != longBackedBitArray.Size) throw new ArgumentException();

        for (var i = 0; i < Data.Length; ++i)
        {
            Data[i] ^= longBackedBitArray.Data[i];
        }
    }

    public int NextTrailingBit(int position)
    {
        if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), position, "must not be negative");

        var firstShift = position & 0x3F;
        var pointer = position >> 6;
        int result;

        if ((result = (Data[pointer++] >> firstShift).NumberOfTrailingZeros()) != 64)
            return position + result;

        while (pointer < Data.Length && (result = Data[pointer++].NumberOfTrailingZeros()) == 64)
        {
        }

        if (result == 64) return -1;
        return (pointer - 1) * 64 + result;
    }

    public override string ToString()
    {
        var c = new char[Size];
        for (var i = 0; i < Size; ++i)
        {
            c[i] = this[i] ? '1' : '0';
        }

        return new string(c);
    }

    public bool Get(byte type)
    {
        throw new NotImplementedException();
    }
}