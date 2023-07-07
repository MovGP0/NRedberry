using System;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Indices;

public sealed class EmptyIndices : IIndices
{
    public static readonly EmptyIndices EmptyIndicesInstance = new();

    public long this[long position] => throw new ArgumentOutOfRangeException(nameof(position), position, "Size is zero.");
    public long this[IndexType type, long position] => throw new ArgumentOutOfRangeException(nameof(position), position, "Size is zero.");

    public IIndices GetInverted()
    {
        return this;
    }

    public IIndices GetFree()
    {
        return this;
    }

    public IIndices GetOfType(IndexType type)
    {
        return this;
    }

    public bool EqualsRegardlessOrder(IIndices indices)
    {
        return indices.Size() == 0;
    }

    public IntArray GetUpper()
    {
        return IntArray.EmptyArray;
    }

    public IntArray GetLower()
    {
        return IntArray.EmptyArray;
    }

    public IntArray GetAllIndices()
    {
        return IntArray.EmptyArray;
    }

    public int Size(IndexType type)
    {
        return 0;
    }

    public int Size()
    {
        return 0;
    }

    public IIndices ApplyIndexMapping(IIndexMapping mapping)
    {
        return this;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return string.Empty;
    }

    public void TestConsistentWithException()
    {
    }

    public override string ToString()
    {
        return string.Empty;
    }

    public override bool Equals(object obj)
    {
        return obj is EmptyIndices;
    }

    public override int GetHashCode()
    {
        return 8758765;
    }

    public short[] GetDiffIds()
    {
        return new short[0];
    }
}