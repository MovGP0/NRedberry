using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcCosTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        ArcCos function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        function[0].ShouldBeSameAs(argument);
        function.Size.ShouldBe(1);
        function.ToString(OutputFormat.Redberry).ShouldBe("ArcCos[a]");
        derivative.ShouldBeOfType<Product>();
        derivative.ToString(OutputFormat.Redberry).ShouldContain("a**2");
        function.GetBuilder().GetType().Name.ShouldBe("ScalarFunctionBuilder");
        function.GetFactory().ShouldBeSameAs(ArcCosFactory.Factory);
    }
}
