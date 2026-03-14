using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArraysUtilsConversionsTests
{
    [Fact]
    public void ShouldConvertIntArrayToSignedByteArray()
    {
        sbyte[] actual = ArraysUtils.Int2byte([1, -2, 3]);

        actual.ShouldBe([(sbyte)1, (sbyte)(-2), (sbyte)3]);
    }

    [Fact]
    public void ShouldConvertIntArrayToShortArray()
    {
        short[] actual = ArraysUtils.Int2short([1, -2, 3]);

        actual.ShouldBe([(short)1, (short)(-2), (short)3]);
        ArraysUtils.IntToShort([1, -2, 3]).ShouldBe(actual);
    }

    [Fact]
    public void ShouldConvertShortArrayToIntArray()
    {
        int[] actual = ArraysUtils.ShortToInt([(short)1, (short)(-2), (short)3]);

        actual.ShouldBe([1, -2, 3]);
    }

    [Fact]
    public void ShouldConvertSignedByteArrayToIntAndShortArrays()
    {
        sbyte[] source = [(sbyte)1, (sbyte)(-2), (sbyte)3];

        ArraysUtils.ByteToInt(source).ShouldBe([1, -2, 3]);
        ArraysUtils.ByteToShort(source).ShouldBe([(short)1, (short)(-2), (short)3]);
    }

    [Fact]
    public void ShouldConvertIntArrayToUnsignedByteArray()
    {
        byte[] actual = ArraysUtils.IntToByte([1, 2, 255]);

        actual.ShouldBe([(byte)1, (byte)2, byte.MaxValue]);
    }
}
