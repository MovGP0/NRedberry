using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSumSubstitutionTests
{
    [Fact]
    public void ShouldThrowWhilePrimitiveSumSubstitutionIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() =>
            new PrimitiveSumSubstitution(TensorApi.Parse("a+b"), TensorApi.Parse("c")));
    }
}
