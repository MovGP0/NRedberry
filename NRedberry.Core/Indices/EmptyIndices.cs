using System.Collections;
using System.Collections.Immutable;

namespace NRedberry.Indices;

public class EmptyIndices : Indices
{
    public static readonly EmptyIndices EmptyIndicesInstance = new();

    public int this[int position] => throw new ArgumentOutOfRangeException(nameof(position), position, "Size is zero.");
    public int this[IndexType type, int position] => throw new ArgumentOutOfRangeException(nameof(position), position, "Size is zero.");

    public Indices GetInverted()
    {
        return this;
    }

    public Indices GetFree()
    {
        return this;
    }

    public Indices GetOfType(IndexType type)
    {
        return this;
    }

    public bool EqualsRegardlessOrder(Indices indices)
    {
        return indices.Size() == 0;
    }

    public ImmutableArray<int> GetUpper() => [];

    public ImmutableArray<int> GetLower() => [];

    public ImmutableArray<int> GetAllIndices() => [];

    public int Size(IndexType type)
    {
        return 0;
    }

    public int Size()
    {
        return 0;
    }

    public Indices ApplyIndexMapping(IIndexMapping mapping)
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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<int> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        return obj is EmptyIndices;
    }

    public override int GetHashCode()
    {
        return 8758765;
    }

    public short[] GetDiffIds()
    {
        return [];
    }
}
