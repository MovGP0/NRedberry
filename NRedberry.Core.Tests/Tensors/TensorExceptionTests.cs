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

        Assert.Contains("failure", exception.Message);
        Assert.Contains("nmseed: 1234", exception.Message);
        Assert.Empty(exception.Tensors);
    }

    [Fact]
    public void ShouldIncludeTensorTextAndStoreTensors()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a+b");
        TensorException exception = new("failure", 77, tensor);

        Assert.Contains("\"failure\" in tensors", exception.Message);
        Assert.Contains("a+b", exception.Message);
        Assert.Contains("nmseed: 77", exception.Message);
        Assert.Single(exception.Tensors);
        Assert.Same(tensor, exception.Tensors[0]);
    }

    [Fact]
    public void ShouldPreserveInnerException()
    {
        InvalidOperationException inner = new("boom");
        TensorException exception = new("failure", inner);

        Assert.Same(inner, exception.InnerException);
        Assert.Contains("failure", exception.Message);
    }
}
