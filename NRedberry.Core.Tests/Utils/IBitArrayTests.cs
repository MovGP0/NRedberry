using NRedberry.Contexts;
using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class IBitArrayTests
{
    [Fact]
    public void ShouldSupportBitArrayContractWithByteBackedImplementation()
    {
        AssertBitArrayContract(new ByteBackedBitArray(8), new ByteBackedBitArray(8));
    }

    [Fact]
    public void ShouldSupportBitArrayContractWithLongBackedImplementation()
    {
        AssertBitArrayContract(new LongBackedBitArray(8), new LongBackedBitArray(8));
    }

    private static void AssertBitArrayContract(IBitArray left, IBitArray right)
    {
        left.Set(1);
        left.Set(3);
        right.Set(3);
        right.Set(5);

        Assert.True(left.Intersects(right));

        IBitArray and = left.Clone();
        and.And(right);
        Assert.Equal(new[] { 3 }, and.GetBits());

        IBitArray or = left.Clone();
        or.Or(right);
        Assert.Equal(new[] { 1, 3, 5 }, or.GetBits());

        IBitArray xor = left.Clone();
        xor.Xor(right);
        Assert.Equal(new[] { 1, 5 }, xor.GetBits());

        xor.ClearAll();
        xor.SetAll();
        Assert.Equal(xor.Size, xor.BitCount());
        Assert.Equal(0, xor.NextTrailingBit(0));
    }
}
