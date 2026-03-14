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
        ShouldHaveIndexedFactors(
            actual,
            "G^{b'}_{c'}",
            "H^{a'}_{b'}",
            "U^{B'}_{C'}",
            "V^{A'}_{B'}");
        actual.Indices.ToString().ShouldBe(tensor.Indices.ToString());
    }

    [Fact]
    public void ShouldDescribeSelectedTypes()
    {
        ReverseTransformation transformation = new(IndexType.Matrix1, IndexType.Matrix2);

        transformation.ToString().ShouldBe("Reverse[Matrix1,Matrix2]");
        transformation.ToString(OutputFormat.Redberry).ShouldBe("Reverse[Matrix1,Matrix2]");
    }

    private static void ShouldHaveIndexedFactors(TensorType tensor, params string[] expected)
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
