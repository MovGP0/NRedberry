using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class ProductBuilderTest
{
    [Fact]
    public void ShouldSimplifyRationalPowersInProduct()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a/(-a)**(1/2)");

        Assert.Equal("a*(-a)**(1/2)**(-1)", tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldSimplifyPowerSubtraction()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("(a+b)**(3/2) - (a+b)*(a+b)**(1/2)");

        Assert.Equal("(a+b)**(3/2)-(a+b)**(1/2)*(a+b)", tensor.ToString(OutputFormat.Redberry));
    }
}
