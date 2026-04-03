using NRedberry.Indices;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorTests
{
    [Fact]
    public void ShouldCacheIndexlessTensorInstancesByName()
    {
        SimpleTensor first = NRedberry.Tensors.Tensor.SimpleTensor("A", IndicesFactory.EmptySimpleIndices);
        SimpleTensor second = NRedberry.Tensors.Tensor.SimpleTensor("A", IndicesFactory.EmptySimpleIndices);

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void ShouldExposeIndicesNameAndZeroSize()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();

        tensor.Indices.ShouldBeSameAs(tensor.SimpleIndices);
        (tensor.Name > 0).ShouldBeTrue();
        tensor.Size.ShouldBe(0);
        Should.Throw<IndexOutOfRangeException>(() => _ = tensor[0]);
    }

    [Fact]
    public void ShouldCompareByNameAndIndices()
    {
        SimpleTensor first = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        SimpleTensor second = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        SimpleTensor third = TensorFactory.Parse("T_b").ShouldBeOfType<SimpleTensor>();

        second.ToString(OutputFormat.Redberry).ShouldBe(first.ToString(OutputFormat.Redberry));
        third.SimpleIndices.ToString(OutputFormat.Redberry).ShouldNotBe(first.SimpleIndices.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeBuilderFactoryAndStringName()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();

        tensor.GetBuilder().ShouldBeOfType<SimpleTensorBuilder>();
        tensor.GetFactory().ShouldBeOfType<SimpleTensorFactory>();
        tensor.GetStringName().ShouldBe("T");
    }
}
