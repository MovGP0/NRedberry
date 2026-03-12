using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcSinTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        ArcSin function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        Assert.Same(argument, function[0]);
        Assert.Equal(1, function.Size);
        Assert.Equal("ArcSin[a]", function.ToString(OutputFormat.Redberry));
        Assert.IsType<Power>(derivative);
        Assert.Contains("a**2", derivative.ToString(OutputFormat.Redberry));
        Assert.Equal("ScalarFunctionBuilder", function.GetBuilder().GetType().Name);
        Assert.Same(ArcSinFactory.Factory, function.GetFactory());
    }
}
