using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SinTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        Sin function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        Assert.Same(argument, function[0]);
        Assert.Equal(1, function.Size);
        Assert.Equal("Sin[a]", function.ToString(OutputFormat.Redberry));
        Assert.IsType<Cos>(derivative);
        Assert.Equal("Cos[a]", derivative.ToString(OutputFormat.Redberry));
        Assert.Equal("ScalarFunctionBuilder", function.GetBuilder().GetType().Name);
        Assert.Same(SinFactory.Factory, function.GetFactory());
    }
}
