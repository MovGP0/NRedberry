using NRedberry.Contexts;
using NRedberry.Core.Utils;
using Shouldly;
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

        left.Intersects(right).ShouldBeTrue();

        IBitArray and = left.Clone();
        and.And(right);
        and.GetBits().ShouldBe(new[] { 3 });

        IBitArray or = left.Clone();
        or.Or(right);
        or.GetBits().ShouldBe(new[] { 1, 3, 5 });

        IBitArray xor = left.Clone();
        xor.Xor(right);
        xor.GetBits().ShouldBe(new[] { 1, 5 });

        xor.ClearAll();
        xor.SetAll();
        xor.BitCount().ShouldBe(xor.Size);
        xor.NextTrailingBit(0).ShouldBe(0);
    }
}
