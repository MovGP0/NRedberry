using System.Numerics;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FrobeniusNumberTests
{
    public static TheoryData<int[], long> IntCases
        => new()
        {
            { [112, 432, 123, 7], 731L },
            { [112, 432, 123, 7, 3], 11L },
            { [112, 432, 122, 8, 31], 147L },
            { [46, 4967, 50, 9208, 6921], 6021L },
            { [5956, 388, 8234, 6312, 5379], 71845L }
        };

    [Theory]
    [MemberData(nameof(IntCases))]
    public void ShouldComputeKnownValuesForIntInput(int[] values, long expected)
    {
        BigInteger result = FrobeniusNumber.Calculate(values);
        Assert.Equal(new BigInteger(expected), result);
    }

    [Fact]
    public void ShouldReturnMinusOneWhenNoFiniteResult()
    {
        BigInteger result = FrobeniusNumber.Calculate(112, 432, 122, 8, 32);
        Assert.Equal(new BigInteger(-1), result);
    }

    [Fact]
    public void ShouldComputeValueForLongInput()
    {
        BigInteger result = FrobeniusNumber.Calculate(112L, 432L, 123L, 7L);
        Assert.Equal(new BigInteger(731), result);
    }

    [Fact]
    public void ShouldComputeValueForBigIntegerInput()
    {
        BigInteger result = FrobeniusNumber.Calculate(new BigInteger(112), new BigInteger(432), new BigInteger(123), new BigInteger(7));
        Assert.Equal(new BigInteger(731), result);
    }

    [Fact]
    public void ShouldComputeValueUsingBigIntegerSpecificAlgorithm()
    {
        BigInteger result = FrobeniusNumber.CalculateFromBigIntegerArray(new BigInteger(112), new BigInteger(432), new BigInteger(123), new BigInteger(7));
        Assert.Equal(new BigInteger(731), result);
    }

    [Fact]
    public void ShouldThrowForNullIntArray()
    {
        Assert.Throws<ArgumentNullException>(() => FrobeniusNumber.Calculate((int[])null!));
    }

    [Fact]
    public void ShouldThrowForNullLongArray()
    {
        Assert.Throws<ArgumentNullException>(() => FrobeniusNumber.Calculate((long[])null!));
    }

    [Fact]
    public void ShouldThrowForNullBigIntegerArray()
    {
        Assert.Throws<ArgumentNullException>(() => FrobeniusNumber.Calculate((BigInteger[])null!));
    }

    [Fact]
    public void ShouldThrowForNullBigIntegerArrayInSpecificAlgorithm()
    {
        Assert.Throws<ArgumentNullException>(() => FrobeniusNumber.CalculateFromBigIntegerArray((BigInteger[])null!));
    }
}
