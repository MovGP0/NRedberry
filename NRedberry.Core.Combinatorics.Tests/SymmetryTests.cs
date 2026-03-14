using System.Numerics;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class SymmetryTests
{
    [Fact]
    public void ShouldExposeCycleParityAndOrderProperties()
    {
        Symmetry symmetry = new([1, 2, 0], false);

        symmetry.Cycles().ShouldBe([[0, 1, 2]]);
        symmetry.LengthsOfCycles.ShouldBe([3]);
        symmetry.Parity.ShouldBe(0);
        symmetry.Order.ShouldBe(new BigInteger(3));
        symmetry.OrderIsOdd.ShouldBeTrue();
        symmetry.ToStringCycles().ShouldBe("+{0, 1, 2}");
    }

    [Fact]
    public void ShouldNegateConvertAndMoveRight()
    {
        Symmetry symmetry = new([1, 0], false);

        symmetry.Negate().IsAntisymmetry.ShouldBeTrue();
        symmetry.ToSymmetry().ShouldBeSameAs(symmetry);
        symmetry.MoveRight(1).OneLine().ShouldBe([0, 2, 1]);
        symmetry.Pow(-1).OneLine().ShouldBe([1, 0]);
    }
}
