using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ProductContentTests
{
    [Fact]
    public void ShouldReturnCopiesForScalarsAndStretchIds()
    {
        ProductContent content = CreateContent();

        NRedberry.Tensors.Tensor[] scalars = content.Scalars;
        short[] stretchIds = content.StretchIds;

        scalars[0] = TensorFactory.Parse("z");
        stretchIds[0] = 99;

        content.Scalars[0].ToString(OutputFormat.Redberry).ShouldBe("a");
        content.StretchIds[0].ShouldBe((short)0);
    }

    [Fact]
    public void ShouldReturnClonedDataSlices()
    {
        ProductContent content = CreateContent();

        NRedberry.Tensors.Tensor[] copy = content.GetDataCopy();
        NRedberry.Tensors.Tensor[] range = content.GetRange(1, 3);

        content.GetDataCopy().ShouldNotBeSameAs(copy);
        copy.Length.ShouldBe(3);
        range[0].ToString(OutputFormat.Redberry).ShouldBe("T_{i}");
        range[1].ToString(OutputFormat.Redberry).ShouldBe("U^{i}");
    }

    [Fact]
    public void ShouldEnumerateDataInStoredOrder()
    {
        ProductContent content = CreateContent();

        string[] rendered = content.Select(t => t.ToString(OutputFormat.Redberry)).ToArray();

        rendered.ShouldBe(["R_{j}", "T_{i}", "U^{i}"]);
    }

    [Fact]
    public void ShouldExposeStructureAndNonScalar()
    {
        ProductContent content = CreateContent();

        content.StructureOfContractionsHashed.ShouldBeSameAs(StructureOfContractionsHashed.EmptyInstance);
        content.StructureOfContractions.ShouldBeSameAs(StructureOfContractions.EmptyFullContractionsStructure);
        content.NonScalar!.ToString(OutputFormat.Redberry).ShouldBe("R_{j}");
        content.Size.ShouldBe(3);
        content.GetStretchId(1).ShouldBe((short)1);
    }

    private static ProductContent CreateContent()
    {
        return new ProductContent(
            StructureOfContractionsHashed.EmptyInstance,
            StructureOfContractions.EmptyFullContractionsStructure,
            [TensorFactory.Parse("a"), TensorFactory.Parse("b")],
            TensorFactory.Parse("R_j"),
            [0, 1, 1],
            [TensorFactory.Parse("R_j"), TensorFactory.Parse("T_i"), TensorFactory.Parse("U^i")]);
    }
}
