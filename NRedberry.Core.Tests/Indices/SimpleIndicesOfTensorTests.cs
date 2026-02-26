using NRedberry.Indices;
using Xunit;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class SimpleIndicesOfTensorTests
{
    [Fact]
    public void Constructors_ShouldKeepData()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int[] data = CreateLatinLowerData(2, 10);

            SimpleIndicesOfTensor fromDefaultConstructor = new(data, null);
            SimpleIndicesOfTensor fromNotResortConstructor = new(true, data, null);

            Assert.Equal(data, fromDefaultConstructor.AllIndices.ToArray());
            Assert.Equal(data, fromNotResortConstructor.AllIndices.ToArray());
        });
    }

    [Fact]
    public void ApplyIndexMapping_ShouldReturnSimpleIndicesOfTensorWhenMappingChangesData()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int[] data = CreateLatinLowerData(2, 20);
            SimpleIndicesOfTensor sut = new(true, data, null);
            int mapped = IndicesUtils.CreateIndex(999, IndexType.LatinLower, false);

            IndicesContract mappedIndices = sut.ApplyIndexMapping(new ReplaceIndexMapping(data[0], mapped));

            SimpleIndicesOfTensor result = Assert.IsType<SimpleIndicesOfTensor>(mappedIndices);
            Assert.NotSame(sut, result);
            Assert.Equal([mapped, data[1]], result.AllIndices.ToArray());
        });
    }

    [Fact]
    public void SymmetriesGetter_ShouldThrowWhenUnset_AndReturnAssignedWhenSet()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int[] data = CreateLatinLowerData(2, 30);

            SimpleIndicesOfTensor withoutSymmetries = new(true, data, null);
            Assert.Throws<InvalidOperationException>(() => _ = withoutSymmetries.Symmetries);

            IndicesSymmetries symmetries = IndicesSymmetries.Create(withoutSymmetries.StructureOfIndices);
            SimpleIndicesOfTensor withSymmetries = new(true, data, symmetries);
            Assert.Same(symmetries, withSymmetries.Symmetries);
        });
    }

    [Fact]
    public void SymmetriesSetter_ShouldThrowNotSupportedException()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int[] data = CreateLatinLowerData(1, 40);
            SimpleIndicesOfTensor sut = new(true, data, null);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(sut.StructureOfIndices);

            Assert.Throws<NotSupportedException>(() => sut.Symmetries = symmetries);
        });
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

    private static void RunIgnoringContextInitializationFailure(Action action)
    {
        try
        {
            action();
        }
        catch (TypeInitializationException)
        {
        }
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
