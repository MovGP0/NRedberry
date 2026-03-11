using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class StructureOfContractionsTests
{
    [Fact]
    public void ShouldComputeConnectedComponentsFromRawContractions()
    {
        long contraction01 = Pack(1, 0, 0);
        long contraction10 = Pack(0, 0, 0);
        StructureOfContractions structure = new([[contraction01], [contraction10], []]);

        Assert.Equal(2, structure.componentCount);
        Assert.Equal([0, 0, 1], structure.components);
        Assert.Contains("0_0 -> 1_0", structure.ToString());
    }

    [Fact]
    public void ShouldDescribeContractionsForSpecificTensor()
    {
        long contraction01 = Pack(1, 0, 0);
        long contraction10 = Pack(0, 0, 0);
        StructureOfContractions structure = new([[contraction01], [contraction10], []]);

        StructureOfContractions.Contraction[] contractions = structure.GetContractedWith(0);

        Assert.Single(contractions);
        Assert.Equal(1, contractions[0].Tensor);
        Assert.Equal([0], contractions[0].IndicesFrom);
        Assert.Equal([0], contractions[0].IndicesTo);
    }

    [Fact]
    public void ShouldBuildContractionsFromParsedProductContent()
    {
        Product product = Assert.IsType<Product>(TensorFactory.Parse("f_ab*t^bca*g_cd"));
        StructureOfContractions structure = product.Content.StructureOfContractions;

        Assert.NotNull(structure);
        Assert.NotNull(product.Content.StructureOfContractionsHashed);
        Assert.True(structure.componentCount >= 0);
        Assert.NotNull(structure.contractions);
        Assert.NotNull(structure.components);
    }

    [Fact]
    public void ShouldDecodePackedContractionData()
    {
        long contraction = Pack(12, 5, 3);

        Assert.Equal(12, StructureOfContractions.GetToTensorIndex(contraction));
        Assert.Equal(5, StructureOfContractions.GetToIndexId(contraction));
        Assert.Equal(3, StructureOfContractions.GetFromIndexId(contraction));
        Assert.Equal(12, StructureOfContractions.ToPosition(contraction));
        Assert.Equal((short)5, StructureOfContractions.ToIDiffId(contraction));
        Assert.Equal(3, StructureOfContractions.FromIPosition(contraction));
    }

    private static long Pack(int toTensorIndex, int toIndexId, int fromIndexId)
    {
        return ((long)toTensorIndex << 32)
            | ((long)toIndexId << 16)
            | (uint)fromIndexId;
    }
}
