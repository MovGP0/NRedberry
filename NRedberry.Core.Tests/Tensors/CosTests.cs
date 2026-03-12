using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class CosTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        Cos function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        Assert.Same(argument, function[0]);
        Assert.Equal(1, function.Size);
        Assert.Equal("Cos[a]", function.ToString(OutputFormat.Redberry));
        Assert.IsType<Product>(derivative);
        Assert.Contains("Sin[a]", derivative.ToString(OutputFormat.Redberry));
        Assert.Equal("ScalarFunctionBuilder", function.GetBuilder().GetType().Name);
        Assert.Same(CosFactory.Factory, function.GetFactory());
    }
}
