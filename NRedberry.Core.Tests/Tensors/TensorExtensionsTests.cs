using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorExtensionsTests
{
    [Fact]
    public void ShouldCreatePowerViaExtensionMethod()
    {
        NRedberry.Tensors.Tensor result = TensorFactory.Parse("a").Pow(new Complex(2));

        result.ShouldBeOfType<Power>();
        result.ToString(OutputFormat.Redberry).ShouldBe("a**2");
    }

    [Fact]
    public void ShouldMultiplyTensorsViaStaticAndExtensionHelpers()
    {
        NRedberry.Tensors.Tensor left = TensorFactory.Parse("a");
        NRedberry.Tensors.Tensor right = TensorFactory.Parse("b");

        NRedberry.Tensors.Tensor product = TensorExtensions.Multiply(left, right);
        NRedberry.Tensors.Tensor chained = left.Multiply(right);

        product.ShouldBeOfType<Product>();
        chained.ToString(OutputFormat.Redberry).ShouldBe(product.ToString(OutputFormat.Redberry));
        product.ToString(OutputFormat.Redberry).ShouldContain("a");
        product.ToString(OutputFormat.Redberry).ShouldContain("b");
    }

    [Fact]
    public void ShouldCreateSumViaHelper()
    {
        NRedberry.Tensors.Tensor result = TensorExtensions.Sum(
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"));

        Sum sum = result.ShouldBeOfType<Sum>();
        sum.Size.ShouldBe(2);
        sum.ToString(OutputFormat.Redberry).ShouldContain("a");
        sum.ToString(OutputFormat.Redberry).ShouldContain("b");
    }
}
