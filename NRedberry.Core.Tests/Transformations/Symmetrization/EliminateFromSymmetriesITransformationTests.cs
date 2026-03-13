using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateFromSymmetriesITransformationTests
{
    [Fact]
    public void ShouldThrowWhileTransformationIsUnimplemented()
    {
        EliminateFromSymmetriesITransformation transformation = new();

        Assert.Throws<NotImplementedException>(() => transformation.Transform(TensorApi.Parse("a+b")));
    }
}
