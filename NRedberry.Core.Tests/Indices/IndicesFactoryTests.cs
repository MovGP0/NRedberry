using NRedberry.Indices;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesFactoryTests
{
    [Fact]
    public void ShouldExposeNonNullEmptySingletons()
    {
        IndicesFactory.EmptyIndices.ShouldNotBeNull();
        IndicesFactory.EmptySimpleIndices.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldReturnEmptyIndicesSingletonWhenCreateFromEmptyArray()
    {
        IndicesContract result = IndicesFactory.Create([]);

        result.ShouldBeSameAs(IndicesFactory.EmptyIndices);
    }

    [Fact]
    public void ShouldReturnSortedIndicesCloneWhenCreateFromNonEmptyArray()
    {
        int[] source = [3, 1];

        IndicesContract result = IndicesFactory.Create(source);
        source[0] = 99;

        result.ShouldBeOfType<SortedIndices>();
        result.AllIndices.ToArray().ShouldBe([1, 3]);
    }

    [Fact]
    public void ShouldReturnEmptyIndicesSingletonWhenCreateFromEmptyIndices()
    {
        IndicesContract result = IndicesFactory.Create(IndicesFactory.EmptyIndices);

        result.ShouldBeSameAs(IndicesFactory.EmptyIndices);
    }

    [Fact]
    public void ShouldPassThroughSortedIndicesWhenCreateFromSortedIndices()
    {
        IndicesContract sorted = IndicesFactory.Create(3, 1);

        IndicesContract result = IndicesFactory.Create(sorted);

        result.ShouldBeSameAs(sorted);
    }

    [Fact]
    public void ShouldCreateSortedIndicesWhenCreateFromNonSortedIndices()
    {
        IndicesContract nonSorted = IndicesFactory.CreateSimple(null, 3, 1);

        IndicesContract result = IndicesFactory.Create(nonSorted);

        result.ShouldBeOfType<SortedIndices>();
        result.ShouldNotBeSameAs(nonSorted);
        result.AllIndices.ToArray().ShouldBe([1, 3]);
    }

    [Fact]
    public void ShouldReturnEmptySimpleIndicesSingletonWhenCreateSimpleFromEmptyData()
    {
        SimpleIndices result = IndicesFactory.CreateSimple(null, []);

        result.ShouldBeSameAs(IndicesFactory.EmptySimpleIndices);
    }

    [Fact]
    public void ShouldReturnNonEmptySimpleIndicesCloneWhenCreateSimpleFromData()
    {
        int[] source = [3, 1];

        SimpleIndices result = IndicesFactory.CreateSimple(null, source);
        source[0] = 99;

        result.ShouldNotBeSameAs(IndicesFactory.EmptySimpleIndices);
        result.AllIndices.ToArray().ShouldBe([3, 1]);
    }

    [Fact]
    public void ShouldReturnEmptySimpleIndicesSingletonWhenCreateSimpleFromEmptyIndices()
    {
        SimpleIndices result = IndicesFactory.CreateSimple(null, IndicesFactory.EmptyIndices);

        result.ShouldBeSameAs(IndicesFactory.EmptySimpleIndices);
    }

    [Fact]
    public void ShouldReturnNonEmptySimpleIndicesWhenCreateSimpleFromNonEmptyIndices()
    {
        IndicesContract indices = IndicesFactory.Create(3, 1);

        SimpleIndices result = IndicesFactory.CreateSimple(null, indices);

        result.ShouldNotBeSameAs(IndicesFactory.EmptySimpleIndices);
        result.AllIndices.ToArray().ShouldBe([1, 3]);
    }
}
