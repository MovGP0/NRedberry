using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class TransformationExceptionTests
{
    [Fact]
    public void ShouldKeepMessageFromConstructor()
    {
        TransformationException exception = new("boom");

        exception.Message.ShouldContain("boom");
    }

    [Fact]
    public void ShouldAcceptTensorPayloadConstructor()
    {
        TransformationException exception = new(TensorApi.Parse("a"), TensorApi.Parse("b"));

        exception.ShouldNotBeNull();
    }
}
