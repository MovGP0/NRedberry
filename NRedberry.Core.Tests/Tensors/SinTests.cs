using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SinTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        Sin function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        function[0].ShouldBeSameAs(argument);
        function.Size.ShouldBe(1);
        function.ToString(OutputFormat.Redberry).ShouldBe("Sin[a]");
        derivative.ShouldBeOfType<Cos>();
        derivative.ToString(OutputFormat.Redberry).ShouldBe("Cos[a]");
        function.GetBuilder().GetType().Name.ShouldBe("ScalarFunctionBuilder");
        function.GetFactory().ShouldBeSameAs(SinFactory.Factory);
    }
}
