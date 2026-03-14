using NRedberry;
using NRedberry.Tensors;
using NRedberry.Transformations.Reverse;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Reverse;

public sealed class ReverseTransformationTest
{
    [Fact]
    public void ShouldReverseEachRequestedMatrixTypeSequentially()
    {
        ReverseTransformation transformation = new(IndexType.Matrix2, IndexType.Matrix1);
        TensorType tensor = TensorApi.Parse("G^a'_b'*H^b'_c'*U^A'_B'*V^B'_C'");

        TensorType actual = transformation.Transform(tensor);
        AssertIndexedFactors(
            actual,
            "G^{b'}_{c'}",
            "H^{a'}_{b'}",
            "U^{B'}_{C'}",
            "V^{A'}_{B'}");
        Assert.Equal(tensor.Indices.ToString(), actual.Indices.ToString());
    }

    [Fact]
    public void ShouldDescribeSelectedTypes()
    {
        ReverseTransformation transformation = new(IndexType.Matrix1, IndexType.Matrix2);

        Assert.Equal("Reverse[Matrix1,Matrix2]", transformation.ToString());
        Assert.Equal("Reverse[Matrix1,Matrix2]", transformation.ToString(OutputFormat.Redberry));
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
