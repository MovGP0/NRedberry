using System;
using System.Collections;
using System.Reflection;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsNextZeroBitTests
{
    private static readonly MethodInfo s_nextZeroBitMethod = GetMethod([typeof(BitArray)]);

    private static readonly MethodInfo s_nextZeroBitFromIndexMethod = GetMethod([typeof(BitArray), typeof(int)]);

    [Fact]
    public void ShouldReturnFirstZeroBitFromStart()
    {
        BitArray bitArray = new([true, true, false, true, false]);

        int index = InvokeNextZeroBit(bitArray);

        Assert.Equal(2, index);
    }

    [Fact]
    public void ShouldReturnMinusOneWhenNoZeroBitsExist()
    {
        BitArray bitArray = new([true, true, true]);

        int index = InvokeNextZeroBit(bitArray);

        Assert.Equal(-1, index);
    }

    [Fact]
    public void ShouldReturnMinusOneForEmptyBitArray()
    {
        BitArray bitArray = new(0);

        int index = InvokeNextZeroBit(bitArray);

        Assert.Equal(-1, index);
    }

    [Fact]
    public void ShouldReturnFirstZeroBitFromProvidedStartIndex()
    {
        BitArray bitArray = new([false, true, false, false, true]);

        int index = InvokeNextZeroBit(bitArray, 2);

        Assert.Equal(2, index);
    }

    [Fact]
    public void ShouldReturnMinusOneWhenStartIndexIsAtLength()
    {
        BitArray bitArray = new([false, true, false]);

        int index = InvokeNextZeroBit(bitArray, bitArray.Length);

        Assert.Equal(-1, index);
    }

    [Fact]
    public void ShouldReturnMinusOneWhenNoZeroBitsAfterStartIndex()
    {
        BitArray bitArray = new([false, false, true, true, true]);

        int index = InvokeNextZeroBit(bitArray, 2);

        Assert.Equal(-1, index);
    }

    [Fact]
    public void ShouldThrowWhenStartIndexIsNegative()
    {
        BitArray bitArray = new([false, true, false]);

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => _ = InvokeNextZeroBit(bitArray, -1));

        Assert.IsType<ArgumentOutOfRangeException>(exception.InnerException);
    }

    private static int InvokeNextZeroBit(BitArray bitArray)
    {
        return (int)s_nextZeroBitMethod.Invoke(null, [bitArray])!;
    }

    private static int InvokeNextZeroBit(BitArray bitArray, int startIndex)
    {
        return (int)s_nextZeroBitFromIndexMethod.Invoke(null, [bitArray, startIndex])!;
    }

    private static MethodInfo GetMethod(Type[] parameterTypes)
    {
        BindingFlags flags = BindingFlags.NonPublic;
        flags |= BindingFlags.Static;

        MethodInfo? method = typeof(GroupPermutations).GetMethod(
            "NextZeroBit",
            flags,
            null,
            parameterTypes,
            null);

        if (method is null)
        {
            throw new InvalidOperationException("Could not find NextZeroBit overload.");
        }

        return method;
    }
}
