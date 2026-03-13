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

        Assert.Empty(empty.FreeContractions);
        Assert.Empty(empty.Contractions);
        Assert.Empty(empty.Components);
        Assert.Equal(0, empty.ComponentCount);
    }

    [Fact]
    public void ShouldStoreRawGraphData()
    {
        long[] freeContractions = [1L];
        long[][] contractions = [[2L, 3L]];
        int[] components = [4, 5];

        GraphStructure graphStructure = new(freeContractions, contractions, components, 6);

        Assert.Same(freeContractions, graphStructure.FreeContractions);
        Assert.Same(contractions, graphStructure.Contractions);
        Assert.Same(components, graphStructure.Components);
        Assert.Equal(6, graphStructure.ComponentCount);
    }

    [Fact]
    public void ShouldThrowForUnportedTensorConstructor()
    {
        Assert.Throws<NotImplementedException>(() => new GraphStructure([TensorApi.Parse("a")], 1, TensorApi.Parse("a").Indices));
    }
}
