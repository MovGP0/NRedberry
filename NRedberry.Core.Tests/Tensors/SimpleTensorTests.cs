using NRedberry.Indices;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorTests
{
    [Fact]
    public void ShouldCacheIndexlessTensorInstancesByName()
    {
        SimpleTensor first = NRedberry.Tensors.Tensor.SimpleTensor("A", IndicesFactory.EmptySimpleIndices);
        SimpleTensor second = NRedberry.Tensors.Tensor.SimpleTensor("A", IndicesFactory.EmptySimpleIndices);

        Assert.Same(first, second);
    }

    [Fact]
    public void ShouldExposeIndicesNameAndZeroSize()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));

        Assert.Same(tensor.SimpleIndices, tensor.Indices);
        Assert.True(tensor.Name > 0);
        Assert.Equal(0, tensor.Size);
        Assert.Throws<IndexOutOfRangeException>(() => _ = tensor[0]);
    }

    [Fact]
    public void ShouldCompareByNameAndIndices()
    {
        SimpleTensor first = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        SimpleTensor second = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        SimpleTensor third = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_b"));

        Assert.Equal(
            first.ToString(OutputFormat.Redberry),
            second.ToString(OutputFormat.Redberry));
        Assert.NotEqual(
            first.SimpleIndices.ToString(OutputFormat.Redberry),
            third.SimpleIndices.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeBuilderFactoryAndStringName()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));

        Assert.IsType<SimpleTensorBuilder>(tensor.GetBuilder());
        Assert.IsType<SimpleTensorFactory>(tensor.GetFactory());
        Assert.Equal("T", tensor.GetStringName());
    }
}
