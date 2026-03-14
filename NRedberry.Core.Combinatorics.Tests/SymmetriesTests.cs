using NRedberry.Core.Combinatorics.Symmetries;
using Shouldly;
using SymmetriesContract = NRedberry.Core.Combinatorics.Symmetries.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class AbstractSymmetriesTests
{
    [Fact]
    public void ShouldReturnCopyOfBasisSymmetries()
    {
        TestDummySymmetries symmetries = new(2, new Symmetry([0, 1], false));

        List<Symmetry> copy = symmetries.BasisSymmetries;
        copy.Clear();

        symmetries.BasisSymmetries.ShouldHaveSingleItem();
        ((SymmetriesContract)symmetries).Dimension.ShouldBe(2);
    }
}

public sealed class DummySymmetriesTests
{
    [Fact]
    public void ShouldAddUniqueSymmetriesAndCloneAsSelf()
    {
        TestDummySymmetries symmetries = new(2, new Symmetry([0, 1], false));
        Symmetry transposition = new([1, 0], false);

        symmetries.Add(transposition).ShouldBeTrue();
        symmetries.Add(transposition).ShouldBeFalse();
        symmetries.Clone().ShouldBeSameAs(symmetries);
    }
}

public sealed class EmptySymmetriesTests
{
    [Fact]
    public void ShouldOnlyAllowZeroOrOneDimensions()
    {
        EmptySymmetries zero = new(0);
        EmptySymmetries one = new(1);

        zero.IsEmpty.ShouldBeTrue();
        one.IsEmpty.ShouldBeTrue();
        Should.Throw<ArgumentException>(() => new EmptySymmetries(2));
    }
}

public sealed class FullSymmetriesTests
{
    [Fact]
    public void ShouldEnumerateAllDimensionTwoSymmetries()
    {
        FullSymmetries symmetries = new(2);

        List<int[]> visited = symmetries.Select(symmetry => symmetry.OneLine()).ToList();

        symmetries.IsEmpty.ShouldBeFalse();
        visited.Count.ShouldBe(2);
        visited.Any(permutation => permutation.SequenceEqual([0, 1])).ShouldBeTrue();
        visited.Any(permutation => permutation.SequenceEqual([1, 0])).ShouldBeTrue();
    }
}

public sealed class ISymmetriesTests
{
    [Fact]
    public void ShouldExposeSharedInterfaceMembers()
    {
        SymmetriesContract symmetries = new FullSymmetries(2);

        symmetries.Dimension.ShouldBe(2);
        symmetries.IsEmpty.ShouldBeFalse();
        symmetries.BasisSymmetries.ShouldNotBeEmpty();
    }
}

file sealed class TestDummySymmetries(int dimension, params List<Symmetry> basis)
    : DummySymmetries(dimension, basis)
{
    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return Basis.GetEnumerator();
    }
}
