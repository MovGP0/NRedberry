using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensor;

public sealed class ProductBuilderTest
{
    [Fact]
    public void ShouldBuildProductContainingInverseRationalPower()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a/(-a)**(1/2)");

        TensorUtils.Equals(tensor, TensorFactory.Parse("-(-a)**(1/2)")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldBuildDifferenceOfProductAndPowerTerms()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("(a+b)**(3/2) - (a+b)*(a+b)**(1/2)");

        tensor.ShouldBe(Complex.Zero);
    }
}
