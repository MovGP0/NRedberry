using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveTensorFieldSubstitutionTests
{
    [Fact]
    public void ShouldConstructPrimitiveTensorFieldSubstitution()
    {
        PrimitiveTensorFieldSubstitution substitution =
            new(TensorApi.Parse("f[x]"), TensorApi.Parse("g[x]"));

        substitution.ShouldNotBeNull();
    }
}
