using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorFactoryTests
{
    [Fact]
    public void ShouldReturnWrappedTensorWhenNoArgumentsAreProvided()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        NRedberry.Tensors.TensorFactory factory = Assert.IsAssignableFrom<NRedberry.Tensors.TensorFactory>(tensor.GetFactory());

        Assert.Same(tensor, factory.Create());
    }

    [Fact]
    public void ShouldRejectUnexpectedArguments()
    {
        SimpleTensor tensor = Assert.IsType<SimpleTensor>(TensorFactory.Parse("T_a"));
        NRedberry.Tensors.TensorFactory factory = Assert.IsAssignableFrom<NRedberry.Tensors.TensorFactory>(tensor.GetFactory());

        Assert.Throws<NotSupportedException>(() => factory.Create(TensorFactory.Parse("a")));
    }
}
