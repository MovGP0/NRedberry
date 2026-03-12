using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorContractionTests
{
    [Fact]
    public void ShouldSortCompareAndFormatContractions()
    {
        TensorContraction contraction = new(3, [Pack(2, 1, 4), Pack(1, 0, 2)]);

        contraction.SortContractions();

        Assert.Equal("3x{^2->1^0:^4->2^1}", contraction.ToString());
        Assert.Equal(0, contraction.CompareTo(new TensorContraction(3, [Pack(1, 0, 2), Pack(2, 1, 4)])));
    }

    [Fact]
    public void ShouldDetectFreeIndicesAndExposePackedParts()
    {
        long free = Pack(-1, 5, 7);
        TensorContraction contraction = new(1, [free]);

        Assert.True(contraction.ContainsFreeIndex());
        Assert.Equal(7, TensorContraction.GetFromIndexId(free));
        Assert.Equal(-1, TensorContraction.GetToTensorId(free));
        Assert.Equal(5, TensorContraction.GetToIndexId(free));
    }

    [Fact]
    public void ShouldUseValueEquality()
    {
        TensorContraction left = new(2, [Pack(1, 0, 3)]);
        TensorContraction right = new(2, [Pack(1, 0, 3)]);
        TensorContraction other = new(2, [Pack(1, 1, 3)]);

        Assert.Equal(left, right);
        Assert.True(left == right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.NotEqual(left, other);
        Assert.True(left != other);
    }

    private static long Pack(int toTensorId, int toIndexId, int fromIndexId)
    {
        return ((long)(ushort)fromIndexId << 32)
            | ((long)(ushort)toTensorId << 16)
            | (ushort)toIndexId;
    }
}
