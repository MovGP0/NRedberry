using System.Numerics;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorsTests
{
    [Fact]
    public void ShouldCreatePowersFromNumericHelpers()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Equal("a**2", TensorApi.Pow(argument, 2).ToString(OutputFormat.Redberry));
        Assert.Equal("a**3", TensorApi.Pow(argument, new BigInteger(3)).ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldMultiplySumAndNegateThroughHelpers()
    {
        NRedberry.Tensors.Tensor left = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor right = TensorApi.Parse("b");

        NRedberry.Tensors.Tensor product = TensorApi.Multiply([left, right]);
        NRedberry.Tensors.Tensor sum = TensorApi.Sum([left, right]);
        NRedberry.Tensors.Tensor negated = TensorApi.Negate(left);

        Assert.IsType<Product>(product);
        Assert.IsType<Sum>(sum);
        Assert.IsType<Product>(negated);
        Assert.Contains("a", product.ToString(OutputFormat.Redberry));
        Assert.Contains("b", product.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldParseHelpersAndGuardSpecializedParsers()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor expressionTensor = TensorApi.Parse("a=b");

        Assert.Same(tensor, TensorApi.Parse(tensor));
        Assert.Equal(2, TensorApi.Parse("a", "b").Length);
        Assert.IsType<SimpleTensor>(TensorApi.ParseSimple("a"));
        Assert.IsType<Expression>(TensorApi.ParseExpression("a=b"));
        Assert.Throws<ArgumentException>(() => TensorApi.ParseSimple("a+b"));
        Assert.Throws<ArgumentException>(() => TensorApi.ParseExpression("a+b"));
        Assert.IsType<Expression>(expressionTensor);
    }
}
