using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcSinTests
{
    [Fact]
    public void ShouldExposeArgumentDerivativeAndFactory()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        ArcSin function = new(argument);

        NRedberry.Tensors.Tensor derivative = function.Derivative();

        function[0].ShouldBeSameAs(argument);
        function.Size.ShouldBe(1);
        function.ToString(OutputFormat.Redberry).ShouldBe("ArcSin[a]");
        derivative.ShouldBeOfType<Power>();
        derivative.ToString(OutputFormat.Redberry).ShouldContain("a**2");
        function.GetBuilder().GetType().Name.ShouldBe("ScalarFunctionBuilder");
        function.GetFactory().ShouldBeSameAs(ArcSinFactory.Factory);
    }
}
