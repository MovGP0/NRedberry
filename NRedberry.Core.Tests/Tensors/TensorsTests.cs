using System.Numerics;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorsTests
{
    [Fact]
    public void ShouldCreatePowersFromNumericHelpers()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        TensorApi.Pow(argument, 2).ToString(OutputFormat.Redberry).ShouldBe("a**2");
        TensorApi.Pow(argument, new BigInteger(3)).ToString(OutputFormat.Redberry).ShouldBe("a**3");
    }

    [Fact]
    public void ShouldMultiplySumAndNegateThroughHelpers()
    {
        NRedberry.Tensors.Tensor left = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor right = TensorApi.Parse("b");

        NRedberry.Tensors.Tensor product = TensorApi.Multiply([left, right]);
        NRedberry.Tensors.Tensor sum = TensorApi.Sum([left, right]);
        NRedberry.Tensors.Tensor negated = TensorApi.Negate(left);

        product.ShouldBeOfType<Product>();
        sum.ShouldBeOfType<Sum>();
        negated.ShouldBeOfType<Product>();
        product.ToString(OutputFormat.Redberry).ShouldContain("a");
        product.ToString(OutputFormat.Redberry).ShouldContain("b");
    }

    [Fact]
    public void ShouldParseHelpersAndGuardSpecializedParsers()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor expressionTensor = TensorApi.Parse("a=b");

        TensorApi.Parse(tensor).ShouldBeSameAs(tensor);
        TensorApi.Parse("a", "b").Length.ShouldBe(2);
        TensorApi.ParseSimple("a").ShouldBeOfType<SimpleTensor>();
        TensorApi.ParseExpression("a=b").ShouldBeOfType<Expression>();
        Should.Throw<ArgumentException>(() => TensorApi.ParseSimple("a+b"));
        Should.Throw<ArgumentException>(() => TensorApi.ParseExpression("a+b"));
        expressionTensor.ShouldBeOfType<Expression>();
    }
}
