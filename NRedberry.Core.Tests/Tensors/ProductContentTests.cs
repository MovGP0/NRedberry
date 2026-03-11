using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

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

        Assert.Equal("a", content.Scalars[0].ToString(OutputFormat.Redberry));
        Assert.Equal((short)0, content.StretchIds[0]);
    }

    [Fact]
    public void ShouldReturnClonedDataSlices()
    {
        ProductContent content = CreateContent();

        NRedberry.Tensors.Tensor[] copy = content.GetDataCopy();
        NRedberry.Tensors.Tensor[] range = content.GetRange(1, 3);

        Assert.NotSame(copy, content.GetDataCopy());
        Assert.Equal(3, copy.Length);
        Assert.Equal("T_{i}", range[0].ToString(OutputFormat.Redberry));
        Assert.Equal("U^{i}", range[1].ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldEnumerateDataInStoredOrder()
    {
        ProductContent content = CreateContent();

        string[] rendered = content.Select(t => t.ToString(OutputFormat.Redberry)).ToArray();

        Assert.Equal(["R_{j}", "T_{i}", "U^{i}"], rendered);
    }

    [Fact]
    public void ShouldExposeStructureAndNonScalar()
    {
        ProductContent content = CreateContent();

        Assert.Same(StructureOfContractionsHashed.EmptyInstance, content.StructureOfContractionsHashed);
        Assert.Same(StructureOfContractions.EmptyFullContractionsStructure, content.StructureOfContractions);
        Assert.Equal("R_{j}", content.NonScalar!.ToString(OutputFormat.Redberry));
        Assert.Equal(3, content.Size);
        Assert.Equal((short)1, content.GetStretchId(1));
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
