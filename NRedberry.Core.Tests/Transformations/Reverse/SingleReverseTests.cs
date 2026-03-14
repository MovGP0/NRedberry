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
        Assert.Throws<ArgumentException>(() => new SingleReverse(IndexType.LatinLower));
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
        Assert.Equal(tensor.Indices.ToString(), actual.Indices.ToString());
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
        Assert.Equal(tensor.Indices.ToString(), actual.Indices.ToString());
    }

    [Fact]
    public void ShouldLeaveCyclesUntouched()
    {
        TensorType tensor = TensorApi.Parse("A^a'_b'*B^b'_a'");

        TensorType actual = SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);

        Assert.True(TensorUtils.Equals(tensor, actual));
    }

    [Fact]
    public void ShouldRejectNonMatrixFactorsOfRequestedType()
    {
        TensorType tensor = TensorApi.Parse("A^a'_b'_c'*B^c'_d'");

        Action action = () => SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);

        Assert.Throws<ArgumentException>(action);
    }

    private static void AssertIndexedFactors(TensorType tensor, params string[] expected)
    {
        string[] actual = GetIndexedFactorTexts(tensor);
        string[] sortedExpected = expected.OrderBy(text => text, StringComparer.Ordinal).ToArray();

        Assert.Equal(sortedExpected, actual);
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
