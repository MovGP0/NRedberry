using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class CollectScalarFactorsITransformationTests
{
    [Fact]
    public void ShouldLeaveNonProductExpressionsUntouched()
    {
        var tensor = TensorApi.Parse("a+b");

        Assert.Same(tensor, CollectScalarFactorsITransformation.Instance.Transform(tensor));
    }

    [Fact]
    public void ShouldExposeWorkingConstructorsAndHelpers()
    {
        var tensor = TensorApi.Parse("a+b");
        CollectScalarFactorsITransformation transformation = new(TraverseGuide.All);

        Assert.Same(tensor, transformation.Transform(tensor));
        Assert.Same(tensor, CollectScalarFactorsITransformation.CollectScalarFactors(tensor));
    }
}
