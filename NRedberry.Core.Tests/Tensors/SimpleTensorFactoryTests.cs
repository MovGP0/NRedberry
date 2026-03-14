using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorFactoryTests
{
    [Fact]
    public void ShouldReturnWrappedTensorWhenNoArgumentsAreProvided()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        NRedberry.Tensors.TensorFactory factory = tensor.GetFactory().ShouldBeAssignableTo<NRedberry.Tensors.TensorFactory>();

        factory.Create().ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldRejectUnexpectedArguments()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        NRedberry.Tensors.TensorFactory factory = tensor.GetFactory().ShouldBeAssignableTo<NRedberry.Tensors.TensorFactory>();

        Should.Throw<NotSupportedException>(() => factory.Create(TensorFactory.Parse("a")));
    }

    [Fact]
    public void ShouldRejectUnexpectedArgumentsForConcreteOverload()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        SimpleTensorFactory factory = tensor.GetFactory().ShouldBeOfType<SimpleTensorFactory>();

        Should.Throw<NotSupportedException>(() => factory.Create(TensorFactory.Parse("a")));
    }
}
