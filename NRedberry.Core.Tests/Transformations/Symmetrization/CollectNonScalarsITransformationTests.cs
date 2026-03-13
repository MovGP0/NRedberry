using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class CollectNonScalarsITransformationTests
{
    [Fact]
    public void ShouldThrowForSingletonAccess()
    {
        Assert.Throws<NotImplementedException>(() => _ = CollectNonScalarsITransformation.Instance);
    }

    [Fact]
    public void ShouldThrowForStaticCollectionHelper()
    {
        Assert.Throws<NotImplementedException>(() =>
            CollectNonScalarsITransformation.CollectNonScalars(TensorApi.Parse("a+b")));
    }
}
