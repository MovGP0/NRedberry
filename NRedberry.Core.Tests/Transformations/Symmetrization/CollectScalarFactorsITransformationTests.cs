using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class CollectScalarFactorsITransformationTests
{
    [Fact]
    public void ShouldLeaveNonProductExpressionsUntouched()
    {
        var tensor = TensorApi.Parse("a+b");

        CollectScalarFactorsITransformation.Instance.Transform(tensor).ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldExposeWorkingConstructorsAndHelpers()
    {
        var tensor = TensorApi.Parse("a+b");
        CollectScalarFactorsITransformation transformation = new(TraverseGuide.All);

        transformation.Transform(tensor).ShouldBeSameAs(tensor);
        CollectScalarFactorsITransformation.CollectScalarFactors(tensor).ShouldBeSameAs(tensor);
    }
}
