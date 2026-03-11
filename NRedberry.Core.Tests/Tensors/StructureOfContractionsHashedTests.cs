using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class StructureOfContractionsHashedTests
{
    [Fact]
    public void ShouldExposeStoredContractionsAndFormatting()
    {
        TensorContraction free = new(-1, []);
        TensorContraction first = new(0, [Pack(1, 0, 0)]);
        TensorContraction second = new(2, [Pack(-1, 1, 0)]);
        StructureOfContractionsHashed hashed = new(free, first, second);

        Assert.Same(free, hashed.FreeContraction);
        Assert.Same(first, hashed[0]);
        Assert.Same(second, hashed.Get(1));
        Assert.Contains("Free: -1x", hashed.ToString());
        Assert.Contains("0x{^0->1^0}", hashed.ToString());
    }

    [Fact]
    public void ShouldCompareByFreeAndStoredContractions()
    {
        StructureOfContractionsHashed left = new(
            new TensorContraction(-1, []),
            new TensorContraction(0, [Pack(1, 0, 0)]));
        StructureOfContractionsHashed right = new(
            new TensorContraction(-1, []),
            new TensorContraction(0, [Pack(1, 0, 0)]));
        StructureOfContractionsHashed other = new(
            new TensorContraction(-1, []),
            new TensorContraction(0, [Pack(2, 0, 0)]));

        Assert.Equal(left, right);
        Assert.True(left == right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.NotEqual(left, other);
        Assert.True(left != other);
    }

    [Fact]
    public void ShouldExposeEmptyInstance()
    {
        StructureOfContractionsHashed empty = StructureOfContractionsHashed.EmptyInstance;

        Assert.Equal((short)-1, empty.FreeContraction.TensorId);
        Assert.Empty(empty.FreeContraction.IndexContractions);
    }

    private static long Pack(int toTensorId, int toIndexId, int fromIndexId)
    {
        return ((long)(ushort)fromIndexId << 32)
            | ((long)(ushort)toTensorId << 16)
            | (ushort)toIndexId;
    }
}
