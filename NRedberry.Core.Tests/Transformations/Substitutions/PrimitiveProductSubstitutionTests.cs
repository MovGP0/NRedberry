using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveProductSubstitutionTests
{
    [Fact]
    public void ShouldThrowWhilePrimitiveProductSubstitutionIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() =>
            new PrimitiveProductSubstitution(TensorApi.Parse("a*b"), TensorApi.Parse("c")));
    }
}
