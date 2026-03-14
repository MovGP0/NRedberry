using NRedberry;
using NRedberry.Tensors;
using NRedberry.Transformations.Reverse;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Reverse;

public sealed class SingleReverseTests
{
    [Fact]
    public void ShouldRejectMetricIndexTypes()
    {
        Should.Throw<ArgumentException>(() => new SingleReverse(IndexType.LatinLower));
    }

    [Fact]
    public void ShouldReverseMatrixLinePreservingOuterIndices()
    {
        TensorType tensor = TensorApi.Parse("A^a'_b'*B^b'_c'*C^c'_d'");

        TensorType actual = SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);
        AssertIndexedFactors(
            actual,
            "A^{c'}_{d'}",
            "B^{b'}_{c'}",
            "C^{a'}_{b'}");
        actual.Indices.ToString().ShouldBe(tensor.Indices.ToString());
    }

    [Fact]
    public void ShouldReverseOnlyMatricesInsideCovectorMatrixVectorLine()
    {
        TensorType tensor = TensorApi.Parse("cv_b'*A^b'_c'*B^c'_d'*v^d'");

        TensorType actual = SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);
        AssertIndexedFactors(
            actual,
            "A^{c'}_{d'}",
            "B^{b'}_{c'}",
            "cv_{b'}",
            "v^{d'}");
        actual.Indices.ToString().ShouldBe(tensor.Indices.ToString());
    }

    [Fact]
    public void ShouldLeaveCyclesUntouched()
    {
        TensorType tensor = TensorApi.Parse("A^a'_b'*B^b'_a'");

        TensorType actual = SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);

        TensorUtils.Equals(tensor, actual).ShouldBeTrue();
    }

    [Fact]
    public void ShouldRejectNonMatrixFactorsOfRequestedType()
    {
        TensorType tensor = TensorApi.Parse("A^a'_b'_c'*B^c'_d'");

        Action action = () => SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);

        Should.Throw<ArgumentException>(action);
    }

    private static void AssertIndexedFactors(TensorType tensor, params string[] expected)
    {
        string[] actual = GetIndexedFactorTexts(tensor);
        string[] sortedExpected = expected.OrderBy(text => text, StringComparer.Ordinal).ToArray();

        actual.ShouldBe(sortedExpected);
    }

    private static string[] GetIndexedFactorTexts(TensorType tensor)
    {
        TensorType[] factors = tensor is Product product ? product.Data : [tensor];
        return factors
            .Select(static factor => factor.ToString())
            .OrderBy(static text => text, StringComparer.Ordinal)
            .ToArray();
    }
}
