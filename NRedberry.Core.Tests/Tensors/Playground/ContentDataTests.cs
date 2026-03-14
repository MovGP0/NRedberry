using NRedberry.Tensors.Playground;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Playground;

public sealed class ContentDataTests
{
    [Fact]
    public void ShouldExposeEmptySingleton()
    {
        ContentData empty = ContentData.EmptyInstance;

        empty.StructureOfContractionsHashed.ShouldBeSameAs(GraphStructureHashed.EmptyInstance);
        empty.StructureOfContractions.ShouldBeSameAs(GraphStructure.EmptyFullContractionsStructure);
        empty.Data.ShouldBeEmpty();
        empty.StretchIndices.ShouldBeEmpty();
        empty.HashCodes.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldStoreConstructorArguments()
    {
        GraphStructureHashed hashed = new([1], [2L], [[3L]]);
        GraphStructure graphStructure = new([4L], [[5L]], [6], 7);
        NRedberry.Tensors.Tensor[] data = [TensorApi.Parse("a")];
        short[] stretchIndices = [8];
        int[] hashCodes = [9];

        ContentData contentData = new(hashed, graphStructure, data, stretchIndices, hashCodes);

        contentData.StructureOfContractionsHashed.ShouldBeSameAs(hashed);
        contentData.StructureOfContractions.ShouldBeSameAs(graphStructure);
        contentData.Data.ShouldBeSameAs(data);
        contentData.StretchIndices.ShouldBeSameAs(stretchIndices);
        contentData.HashCodes.ShouldBeSameAs(hashCodes);
    }
}
