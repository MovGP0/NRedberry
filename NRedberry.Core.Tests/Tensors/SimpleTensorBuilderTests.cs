using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorBuilderTests
{
    [Fact]
    public void ShouldReturnWrappedTensorFromBuild()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        SimpleTensorBuilder builder = new(tensor);

        Assert.Same(tensor, builder.Build());
    }

    [Fact]
    public void ShouldRejectPutOperations()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        SimpleTensorBuilder builder = new(tensor);

        Assert.Throws<NotSupportedException>(() => builder.Put(TensorFactory.Parse("a")));
    }

    [Fact]
    public void ShouldReturnSameInstanceFromClone()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        SimpleTensorBuilder builder = new(tensor);

        Assert.Same(builder, builder.Clone());
    }
}
