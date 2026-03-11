using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ProductTests
{
    [Fact]
    public void ShouldIgnoreUnitSignInHashCodeForProducts()
    {
        Assert.Equal(
            TensorFactory.Parse("(-1)*D*S").GetHashCode(),
            TensorFactory.Parse("D*S").GetHashCode());
    }

    [Fact]
    public void ShouldDifferentiateProductsWithAndWithoutNonUnitNumericFactors()
    {
        Assert.NotEqual(
            TensorFactory.Parse("2*D*S").GetHashCode(),
            TensorFactory.Parse("D*S").GetHashCode());
    }

    [Fact]
    public void ShouldReturnRequestedRangeFromIndexerSequence()
    {
        Product product = Assert.IsType<Product>(TensorFactory.Parse("2*e^i*A*B*C_i*N_j*T_r*a*b*15*R^jkl*B_kly"));

        NRedberry.Tensors.Tensor[] range = product.GetRange(1, 4);

        Assert.Equal(3, range.Length);
        Assert.Same(product[1], range[0]);
        Assert.Same(product[2], range[1]);
        Assert.Same(product[3], range[2]);
    }

    [Fact]
    public void ShouldExposeProductBuilderAndFactory()
    {
        Product product = Assert.IsType<Product>(TensorFactory.Parse("a*b*T_i"));

        Assert.IsType<ScalarsBackedProductBuilder>(product.GetBuilder());
        Assert.Same(ProductFactory.Factory, product.GetFactory());
    }

    [Fact]
    public void ShouldRoundTripNegativeProductsThroughStringForm()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("-a*b*g_mn");
        string text = tensor.ToString(OutputFormat.Redberry);

        Assert.Equal(text, TensorFactory.Parse(text).ToString(OutputFormat.Redberry));
    }
}
