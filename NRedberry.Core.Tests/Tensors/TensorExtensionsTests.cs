using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorExtensionsTests
{
    [Fact]
    public void ShouldCreatePowerViaExtensionMethod()
    {
        NRedberry.Tensors.Tensor result = TensorFactory.Parse("a").Pow(new Complex(2));

        Assert.IsType<Power>(result);
        Assert.Equal("a**2", result.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldMultiplyTensorsViaStaticAndExtensionHelpers()
    {
        NRedberry.Tensors.Tensor left = TensorFactory.Parse("a");
        NRedberry.Tensors.Tensor right = TensorFactory.Parse("b");

        NRedberry.Tensors.Tensor product = TensorExtensions.Multiply(left, right);
        NRedberry.Tensors.Tensor chained = left.Multiply(right);

        Assert.IsType<Product>(product);
        Assert.Equal(product.ToString(OutputFormat.Redberry), chained.ToString(OutputFormat.Redberry));
        Assert.Contains("a", product.ToString(OutputFormat.Redberry));
        Assert.Contains("b", product.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldCreateSumViaHelper()
    {
        NRedberry.Tensors.Tensor result = TensorExtensions.Sum(
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"));

        Sum sum = Assert.IsType<Sum>(result);
        Assert.Equal(2, sum.Size);
        Assert.Contains("a", sum.ToString(OutputFormat.Redberry));
        Assert.Contains("b", sum.ToString(OutputFormat.Redberry));
    }
}
