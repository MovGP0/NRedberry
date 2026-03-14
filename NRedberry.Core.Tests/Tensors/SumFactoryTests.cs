using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumFactoryTests
{
    [Fact]
    public void ShouldReturnZeroForEmptyInput()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;

        factory.Create().ShouldBe(Complex.Zero);
    }

    [Fact]
    public void ShouldReturnSingleTensorWithoutWrapping()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a");

        factory.Create(tensor).ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldBuildSumFromMultipleDistinctTerms()
    {
        NRedberry.Tensors.TensorFactory factory = SumFactory.Factory;

        NRedberry.Tensors.Tensor result = factory.Create(
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"));

        Sum sum = result.ShouldBeOfType<Sum>();
        sum.Size.ShouldBe(2);
        sum.ToString(OutputFormat.Redberry).ShouldContain("a");
        sum.ToString(OutputFormat.Redberry).ShouldContain("b");
    }
}
