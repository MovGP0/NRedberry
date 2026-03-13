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

        Assert.Same(GraphStructureHashed.EmptyInstance, empty.StructureOfContractionsHashed);
        Assert.Same(GraphStructure.EmptyFullContractionsStructure, empty.StructureOfContractions);
        Assert.Empty(empty.Data);
        Assert.Empty(empty.StretchIndices);
        Assert.Empty(empty.HashCodes);
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

        Assert.Same(hashed, contentData.StructureOfContractionsHashed);
        Assert.Same(graphStructure, contentData.StructureOfContractions);
        Assert.Same(data, contentData.Data);
        Assert.Same(stretchIndices, contentData.StretchIndices);
        Assert.Same(hashCodes, contentData.HashCodes);
    }
}
