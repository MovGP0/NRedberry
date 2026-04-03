using NRedberry.Tensors;
using NRedberry.Tensors.Playground;
using TensorApi = NRedberry.Tensors.Tensors;

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
    public void ShouldMatchStructureOfContractionsForTensorConstructor()
    {
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("a");
        StructureOfContractions expected = new([scalar], 0, scalar.Indices);
        GraphStructure actual = new([scalar], 0, scalar.Indices);

        actual.FreeContractions.ShouldBe(expected.freeContractions);
        actual.Contractions.Length.ShouldBe(expected.contractions.Length);
        for (int i = 0; i < actual.Contractions.Length; ++i)
        {
            actual.Contractions[i].ShouldBe(expected.contractions[i]);
        }

        actual.Components.ShouldBe(expected.components);
        actual.ComponentCount.ShouldBe(expected.componentCount);
    }
}
