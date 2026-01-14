using System.Collections.Immutable;

namespace NRedberry.Indices;

/// <summary>
/// Common tensor indices functionality. Indices are immutable and follow the bit-mask
/// representation described in <see cref="IndicesUtils"/>.
/// </summary>
/// <seealso cref="IndicesBuilder"/>
/// <seealso cref="IIndexMapping"/>
public interface Indices : IEquatable<object>, IEnumerable<int>
{
    /// <summary>
    /// Returns upper (contravariant) indices.
    /// </summary>
    ImmutableArray<int> UpperIndices { get; }

    /// <summary>
    /// Returns lower (covariant) indices.
    /// </summary>
    ImmutableArray<int> LowerIndices { get; }

    /// <summary>
    /// Returns all indices.
    /// </summary>
    ImmutableArray<int> AllIndices { get; }

    /// <summary>
    /// Returns the number of indices.
    /// </summary>
    int Size();

    /// <summary>
    /// Returns the number of indices of the specified type.
    /// </summary>
    int Size(IndexType type);

    /// <summary>
    /// Returns the index at the specified position.
    /// </summary>
    int this[int position] { get; }

    /// <summary>
    /// Returns the index of the specified type at the specified position.
    /// </summary>
    int this[IndexType type, int position] { get; }

    /// <summary>
    /// Returns indices of the specified type contained in this instance.
    /// </summary>
    Indices GetOfType(IndexType type);

    /// <summary>
    /// Returns only free (non-contracted) indices.
    /// </summary>
    Indices GetFree();

    /// <summary>
    /// Returns indices with inverted states (upper &lt;-&gt; lower).
    /// </summary>
    Indices GetInverted();

    /// <summary>
    /// Returns true if this instance contains the same indices regardless of order.
    /// </summary>
    bool EqualsRegardlessOrder(Indices indices);

    /// <summary>
    /// Throws an exception if indices are inconsistent in public API calls.
    /// </summary>
    void TestConsistentWithException();

    /// <summary>
    /// Applies the specified index mapping and returns the resulting indices.
    /// </summary>
    Indices ApplyIndexMapping(IIndexMapping mapping);

    /// <summary>
    /// String representation in the specified output format.
    /// </summary>
    string ToString(OutputFormat outputFormat);

    /// <summary>
    /// Returns positions in orbits for symmetry-aware hashing.
    /// </summary>
    short[] GetDiffIds();
}
