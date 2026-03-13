using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class HashFunctionsTests
{
    [Fact]
    public void ShouldHashEquivalentIntValuesConsistentlyAcrossFvn32Overloads()
    {
        byte[] bytes = [0x12, 0x34, 0x56, 0x78];
        const int value = 0x12345678;

        int fromBytes = HashFunctions.FVN32hash(bytes);
        int fromInt = HashFunctions.FVN32hash(value);

        Assert.Equal(fromInt, fromBytes);
    }

    [Fact]
    public void ShouldHashEquivalentLongValuesConsistentlyAcrossFvn64Overloads()
    {
        byte[] bytes = [1, 2, 3, 4, 5, 6, 7, 8];
        const long value = 0x0102030405060708L;

        long fromBytes = HashFunctions.FVN64hash(bytes);
        long fromLong = HashFunctions.FVN64hash(value);

        Assert.Equal(fromLong, fromBytes);
    }

    [Fact]
    public void ShouldHashEquivalentIntValuesConsistentlyAcrossMurmurOverloads()
    {
        byte[] bytes = [0x78, 0x56, 0x34, 0x12];
        const int value = 0x12345678;
        const int seed = 0x13572468;

        int fromBytes = HashFunctions.MurmurHash2(bytes, seed);
        int fromInt = HashFunctions.MurmurHash2(value, seed);

        Assert.Equal(fromInt, fromBytes);
    }

    [Fact]
    public void ShouldProduceDifferentHashesForDifferentInputs()
    {
        Assert.NotEqual(HashFunctions.mix(1, 2, 3), HashFunctions.mix(1, 2, 4));
        Assert.NotEqual(HashFunctions.JenkinWang32shift(123), HashFunctions.JenkinWang32shift(124));
        Assert.NotEqual(HashFunctions.Wang32shiftmult(123L), HashFunctions.Wang32shiftmult(124L));
        Assert.NotEqual(HashFunctions.JenkinWang64shift(123L), HashFunctions.JenkinWang64shift(124L));
        Assert.NotEqual(HashFunctions.Wang64to32shift(123L), HashFunctions.Wang64to32shift(124L));
        Assert.NotEqual(HashFunctions.FVN64to32hash(123L), HashFunctions.FVN64to32hash(124L));
    }
}
