using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ComplexConjugateTransformationTests
{
    [Fact]
    public void ShouldConjugateComplexNumbers()
    {
        var actual = ComplexConjugateTransformation.Instance.Transform(TensorApi.Parse("1+I"));

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("1-I")));
        Assert.Equal("Conjugate", ComplexConjugateTransformation.Instance.ToString());
    }
}
