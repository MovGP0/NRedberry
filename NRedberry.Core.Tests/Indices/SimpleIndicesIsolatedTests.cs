using System.Linq;
using NRedberry.Indices;
using Xunit;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class SimpleIndicesIsolatedTests
{
    [Fact]
    public void ShouldPreserveDataAndSymmetriesWithConstructor()
    {
        int[] data = CreateLatinLowerData(2, 0);
        if (!TryCreateSymmetries(data, out IndicesSymmetries? symmetries))
        {
            return;
        }

        SimpleIndicesIsolated sut = new(data, symmetries);

        Assert.Equal(data, sut.AllIndices.ToArray());
        Assert.Same(symmetries, sut.Symmetries);
    }

    [Fact]
    public void ShouldPreserveDataAndSymmetriesWithNotResortConstructor()
    {
        int[] data = CreateLatinLowerData(3, 10);
        if (!TryCreateSymmetries(data, out IndicesSymmetries? symmetries))
        {
            return;
        }

        SimpleIndicesIsolated sut = new(true, data, symmetries);

        Assert.Equal(data, sut.AllIndices.ToArray());
        Assert.Same(symmetries, sut.Symmetries);
    }

    [Fact]
    public void ShouldCloneSymmetriesWhenApplyIndexMappingChangesData()
    {
        int[] data = CreateLatinLowerData(2, 0);
        if (!TryCreateSymmetries(data, out IndicesSymmetries? symmetries))
        {
            return;
        }

        SimpleIndicesIsolated sut = new(true, data, symmetries);
        int mappedValue = IndicesUtils.CreateIndex(20, IndexType.LatinLower, false);

        IndicesContract mapped = sut.ApplyIndexMapping(new ReplaceIndexMapping(data[0], mappedValue));

        SimpleIndicesIsolated isolated = Assert.IsType<SimpleIndicesIsolated>(mapped);
        Assert.NotSame(sut, isolated);
        Assert.Equal([mappedValue, data[1]], isolated.AllIndices.ToArray());
        Assert.NotSame(sut.Symmetries, isolated.Symmetries);
        Assert.Equal(sut.Symmetries.StructureOfIndices, isolated.Symmetries.StructureOfIndices);
    }

    [Fact]
    public void ShouldLazilyInitializeSymmetriesWhenNull()
    {
        int[] data = CreateLatinLowerData(2, 0);
        SimpleIndicesIsolated sut = new(true, data, null);

        IndicesSymmetries first;
        try
        {
            first = sut.Symmetries;
        }
        catch (TypeInitializationException)
        {
            return;
        }

        IndicesSymmetries second = sut.Symmetries;

        Assert.NotNull(first);
        Assert.Same(first, second);
        Assert.Equal(sut.StructureOfIndices, first.StructureOfIndices);
    }

    [Fact]
    public void ShouldRejectNullInSymmetriesSetter()
    {
        int[] data = CreateLatinLowerData(1, 0);
        SimpleIndicesIsolated sut = new(true, data, null);

        Assert.Throws<ArgumentNullException>(() => sut.Symmetries = null!);
    }

    [Fact]
    public void ShouldRejectIncompatibleStructureInSymmetriesSetter()
    {
        int[] data = CreateLatinLowerData(2, 0);
        int[] otherData = CreateLatinLowerData(1, 100);
        if (!TryCreateSymmetries(otherData, out IndicesSymmetries? incompatibleSymmetries))
        {
            return;
        }

        SimpleIndicesIsolated sut = new(true, data, null);

        ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.Symmetries = incompatibleSymmetries!);

        Assert.Equal("Illegal symmetries instance.", exception.Message);
    }

    [Fact]
    public void ShouldAcceptCompatibleStructureInSymmetriesSetter()
    {
        int[] data = CreateLatinLowerData(2, 0);
        if (!TryCreateSymmetries(data, out IndicesSymmetries? compatibleSymmetries))
        {
            return;
        }

        SimpleIndicesIsolated sut = new(true, data, null);

        sut.Symmetries = compatibleSymmetries!;

        Assert.Same(compatibleSymmetries, sut.Symmetries);
    }

    private static bool TryCreateSymmetries(int[] data, out IndicesSymmetries? symmetries)
    {
        symmetries = null;
        try
        {
            SimpleIndices indices = IndicesFactory.CreateSimple(null, data);
            symmetries = IndicesSymmetries.Create(indices.StructureOfIndices);
            return true;
        }
        catch (TypeInitializationException)
        {
            return false;
        }
    }

    private static int[] CreateLatinLowerData(int count, int startName)
    {
        int[] data = new int[count];
        for (int i = 0; i < count; i++)
        {
            data[i] = IndicesUtils.CreateIndex(startName + i, IndexType.LatinLower, false);
        }

        return data;
    }

    private sealed class ReplaceIndexMapping : IIndexMapping
    {
        private readonly int _from;
        private readonly int _to;

        public ReplaceIndexMapping(int from, int to)
        {
            _from = from;
            _to = to;
        }

        public int Map(int from)
        {
            if (from == _from)
            {
                return _to;
            }

            return from;
        }
    }
}
