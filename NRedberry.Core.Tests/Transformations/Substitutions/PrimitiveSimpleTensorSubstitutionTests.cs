using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSimpleTensorSubstitutionTests
{
    [Fact]
    public void ShouldConstructPrimitiveSimpleTensorSubstitution()
    {
        PrimitiveSimpleTensorSubstitution substitution =
            new(TensorApi.Parse("a"), TensorApi.Parse("b"));

        substitution.ShouldNotBeNull();
    }
}
