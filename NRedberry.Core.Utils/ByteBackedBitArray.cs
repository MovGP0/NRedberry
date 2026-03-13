using System.Numerics;

namespace NRedberry.Core.Utils;

[Obsolete("use System.Collections.BitArray instead")]
public sealed class ByteBackedBitArray : IBitArray, IEquatable<ByteBackedBitArray>
{
    private int[] Data { get; }

    public static ByteBackedBitArray Empty { get; } = new(0);

    public ByteBackedBitArray(params bool[] bits)
        : this(bits, bits.Length)
    {
    }

    public ByteBackedBitArray(int size)
        : this(new int[(size + 31) >> 5], size)
    {
    }

    public ByteBackedBitArray(bool[] data, int size)
        : this(CreateData(data, size), size)
    {
    }

    private ByteBackedBitArray(int[] data, int size)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(size);

        Data = data;
        Size = size;
    }

    public bool this[int i]
    {
        get
        {
            EnsureIndex(i);
            return (Data[i >> 5] & (1 << (i & 0x1F))) != 0;
        }
        set
        {
            EnsureIndex(i);
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

    public int Size { get; }

    public void Clear(int i)
    {
        EnsureIndex(i);
        Data[i >> 5] &= ~(1 << (i & 0x1F));
    }

    public void Set(ByteBackedBitArray ba)
    {
        ArgumentNullException.ThrowIfNull(ba);
        LoadValueFrom(ba);
    }

    public void SetAll()
    {
        Array.Fill(Data, -1);
        if (Size > 0)
        {
            Data[^1] &= LastElementMask();
        }
    }

    public void Set(int i)
    {
        EnsureIndex(i);
        Data[i >> 5] |= 1 << (i & 0x1F);
    }

    public int BitCount()
    {
        int bits = 0;
        foreach (int value in Data)
        {
            bits += BitOperations.PopCount((uint)value);
        }

        return bits;
    }

    public void Or(IBitArray bitArray)
    {
        ByteBackedBitArray other = RequireCompatible(bitArray);
        for (int i = 0; i < Data.Length; ++i)
        {
            Data[i] |= other.Data[i];
        }
    }

    public void Xor(IBitArray bitArray)
    {
        ByteBackedBitArray other = RequireCompatible(bitArray);
        for (int i = 0; i < Data.Length; ++i)
        {
            Data[i] ^= other.Data[i];
        }
    }

    public void And(IBitArray bitArray)
    {
        ByteBackedBitArray other = RequireCompatible(bitArray);
        for (int i = 0; i < Data.Length; ++i)
        {
            Data[i] &= other.Data[i];
        }
    }

    public bool Intersects(IBitArray bitArray)
    {
        ByteBackedBitArray other = RequireCompatible(bitArray);
        for (int i = 0; i < Data.Length; ++i)
        {
            if ((Data[i] & other.Data[i]) != 0)
            {
                return true;
            }
        }

        return false;
    }

    public void LoadValueFrom(IBitArray bitArray)
    {
        ByteBackedBitArray other = RequireCompatible(bitArray);
        Array.Copy(other.Data, 0, Data, 0, Data.Length);
    }

    public void ClearAll()
    {
        Array.Fill(Data, 0);
    }

    public IBitArray Clone()
    {
        return new ByteBackedBitArray((int[])Data.Clone(), Size);
    }

    public int[] GetBits()
    {
        int[] bits = new int[BitCount()];
        int position = 0;
        for (int i = 0; i < Size; ++i)
        {
            if (this[i])
            {
                bits[position++] = i;
            }
        }

        return bits;
    }

    public override bool Equals(object? obj)
    {
        return obj is ByteBackedBitArray other && Equals(other);
    }

    public bool Equals(ByteBackedBitArray? other)
    {
        if (other is null || Size != other.Size)
        {
            return false;
        }

        return Data.AsSpan().SequenceEqual(other.Data);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Size);
        foreach (int value in Data)
        {
            hashCode.Add(value);
        }

        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        char[] chars = new char[Size];
        for (int i = 0; i < Size; ++i)
        {
            chars[i] = this[i] ? '1' : '0';
        }

        return new string(chars);
    }

    public int NextTrailingBit(int position)
    {
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "must not be negative");
        }

        if (position >= Size)
        {
            return -1;
        }

        int firstShift = position & 0x1F;
        int pointer = position >> 5;
        uint current = (uint)Data[pointer] >> firstShift;
        if (current != 0)
        {
            return position + BitOperations.TrailingZeroCount(current);
        }

        pointer++;
        while (pointer < Data.Length)
        {
            current = (uint)Data[pointer];
            if (current != 0)
            {
                int result = (pointer << 5) + BitOperations.TrailingZeroCount(current);
                return result >= Size ? -1 : result;
            }

            pointer++;
        }

        return -1;
    }

    private static int[] CreateData(bool[] data, int size)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentOutOfRangeException.ThrowIfNegative(size);

        if (data.Length < size)
        {
            throw new ArgumentException("Boolean data must contain at least size elements.", nameof(data));
        }

        int[] result = new int[(size + 31) >> 5];
        for (int i = 0; i < size; ++i)
        {
            if (data[i])
            {
                result[i >> 5] |= 1 << (i & 0x1F);
            }
        }

        return result;
    }

    private ByteBackedBitArray RequireCompatible(IBitArray bitArray)
    {
        if (bitArray is not ByteBackedBitArray other || other.Size != Size)
        {
            throw new ArgumentException();
        }

        return other;
    }

    private void EnsureIndex(int i)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(i);
        if (i >= Size)
        {
            throw new ArgumentOutOfRangeException(nameof(i), i, "must be within the bit-array bounds");
        }
    }

    private int LastElementMask()
    {
        int trailingBits = Size & 0x1F;
        if (trailingBits == 0)
        {
            return -1;
        }

        return unchecked((int)((1u << trailingBits) - 1));
    }
}
