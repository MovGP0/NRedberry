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
        TensorType expected = TensorApi.Parse("H^a'_b'*G^b'_c'*V^A'_B'*U^B'_C'");

        Assert.True(TensorUtils.Equals(expected, actual));
    }

    [Fact]
    public void ShouldDescribeSelectedTypes()
    {
        ReverseTransformation transformation = new(IndexType.Matrix1, IndexType.Matrix2);

        Assert.Equal("Reverse[Matrix1,Matrix2]", transformation.ToString());
        Assert.Equal("Reverse[Matrix1,Matrix2]", transformation.ToString(OutputFormat.Redberry));
    }
}
