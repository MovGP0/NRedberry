using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

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
