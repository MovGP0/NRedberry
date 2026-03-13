using System.Numerics;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class SymmetryTests
{
    [Fact]
    public void ShouldExposeCycleParityAndOrderProperties()
    {
        Symmetry symmetry = new([1, 2, 0], false);

        Assert.Equal([[0, 1, 2]], symmetry.Cycles());
        Assert.Equal([3], symmetry.LengthsOfCycles);
        Assert.Equal(0, symmetry.Parity);
        Assert.Equal(new BigInteger(3), symmetry.Order);
        Assert.True(symmetry.OrderIsOdd);
        Assert.Equal("+{0, 1, 2}", symmetry.ToStringCycles());
    }

    [Fact]
    public void ShouldNegateConvertAndMoveRight()
    {
        Symmetry symmetry = new([1, 0], false);

        Assert.True(symmetry.Negate().IsAntisymmetry);
        Assert.Same(symmetry, symmetry.ToSymmetry());
        Assert.Equal([0, 2, 1], symmetry.MoveRight(1).OneLine());
        Assert.Equal([1, 0], symmetry.Pow(-1).OneLine());
    }
}
