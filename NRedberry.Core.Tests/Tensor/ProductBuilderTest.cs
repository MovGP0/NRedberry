using NRedberry.Numbers;
using NRedberry.Tensors;
using System.Linq;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class ProductBuilderTest
{
    [Fact]
    public void ShouldBuildProductContainingInverseRationalPower()
    {
        Product tensor = TensorFactory.Parse("a/(-a)**(1/2)").ShouldBeOfType<Product>();

        tensor.Size.ShouldBe(2);
        tensor.Any(term => term is SimpleTensor).ShouldBeTrue();
        bool containsInversePower = tensor.Any(term => term is Power power
            && TensorUtils.EqualsExactly(Complex.MinusOne, power[1]));
        containsInversePower.ShouldBeTrue();
    }

    [Fact]
    public void ShouldBuildDifferenceOfProductAndPowerTerms()
    {
        Sum tensor = TensorFactory.Parse("(a+b)**(3/2) - (a+b)*(a+b)**(1/2)").ShouldBeOfType<Sum>();

        tensor.Size.ShouldBe(2);
        tensor.Any(term => term is Product).ShouldBeTrue();
        tensor.Any(term => term is SimpleTensor).ShouldBeTrue();
    }
}
