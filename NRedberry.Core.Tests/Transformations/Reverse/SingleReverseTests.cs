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
        TensorType expected = TensorApi.Parse("C^a'_b'*B^b'_c'*A^c'_d'");

        Assert.True(TensorUtils.Equals(expected, actual));
    }

    [Fact]
    public void ShouldReverseOnlyMatricesInsideCovectorMatrixVectorLine()
    {
        TensorType tensor = TensorApi.Parse("cv_b'*A^b'_c'*B^c'_d'*v^d'");

        TensorType actual = SingleReverse.InverseOrderOfMatrices(tensor, IndexType.Matrix1);
        TensorType expected = TensorApi.Parse("cv_b'*B^b'_c'*A^c'_d'*v^d'");

        Assert.True(TensorUtils.Equals(expected, actual));
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
}
