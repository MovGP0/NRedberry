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

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void ShouldHaveZeroSizeForAllIndexTypes()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        indices.Size().ShouldBe(0);

        foreach (var indexType in Enum.GetValues<IndexType>())
        {
            indices.Size(indexType).ShouldBe(0);
        }
    }

    [Fact]
    public void ShouldExposeEmptyIndexCollections()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        indices.AllIndices.ShouldBeEmpty();
        indices.UpperIndices.ShouldBeEmpty();
        indices.LowerIndices.ShouldBeEmpty();
        indices.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldReturnSameInstanceForIdentityOperations()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        indices.GetFree().ShouldBeSameAs(indices);
        indices.GetInverted().ShouldBeSameAs(indices);
        indices.GetOfType(IndexType.Matrix1).ShouldBeSameAs(indices);
        indices.ApplyIndexMapping(new IdentityIndexMappingStub()).ShouldBeSameAs(indices);
    }

    [Fact]
    public void ShouldCompareRegardlessOfOrderForEmptyAndNonEmpty()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;
        var nonEmptyIndices = new NonEmptyIndicesStub();

        indices.EqualsRegardlessOrder(EmptyIndices.EmptyIndicesInstance).ShouldBeTrue();
        indices.EqualsRegardlessOrder(nonEmptyIndices).ShouldBeFalse();
    }

    [Fact]
    public void ShouldThrowOutOfRangeForIndexers()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        Should.Throw<ArgumentOutOfRangeException>(() => _ = indices[0]);
        Should.Throw<ArgumentOutOfRangeException>(() => _ = indices[IndexType.GreekLower, 0]);
    }

    [Fact]
    public void ShouldReturnEmptyStringForAllToStringVariants()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        indices.ToString().ShouldBe(string.Empty);
        indices.ToString(OutputFormat.Redberry).ShouldBe(string.Empty);
        indices.ToString(OutputFormat.LaTeX).ShouldBe(string.Empty);
        indices.ToString(OutputFormat.WolframMathematica).ShouldBe(string.Empty);
    }

    [Fact]
    public void ShouldReturnEmptyDiffIds()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;

        indices.GetDiffIds().ShouldBeEmpty();
    }

    [Fact]
    public void ShouldHaveStableHashCodeAndEquality()
    {
        var indices = EmptyIndices.EmptyIndicesInstance;
        var anotherEmpty = new EmptyIndices();

        indices.Equals(indices).ShouldBeTrue();
        indices.Equals(anotherEmpty).ShouldBeTrue();
        indices.Equals(new object()).ShouldBeFalse();
        anotherEmpty.GetHashCode().ShouldBe(indices.GetHashCode());
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
