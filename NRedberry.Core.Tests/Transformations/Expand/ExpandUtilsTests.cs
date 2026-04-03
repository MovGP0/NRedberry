using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandUtilsTests
{
    [Fact]
    public void ShouldDetectExpandablePower()
    {
        ExpandUtils.IsExpandablePower(TensorApi.Parse("Power[a+b,2]")).ShouldBeTrue();
        ExpandUtils.IsExpandablePower(TensorApi.Parse("Power[a+b,x]")).ShouldBeFalse();
    }

    [Fact]
    public void ShouldDetectIndexedSummands()
    {
        ExpandUtils.SumContainsIndexed(TensorApi.Parse("A_i+B_i")).ShouldBeTrue();
        ExpandUtils.SumContainsIndexed(TensorApi.Parse("a+b")).ShouldBeFalse();
    }

    [Fact]
    public void ShouldApplyTransformationsSequentially()
    {
        NRedberry.Tensors.Tensor actual = ExpandUtils.Apply(
            [GetNumeratorTransformation.Instance, GetDenominatorTransformation.Instance],
            TensorApi.Parse("a/b"));

        actual.ToString(OutputFormat.Redberry).ShouldBe("1");
    }

    [Fact]
    public void ShouldExpandPowerIntoSeparateFactors()
    {
        IList<NRedberry.Tensors.Tensor> expanded = NumeratorDenominator.ExpandPower(TensorApi.Parse("Power[a,x+y]"));

        expanded
            .Select(tensor => tensor.ToString(OutputFormat.Redberry))
            .OrderBy(term => term, StringComparer.Ordinal)
            .ToArray()
            .ShouldBe(["a**x", "a**y"]);
    }
}
