using System.Collections;
using System.Collections.Immutable;
using NRedberry;
using NRedberry.Indices;
using IndicesInterface = NRedberry.Indices.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public class EmptyIndicesTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        var first = EmptyIndices.EmptyIndicesInstance;
        var second = EmptyIndices.EmptyIndicesInstance;

        Assert.Same(first, second);
    }

    [Fact]
    public void ShouldHaveZeroSizeForAllIndexTypes()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Assert.Equal(0, indices.Size());

        foreach (var indexType in Enum.GetValues<IndexType>())
        {
            Assert.Equal(0, indices.Size(indexType));
        }
    }

    [Fact]
    public void ShouldExposeEmptyIndexCollections()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Assert.Empty(indices.AllIndices);
        Assert.Empty(indices.UpperIndices);
        Assert.Empty(indices.LowerIndices);
        Assert.Empty(indices);
    }

    [Fact]
    public void ShouldReturnSameInstanceForIdentityOperations()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Assert.Same(indices, indices.GetFree());
        Assert.Same(indices, indices.GetInverted());
        Assert.Same(indices, indices.GetOfType(IndexType.Matrix1));
        Assert.Same(indices, indices.ApplyIndexMapping(new IdentityIndexMappingStub()));
    }

    [Fact]
    public void ShouldCompareRegardlessOfOrderForEmptyAndNonEmpty()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;
        var nonEmptyIndices = new NonEmptyIndicesStub();

        Assert.True(indices.EqualsRegardlessOrder(EmptyIndices.EmptyIndicesInstance));
        Assert.False(indices.EqualsRegardlessOrder(nonEmptyIndices));
    }

    [Fact]
    public void ShouldThrowOutOfRangeForIndexers()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Assert.Throws<ArgumentOutOfRangeException>(() => _ = indices[0]);
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = indices[IndexType.GreekLower, 0]);
    }

    [Fact]
    public void ShouldReturnEmptyStringForAllToStringVariants()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Assert.Equal(string.Empty, indices.ToString());
        Assert.Equal(string.Empty, indices.ToString(OutputFormat.Redberry));
        Assert.Equal(string.Empty, indices.ToString(OutputFormat.LaTeX));
        Assert.Equal(string.Empty, indices.ToString(OutputFormat.WolframMathematica));
    }

    [Fact]
    public void ShouldReturnEmptyDiffIds()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Assert.Empty(indices.GetDiffIds());
    }

    [Fact]
    public void ShouldHaveStableHashCodeAndEquality()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;
        var anotherEmpty = new EmptyIndices();

        Assert.True(indices.Equals(indices));
        Assert.True(indices.Equals(anotherEmpty));
        Assert.False(indices.Equals(new object()));
        Assert.Equal(indices.GetHashCode(), anotherEmpty.GetHashCode());
    }
}

file sealed class IdentityIndexMappingStub : IIndexMapping
{
    public int Map(int from)
    {
        return from;
    }
}

file sealed class NonEmptyIndicesStub : IndicesInterface
{
    public ImmutableArray<int> UpperIndices => [1];

    public ImmutableArray<int> LowerIndices => ImmutableArray<int>.Empty;

    public ImmutableArray<int> AllIndices => [1];

    public int Size()
    {
        return 1;
    }

    public int Size(IndexType type)
    {
        return type == IndexType.LatinLower ? 1 : 0;
    }

    public int this[int position]
    {
        get
        {
            if (position != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return 1;
        }
    }

    public int this[IndexType type, int position]
    {
        get
        {
            if (type != IndexType.LatinLower || position != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            return 1;
        }
    }

    public IndicesInterface GetOfType(IndexType type)
    {
        return this;
    }

    public IndicesInterface GetFree()
    {
        return this;
    }

    public IndicesInterface GetInverted()
    {
        return this;
    }

    public bool EqualsRegardlessOrder(IndicesInterface indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        return indices.Size() == 1;
    }

    public void TestConsistentWithException()
    {
    }

    public IndicesInterface ApplyIndexMapping(IIndexMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        return this;
    }

    public string ToString(OutputFormat outputFormat)
    {
        ArgumentNullException.ThrowIfNull(outputFormat);
        return "i";
    }

    public short[] GetDiffIds()
    {
        return [0];
    }

    public IEnumerator<int> GetEnumerator()
    {
        return ((IEnumerable<int>)AllIndices).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
