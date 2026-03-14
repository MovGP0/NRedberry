using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class InconsistentSubstitutionExceptionTests
{
    [Fact]
    public void ShouldCaptureOriginalSubstitutionMessage()
    {
        InconsistentSubstitutionException exception = new(
            TensorApi.Parse("a"),
            TensorApi.Parse("b"),
            TensorApi.Parse("c"));

        Assert.Equal("Substitution: a = b.  Detected tensor c", exception.Message);
    }
}
