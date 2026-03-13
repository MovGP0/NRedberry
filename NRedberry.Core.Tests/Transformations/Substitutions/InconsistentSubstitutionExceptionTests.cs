using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class InconsistentSubstitutionExceptionTests
{
    [Fact]
    public void ShouldThrowWhileExceptionPayloadIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() =>
            new InconsistentSubstitutionException(
                TensorApi.Parse("a"),
                TensorApi.Parse("b"),
                TensorApi.Parse("c")));
    }
}
