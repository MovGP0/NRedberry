using NRedberry.Tensors;

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

        hashed.FreeContraction.ShouldBeSameAs(free);
        hashed[0].ShouldBeSameAs(first);
        hashed.Get(1).ShouldBeSameAs(second);
        hashed.ToString().ShouldContain("Free: -1x");
        hashed.ToString().ShouldContain("0x{^0->1^0}");
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

        right.ShouldBe(left);
        (left == right).ShouldBeTrue();
        right.GetHashCode().ShouldBe(left.GetHashCode());
        other.ShouldNotBe(left);
        (left != other).ShouldBeTrue();
    }

    [Fact]
    public void ShouldExposeEmptyInstance()
    {
        StructureOfContractionsHashed empty = StructureOfContractionsHashed.EmptyInstance;

        empty.FreeContraction.TensorId.ShouldBe((short)-1);
        empty.FreeContraction.IndexContractions.ShouldBeEmpty();
    }

    private static long Pack(int toTensorId, int toIndexId, int fromIndexId)
    {
        return ((long)(ushort)fromIndexId << 32)
            | ((long)(ushort)toTensorId << 16)
            | (ushort)toIndexId;
    }
}
