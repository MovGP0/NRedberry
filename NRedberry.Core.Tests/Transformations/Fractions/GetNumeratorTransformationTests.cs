using NRedberry.Transformations.Fractions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class GetNumeratorTransformationTests
{
    [Fact]
    public void ShouldExtractNumerator()
    {
        NRedberry.Tensors.Tensor actual = GetNumeratorTransformation.Instance.Transform(TensorApi.Parse("a/b"));

        actual.ToString(OutputFormat.Redberry).ShouldBe("a");
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        GetNumeratorTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("Numerator");
    }
}
