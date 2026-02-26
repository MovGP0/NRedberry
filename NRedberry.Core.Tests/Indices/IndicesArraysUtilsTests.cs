using System;
using System.Collections.Immutable;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesArraysUtilsTests
{
    [Fact]
    public void ArrayCopyShouldCopyEntireImmutableArray()
    {
        ImmutableArray<int> source = ImmutableArray.Create(1, 2, 3, 4);
        int[] destination = new int[source.Length];

        IndicesArraysUtils.ArrayCopy(source, 0, destination, 0, source.Length);

        Assert.Equal(source.ToArray(), destination);
    }

    [Fact]
    public void ArrayCopyShouldCopyRangeWithOffsetsAndLength()
    {
        ImmutableArray<int> source = ImmutableArray.Create(10, 20, 30, 40, 50);
        int[] destination = { -1, -1, -1, -1, -1 };

        IndicesArraysUtils.ArrayCopy(source, 1, destination, 2, 2);

        Assert.Equal([-1, -1, 20, 30, -1], destination);
    }

    [Fact]
    public void ArrayCopyShouldDoNothingWhenLengthIsZero()
    {
        ImmutableArray<int> source = ImmutableArray.Create(7, 8, 9);
        int[] destination = { 1, 2, 3 };

        IndicesArraysUtils.ArrayCopy(source, 1, destination, 2, 0);

        Assert.Equal([1, 2, 3], destination);
    }

    [Fact]
    public void ArrayCopyShouldThrowWhenBoundsAreInvalid()
    {
        ImmutableArray<int> source = ImmutableArray.Create(1, 2, 3);
        int[] destination = new int[2];

        Assert.Throws<ArgumentException>(() => IndicesArraysUtils.ArrayCopy(source, 1, destination, 0, 3));
    }
}
