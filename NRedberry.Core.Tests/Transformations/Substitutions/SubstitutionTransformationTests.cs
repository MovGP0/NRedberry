using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SubstitutionTransformationTests
{
    [Fact]
    public void ShouldThrowForExpressionConstructors()
    {
        Should.Throw<NotImplementedException>(() =>
            new SubstitutionTransformation(TensorApi.ParseExpression("a=b")));
        Should.Throw<NotImplementedException>(() =>
            new SubstitutionTransformation([TensorApi.ParseExpression("a=b")], true));
    }

    [Fact]
    public void ShouldThrowForTensorConstructors()
    {
        Should.Throw<NotImplementedException>(() =>
            new SubstitutionTransformation(TensorApi.Parse("a"), TensorApi.Parse("b")));
        Should.Throw<NotImplementedException>(() =>
            new SubstitutionTransformation([TensorApi.Parse("a")], [TensorApi.Parse("b")], true));
    }
}
