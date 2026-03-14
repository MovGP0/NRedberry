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

        sut.AllIndices.ToArray().ShouldBe(data);
        sut.Symmetries.ShouldBeSameAs(symmetries);
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

        sut.AllIndices.ToArray().ShouldBe(data);
        sut.Symmetries.ShouldBeSameAs(symmetries);
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

        SimpleIndicesIsolated isolated = mapped.ShouldBeOfType<SimpleIndicesIsolated>();
        isolated.ShouldNotBeSameAs(sut);
        isolated.AllIndices.ToArray().ShouldBe([mappedValue, data[1]]);
        isolated.Symmetries.ShouldNotBeSameAs(sut.Symmetries);
        isolated.Symmetries.StructureOfIndices.ShouldBe(sut.Symmetries.StructureOfIndices);
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

        first.ShouldNotBeNull();
        second.ShouldBeSameAs(first);
        first.StructureOfIndices.ShouldBe(sut.StructureOfIndices);
    }

    [Fact]
    public void ShouldRejectNullInSymmetriesSetter()
    {
        int[] data = CreateLatinLowerData(1, 0);
        SimpleIndicesIsolated sut = new(true, data, null);

        Should.Throw<ArgumentNullException>(() => sut.Symmetries = null!);
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

        ArgumentException exception = Should.Throw<ArgumentException>(() => sut.Symmetries = incompatibleSymmetries!);

        exception.Message.ShouldBe("Illegal symmetries instance.");
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

        sut.Symmetries.ShouldBeSameAs(compatibleSymmetries);
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

    private sealed class ReplaceIndexMapping(int from, int to) : IIndexMapping
    {
        public int Map(int from1)
        {
            if (from1 == from)
            {
                return to;
            }

            return from1;
        }
    }
}
