using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class TransformationExceptionTests
{
    [Fact]
    public void ShouldKeepMessageFromConstructor()
    {
        TransformationException exception = new("boom");

        Assert.Contains("boom", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ShouldAcceptTensorPayloadConstructor()
    {
        TransformationException exception = new(TensorApi.Parse("a"), TensorApi.Parse("b"));

        Assert.NotNull(exception);
    }
}
