using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class ProductBuilderTest
{
    [Fact]
    public void ShouldBuildProductContainingInverseRationalPower()
    {
        Product tensor = Assert.IsType<Product>(TensorFactory.Parse("a/(-a)**(1/2)"));

        Assert.Equal(2, tensor.Size);
        Assert.Contains(tensor, term => term is SimpleTensor);
        Assert.Contains(
            tensor,
            term => term is Power power
                && TensorUtils.EqualsExactly(Complex.MinusOne, power[1]));
    }

    [Fact]
    public void ShouldBuildDifferenceOfProductAndPowerTerms()
    {
        Sum tensor = Assert.IsType<Sum>(TensorFactory.Parse("(a+b)**(3/2) - (a+b)*(a+b)**(1/2)"));

        Assert.Equal(2, tensor.Size);
        Assert.Contains(tensor, term => term is Product);
        Assert.Contains(tensor, term => term is SimpleTensor);
    }
}
