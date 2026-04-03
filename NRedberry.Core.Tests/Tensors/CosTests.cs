using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class CosTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        Cos function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        function[0].ShouldBeSameAs(argument);
        function.Size.ShouldBe(1);
        function.ToString(OutputFormat.Redberry).ShouldBe("Cos[a]");
        derivative.ShouldBeOfType<Product>();
        derivative.ToString(OutputFormat.Redberry).ShouldContain("Sin[a]");
        function.GetBuilder().GetType().Name.ShouldBe("ScalarFunctionBuilder");
        function.GetFactory().ShouldBeSameAs(CosFactory.Factory);
    }
}
