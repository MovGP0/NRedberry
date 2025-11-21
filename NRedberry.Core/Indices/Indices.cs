using System.Collections.Immutable;

namespace NRedberry.Indices;

public interface Indices : IEquatable<object>, IEnumerable<int>
{
    ImmutableArray<int> GetUpper();
    ImmutableArray<int> GetLower();
    ImmutableArray<int> GetAllIndices();

    ImmutableArray<int> AllIndices => GetAllIndices();

    int Size();
    int Size(IndexType type);

    int this[int position] { get; }
    int this[IndexType type, int position] { get; }

    Indices GetFree();
    Indices GetInverted();
    Indices GetOfType(IndexType type);
    bool EqualsRegardlessOrder(Indices indices);
    void TestConsistentWithException();
    Indices ApplyIndexMapping(IIndexMapping mapping);
    string ToString(OutputFormat outputFormat);
    short[] GetDiffIds();
}
