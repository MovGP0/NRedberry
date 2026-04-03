using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class StructureOfContractionsTests
{
    [Fact]
    public void ShouldComputeConnectedComponentsFromRawContractions()
    {
        long contraction01 = Pack(1, 0, 0);
        long contraction10 = Pack(0, 0, 0);
        StructureOfContractions structure = new([[contraction01], [contraction10], []]);

        structure.componentCount.ShouldBe(2);
        structure.components.ShouldBe([0, 0, 1]);
        structure.ToString().ShouldContain("0_0 -> 1_0");
    }

    [Fact]
    public void ShouldDescribeContractionsForSpecificTensor()
    {
        long contraction01 = Pack(1, 0, 0);
        long contraction10 = Pack(0, 0, 0);
        StructureOfContractions structure = new([[contraction01], [contraction10], []]);

        StructureOfContractions.Contraction[] contractions = structure.GetContractedWith(0);

        contractions.ShouldHaveSingleItem();
        contractions[0].Tensor.ShouldBe(1);
        contractions[0].IndicesFrom.ShouldBe([0]);
        contractions[0].IndicesTo.ShouldBe([0]);
    }

    [Fact]
    public void ShouldBuildContractionsFromParsedProductContent()
    {
        Product product = TensorFactory.Parse("f_ab*t^bca*g_cd").ShouldBeOfType<Product>();
        StructureOfContractions structure = product.Content.StructureOfContractions;

        structure.ShouldNotBeNull();
        product.Content.StructureOfContractionsHashed.ShouldNotBeNull();
        (structure.componentCount >= 0).ShouldBeTrue();
        structure.contractions.ShouldNotBeNull();
        structure.components.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldDecodePackedContractionData()
    {
        long contraction = Pack(12, 5, 3);

        StructureOfContractions.GetToTensorIndex(contraction).ShouldBe(12);
        StructureOfContractions.GetToIndexId(contraction).ShouldBe(5);
        StructureOfContractions.GetFromIndexId(contraction).ShouldBe(3);
        StructureOfContractions.ToPosition(contraction).ShouldBe(12);
        StructureOfContractions.ToIDiffId(contraction).ShouldBe((short)5);
        StructureOfContractions.FromIPosition(contraction).ShouldBe(3);
    }

    private static long Pack(int toTensorIndex, int toIndexId, int fromIndexId)
    {
        return ((long)toTensorIndex << 32)
            | ((long)toIndexId << 16)
            | (uint)fromIndexId;
    }
}
