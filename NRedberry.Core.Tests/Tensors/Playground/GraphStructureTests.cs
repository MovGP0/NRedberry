using NRedberry.Tensors.Playground;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Playground;

public sealed class GraphStructureTests
{
    [Fact]
    public void ShouldExposeEmptySingleton()
    {
        GraphStructure empty = GraphStructure.EmptyFullContractionsStructure;

        empty.FreeContractions.ShouldBeEmpty();
        empty.Contractions.ShouldBeEmpty();
        empty.Components.ShouldBeEmpty();
        empty.ComponentCount.ShouldBe(0);
    }

    [Fact]
    public void ShouldStoreRawGraphData()
    {
        long[] freeContractions = [1L];
        long[][] contractions = [[2L, 3L]];
        int[] components = [4, 5];

        GraphStructure graphStructure = new(freeContractions, contractions, components, 6);

        graphStructure.FreeContractions.ShouldBeSameAs(freeContractions);
        graphStructure.Contractions.ShouldBeSameAs(contractions);
        graphStructure.Components.ShouldBeSameAs(components);
        graphStructure.ComponentCount.ShouldBe(6);
    }

    [Fact]
    public void ShouldThrowForUnportedTensorConstructor()
    {
        Should.Throw<NotImplementedException>(() => new GraphStructure([TensorApi.Parse("a")], 1, TensorApi.Parse("a").Indices));
    }
}
