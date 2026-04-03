using System.Collections.Immutable;
using NRedberry.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesArraysUtilsTests
{
    [Fact]
    public void ArrayCopyShouldCopyEntireImmutableArray()
    {
        ImmutableArray<int> source = [1, 2, 3, 4];
        int[] destination = new int[source.Length];

        IndicesArraysUtils.ArrayCopy(source, 0, destination, 0, source.Length);

        destination.ShouldBe(source.ToArray());
    }

    [Fact]
    public void ArrayCopyShouldCopyRangeWithOffsetsAndLength()
    {
        ImmutableArray<int> source = [10, 20, 30, 40, 50];
        int[] destination = [-1, -1, -1, -1, -1];

        IndicesArraysUtils.ArrayCopy(source, 1, destination, 2, 2);

        destination.ShouldBe([-1, -1, 20, 30, -1]);
    }

    [Fact]
    public void ArrayCopyShouldDoNothingWhenLengthIsZero()
    {
        ImmutableArray<int> source = [7, 8, 9];
        int[] destination = [1, 2, 3];

        IndicesArraysUtils.ArrayCopy(source, 1, destination, 2, 0);

        destination.ShouldBe([1, 2, 3]);
    }

    [Fact]
    public void ArrayCopyShouldThrowWhenBoundsAreInvalid()
    {
        ImmutableArray<int> source = [1, 2, 3];
        int[] destination = new int[2];

        Should.Throw<ArgumentException>(() => IndicesArraysUtils.ArrayCopy(source, 1, destination, 0, 3));
    }
}
