using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ExpTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        Exp function = new(argument);

        Assert.Same(argument, function[0]);
        Assert.Equal(1, function.Size);
        Assert.Equal("Exp[a]", function.ToString(OutputFormat.Redberry));
        Assert.Same(function, function.Derivative());
        Assert.Equal("ScalarFunctionBuilder", function.GetBuilder().GetType().Name);
        Assert.Same(ExpFactory.Factory, function.GetFactory());
    }
}
