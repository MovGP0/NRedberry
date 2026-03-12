using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcCosTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        ArcCos function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        Assert.Same(argument, function[0]);
        Assert.Equal(1, function.Size);
        Assert.Equal("ArcCos[a]", function.ToString(OutputFormat.Redberry));
        Assert.IsType<Product>(derivative);
        Assert.Contains("a**2", derivative.ToString(OutputFormat.Redberry));
        Assert.Equal("ScalarFunctionBuilder", function.GetBuilder().GetType().Name);
        Assert.Same(ArcCosFactory.Factory, function.GetFactory());
    }
}
