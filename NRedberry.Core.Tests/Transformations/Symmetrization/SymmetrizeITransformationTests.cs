using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class SymmetrizeITransformationTests
{
    [Fact]
    public void ShouldThrowForConstructors()
    {
        NRedberry.Tensors.SimpleTensor tensor = Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("A_ab"));

        Assert.Throws<NotImplementedException>(() =>
            new SymmetrizeITransformation(tensor.SimpleIndices, false));
        Assert.Throws<NotImplementedException>(() =>
            new SymmetrizeITransformation(tensor.SimpleIndices, new SymmetrizeITransformation.SymmetrizeOptions()));
    }
}
