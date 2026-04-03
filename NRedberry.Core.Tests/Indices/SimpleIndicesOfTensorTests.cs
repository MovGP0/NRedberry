using NRedberry.Indices;
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

            fromDefaultConstructor.AllIndices.ToArray().ShouldBe(data);
            fromNotResortConstructor.AllIndices.ToArray().ShouldBe(data);
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

            SimpleIndicesOfTensor result = mappedIndices.ShouldBeOfType<SimpleIndicesOfTensor>();
            result.ShouldNotBeSameAs(sut);
            result.AllIndices.ToArray().ShouldBe([mapped, data[1]]);
        });
    }

    [Fact]
    public void SymmetriesGetter_ShouldThrowWhenUnset_AndReturnAssignedWhenSet()
    {
        RunIgnoringContextInitializationFailure(() =>
        {
            int[] data = CreateLatinLowerData(2, 30);

            SimpleIndicesOfTensor withoutSymmetries = new(true, data, null);
            Should.Throw<InvalidOperationException>(() => _ = withoutSymmetries.Symmetries);

            IndicesSymmetries symmetries = IndicesSymmetries.Create(withoutSymmetries.StructureOfIndices);
            SimpleIndicesOfTensor withSymmetries = new(true, data, symmetries);
            withSymmetries.Symmetries.ShouldBeSameAs(symmetries);
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

            Should.Throw<NotSupportedException>(() => sut.Symmetries = symmetries);
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
