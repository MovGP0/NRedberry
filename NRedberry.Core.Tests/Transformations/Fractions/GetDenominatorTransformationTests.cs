using NRedberry.Transformations.Fractions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class GetDenominatorTransformationTests
{
    [Fact]
    public void ShouldExtractDenominator()
    {
        NRedberry.Tensors.Tensor actual = GetDenominatorTransformation.Instance.Transform(TensorApi.Parse("a/b"));

        actual.ToString(OutputFormat.Redberry).ShouldBe("b");
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        GetDenominatorTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("Denominator");
    }
}
