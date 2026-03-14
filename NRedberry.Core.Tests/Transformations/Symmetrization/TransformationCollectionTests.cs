using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class TransformationCollectionTests
{
    [Fact]
    public void ShouldApplyTransformationsSequentially()
    {
        TransformationCollection collection = new(
            GetNumeratorTransformation.Instance,
            GetDenominatorTransformation.Instance);

        NRedberry.Tensors.Tensor actual = collection.Transform(TensorApi.Parse("a/b"));

        actual.ToString(OutputFormat.Redberry).ShouldBe("1");
    }

    [Fact]
    public void ShouldRenderJoinedTransformationNames()
    {
        TransformationCollection collection = new(
            GetNumeratorTransformation.Instance,
            GetDenominatorTransformation.Instance);

        collection.ToString(OutputFormat.Redberry).ShouldBe("Numerator & Denominator");
    }
}
