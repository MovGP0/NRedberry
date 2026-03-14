using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveTensorFieldSubstitutionTests
{
    [Fact]
    public void ShouldThrowWhilePrimitiveTensorFieldSubstitutionIsUnimplemented()
    {
        Should.Throw<NotImplementedException>(() =>
            new PrimitiveTensorFieldSubstitution(TensorApi.Parse("f[x]"), TensorApi.Parse("g[x]")));
    }
}
