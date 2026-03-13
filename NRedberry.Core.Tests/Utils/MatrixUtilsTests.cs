using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class MatrixUtilsTests
{
    [Fact]
    public void ShouldRecognizeMatrixVectorAndCovectorShapes()
    {
        SimpleTensor matrix = CreateSimpleTensor(
            IndicesUtils.CreateIndex(1, IndexType.Matrix1, true),
            IndicesUtils.CreateIndex(2, IndexType.Matrix1, false));
        SimpleTensor vector = CreateSimpleTensor(IndicesUtils.CreateIndex(3, IndexType.Matrix1, true));
        SimpleTensor covector = CreateSimpleTensor(IndicesUtils.CreateIndex(4, IndexType.Matrix1, false));

        Assert.True(MatrixUtils.IsMatrix(matrix, IndexType.Matrix1));
        Assert.True(MatrixUtils.IsVector(vector, IndexType.Matrix1));
        Assert.True(MatrixUtils.IsCovector(covector, IndexType.Matrix1));
        Assert.False(MatrixUtils.IsVector(covector, IndexType.Matrix1));
    }

    [Fact]
    public void ShouldRejectMetricIndexTypes()
    {
        SimpleTensor tensor = CreateSimpleTensor(IndicesUtils.CreateIndex(1, IndexType.LatinLower, true));

        Assert.Throws<ArgumentException>(() => MatrixUtils.IsVector(tensor, IndexType.LatinLower));
    }

    private static SimpleTensor CreateSimpleTensor(params int[] indices)
    {
        return new SimpleTensor(1, IndicesFactory.CreateSimple(null, indices));
    }
}
