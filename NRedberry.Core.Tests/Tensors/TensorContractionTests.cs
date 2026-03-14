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

        contraction.ToString().ShouldBe("3x{^2->1^0:^4->2^1}");
        contraction.CompareTo(new TensorContraction(3, [Pack(1, 0, 2), Pack(2, 1, 4)])).ShouldBe(0);
    }

    [Fact]
    public void ShouldDetectFreeIndicesAndExposePackedParts()
    {
        long free = Pack(-1, 5, 7);
        TensorContraction contraction = new(1, [free]);

        contraction.ContainsFreeIndex().ShouldBeTrue();
        TensorContraction.GetFromIndexId(free).ShouldBe(7);
        TensorContraction.GetToTensorId(free).ShouldBe(-1);
        TensorContraction.GetToIndexId(free).ShouldBe(5);
    }

    [Fact]
    public void ShouldUseValueEquality()
    {
        TensorContraction left = new(2, [Pack(1, 0, 3)]);
        TensorContraction right = new(2, [Pack(1, 0, 3)]);
        TensorContraction other = new(2, [Pack(1, 1, 3)]);

        right.ShouldBe(left);
        left == right.ShouldBeTrue();
        right.GetHashCode().ShouldBe(left.GetHashCode());
        other.ShouldNotBe(left);
        left != other.ShouldBeTrue();
    }

    private static long Pack(int toTensorId, int toIndexId, int fromIndexId)
    {
        return ((long)(ushort)fromIndexId << 32)
            | ((long)(ushort)toTensorId << 16)
            | (ushort)toIndexId;
    }
}
