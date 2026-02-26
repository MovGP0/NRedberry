using System.Linq;
using NRedberry.Indices;
using Xunit;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesFactoryTests
{
    [Fact]
    public void ShouldExposeNonNullEmptySingletons()
    {
        Assert.NotNull(IndicesFactory.EmptyIndices);
        Assert.NotNull(IndicesFactory.EmptySimpleIndices);
    }

    [Fact]
    public void ShouldReturnEmptyIndicesSingletonWhenCreateFromEmptyArray()
    {
        IndicesContract result = IndicesFactory.Create([]);

        Assert.Same(IndicesFactory.EmptyIndices, result);
    }

    [Fact]
    public void ShouldReturnSortedIndicesCloneWhenCreateFromNonEmptyArray()
    {
        int[] source = [3, 1];

        IndicesContract result = IndicesFactory.Create(source);
        source[0] = 99;

        Assert.IsType<SortedIndices>(result);
        Assert.Equal([1, 3], result.AllIndices.ToArray());
    }

    [Fact]
    public void ShouldReturnEmptyIndicesSingletonWhenCreateFromEmptyIndices()
    {
        IndicesContract result = IndicesFactory.Create(IndicesFactory.EmptyIndices);

        Assert.Same(IndicesFactory.EmptyIndices, result);
    }

    [Fact]
    public void ShouldPassThroughSortedIndicesWhenCreateFromSortedIndices()
    {
        IndicesContract sorted = IndicesFactory.Create(3, 1);

        IndicesContract result = IndicesFactory.Create(sorted);

        Assert.Same(sorted, result);
    }

    [Fact]
    public void ShouldCreateSortedIndicesWhenCreateFromNonSortedIndices()
    {
        IndicesContract nonSorted = IndicesFactory.CreateSimple(null, 3, 1);

        IndicesContract result = IndicesFactory.Create(nonSorted);

        Assert.IsType<SortedIndices>(result);
        Assert.NotSame(nonSorted, result);
        Assert.Equal([1, 3], result.AllIndices.ToArray());
    }

    [Fact]
    public void ShouldReturnEmptySimpleIndicesSingletonWhenCreateSimpleFromEmptyData()
    {
        SimpleIndices result = IndicesFactory.CreateSimple(null, []);

        Assert.Same(IndicesFactory.EmptySimpleIndices, result);
    }

    [Fact]
    public void ShouldReturnNonEmptySimpleIndicesCloneWhenCreateSimpleFromData()
    {
        int[] source = [3, 1];

        SimpleIndices result = IndicesFactory.CreateSimple(null, source);
        source[0] = 99;

        Assert.NotSame(IndicesFactory.EmptySimpleIndices, result);
        Assert.Equal([3, 1], result.AllIndices.ToArray());
    }

    [Fact]
    public void ShouldReturnEmptySimpleIndicesSingletonWhenCreateSimpleFromEmptyIndices()
    {
        SimpleIndices result = IndicesFactory.CreateSimple(null, IndicesFactory.EmptyIndices);

        Assert.Same(IndicesFactory.EmptySimpleIndices, result);
    }

    [Fact]
    public void ShouldReturnNonEmptySimpleIndicesWhenCreateSimpleFromNonEmptyIndices()
    {
        IndicesContract indices = IndicesFactory.Create(3, 1);

        SimpleIndices result = IndicesFactory.CreateSimple(null, indices);

        Assert.NotSame(IndicesFactory.EmptySimpleIndices, result);
        Assert.Equal([1, 3], result.AllIndices.ToArray());
    }
}
