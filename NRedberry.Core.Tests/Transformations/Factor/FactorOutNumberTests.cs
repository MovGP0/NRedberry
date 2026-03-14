using NRedberry.Transformations.Factor;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class FactorOutNumberTests
{
    [Fact]
    public void ShouldLeaveMixedNumericAndSymbolicSumUntouched()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("I*a+f");

        NRedberry.Tensors.Tensor actual = FactorOutNumber.Instance.Transform(tensor);

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        FactorOutNumber.Instance.ToString(OutputFormat.Redberry).ShouldBe("FactorOutNumber");
    }
}
