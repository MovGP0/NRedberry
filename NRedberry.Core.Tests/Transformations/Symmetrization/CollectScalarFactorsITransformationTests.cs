using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class CollectScalarFactorsITransformationTests
{
    [Fact]
    public void ShouldThrowForSingletonAccess()
    {
        Assert.Throws<NotImplementedException>(() => _ = CollectScalarFactorsITransformation.Instance);
    }

    [Fact]
    public void ShouldThrowForConstructorsAndHelpers()
    {
        Assert.Throws<NotImplementedException>(() => new CollectScalarFactorsITransformation(TraverseGuide.All));
        Assert.Throws<NotImplementedException>(() =>
            CollectScalarFactorsITransformation.CollectScalarFactors(TensorApi.Parse("a+b")));
    }
}
