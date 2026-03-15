using NRedberry.Tensors;
using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SubstitutionTransformationTests
{
    [Fact]
    public void ShouldApplyExpressionConstructors()
    {
        Expression expression = TensorApi.ParseExpression("a=b");

        TensorType result = new SubstitutionTransformation(expression)
            .Transform(TensorApi.Parse("a+c"));
        TensorType resultFromArray = new SubstitutionTransformation(new[] { expression }, true)
            .Transform(TensorApi.Parse("a+c"));

        result.ShouldSatisfyAllConditions(
            () => TensorUtils.Equals(result, TensorApi.Parse("b+c")).ShouldBeTrue(),
            () => TensorUtils.Equals(resultFromArray, TensorApi.Parse("b+c")).ShouldBeTrue());
    }

    [Fact]
    public void ShouldApplyTensorConstructors()
    {
        TensorType from = TensorApi.Parse("A_mn");
        TensorType to = TensorApi.Parse("B_m*C_n");
        TensorType target = TensorApi.Parse("A_ab*d");
        TensorType expected = TensorApi.Parse("B_a*C_b*d");

        TensorType result = new SubstitutionTransformation(from, to).Transform(target);
        TensorType arrayResult = new SubstitutionTransformation(new[] { from }, new[] { to }, true).Transform(target);

        result.ShouldSatisfyAllConditions(
            () => TensorUtils.Equals(result, expected).ShouldBeTrue(),
            () => TensorUtils.Equals(arrayResult, expected).ShouldBeTrue());
    }

    [Fact]
    public void ShouldTransposeSubstitution()
    {
        SubstitutionTransformation substitution = new(TensorApi.Parse("a"), TensorApi.Parse("b"));

        TensorType result = substitution.Transpose().Transform(TensorApi.Parse("b+c"));

        TensorUtils.Equals(result, TensorApi.Parse("a+c")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldCreateSimpleSubstitutionAndFormat()
    {
        SubstitutionTransformation substitution = new(TensorApi.Parse("a"), TensorApi.Parse("b"));

        SubstitutionTransformation simpleSubstitution = substitution.AsSimpleSubstitution();
        TensorType result = simpleSubstitution.Transform(TensorApi.Parse("a+c"));

        result.ShouldSatisfyAllConditions(
            () => TensorUtils.Equals(result, TensorApi.Parse("b+c")).ShouldBeTrue(),
            () => simpleSubstitution.ToString().ShouldBe("{a=b}"));
    }

    [Fact]
    public void ShouldRejectInconsistentFreeIndices()
    {
        Should.Throw<ArgumentException>(() =>
            new SubstitutionTransformation(TensorApi.Parse("A_m"), TensorApi.Parse("B_n")));
    }
}
