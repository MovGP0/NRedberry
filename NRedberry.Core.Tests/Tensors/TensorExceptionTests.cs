using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorExceptionTests
{
    [Fact]
    public void ShouldIncludeSeedWhenNoTensorsAreProvided()
    {
        TensorException exception = new("failure", 1234);

        exception.Message.ShouldContain("failure");
        exception.Message.ShouldContain("nmseed: 1234");
        exception.Tensors.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldIncludeTensorTextAndStoreTensors()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a+b");
        TensorException exception = new("failure", 77, tensor);

        exception.Message.ShouldContain("\"failure\" in tensors");
        exception.Message.ShouldContain("a+b");
        exception.Message.ShouldContain("nmseed: 77");
        exception.Tensors.ShouldHaveSingleItem();
        exception.Tensors[0].ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldPreserveInnerException()
    {
        InvalidOperationException inner = new("boom");
        TensorException exception = new("failure", inner);

        exception.InnerException.ShouldBeSameAs(inner);
        exception.Message.ShouldContain("failure");
    }
}
