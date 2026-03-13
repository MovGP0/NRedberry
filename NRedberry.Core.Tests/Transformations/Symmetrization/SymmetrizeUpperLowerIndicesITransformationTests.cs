using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class SymmetrizeUpperLowerIndicesITransformationTests
{
    [Fact]
    public void ShouldThrowForSingletonAccessAndHelpers()
    {
        Assert.Throws<NotImplementedException>(() => _ = SymmetrizeUpperLowerIndicesITransformation.Instance);
        Assert.Throws<NotImplementedException>(() =>
            SymmetrizeUpperLowerIndicesITransformation.SymmetrizeUpperLowerIndices(TensorApi.Parse("A_ab")));
    }
}
