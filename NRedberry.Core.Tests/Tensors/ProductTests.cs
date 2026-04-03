using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ProductTests
{
    [Fact]
    public void ShouldIgnoreUnitSignInHashCodeForProducts()
    {
        TensorFactory.Parse("D*S").GetHashCode().ShouldBe(TensorFactory.Parse("(-1)*D*S").GetHashCode());
    }

    [Fact]
    public void ShouldDifferentiateProductsWithAndWithoutNonUnitNumericFactors()
    {
        TensorFactory.Parse("D*S").GetHashCode().ShouldNotBe(TensorFactory.Parse("2*D*S").GetHashCode());
    }

    [Fact]
    public void ShouldReturnRequestedRangeFromIndexerSequence()
    {
        Product product = TensorFactory.Parse("2*e^i*A*B*C_i*N_j*T_r*a*b*15*R^jkl*B_kly").ShouldBeOfType<Product>();

        NRedberry.Tensors.Tensor[] range = product.GetRange(1, 4);

        range.Length.ShouldBe(3);
        range[0].ShouldBeSameAs(product[1]);
        range[1].ShouldBeSameAs(product[2]);
        range[2].ShouldBeSameAs(product[3]);
    }

    [Fact]
    public void ShouldExposeProductBuilderAndFactory()
    {
        Product product = TensorFactory.Parse("a*b*T_i").ShouldBeOfType<Product>();

        product.GetBuilder().ShouldBeOfType<ScalarsBackedProductBuilder>();
        product.GetFactory().ShouldBeSameAs(ProductFactory.Factory);
    }

    [Fact]
    public void ShouldRoundTripNegativeProductsThroughStringForm()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("-a*b*g_mn");
        string text = tensor.ToString(OutputFormat.Redberry);

        TensorFactory.Parse(text).ToString(OutputFormat.Redberry).ShouldBe(text);
    }
}
