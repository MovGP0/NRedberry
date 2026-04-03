using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ComplexConjugateTransformationTests
{
    [Fact]
    public void ShouldConjugateComplexNumbers()
    {
        var actual = ComplexConjugateTransformation.Instance.Transform(TensorApi.Parse("1+I"));

        NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("1-I")).ShouldBeTrue();
        ComplexConjugateTransformation.Instance.ToString().ShouldBe("Conjugate");
    }
}
