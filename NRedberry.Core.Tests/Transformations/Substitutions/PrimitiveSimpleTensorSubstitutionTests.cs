using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSimpleTensorSubstitutionTests
{
    [Fact]
    public void ShouldThrowWhilePrimitiveSimpleTensorSubstitutionIsUnimplemented()
    {
        Should.Throw<NotImplementedException>(() =>
            new PrimitiveSimpleTensorSubstitution(TensorApi.Parse("a"), TensorApi.Parse("b")));
    }
}
