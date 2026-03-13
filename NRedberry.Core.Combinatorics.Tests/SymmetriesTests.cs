using NRedberry.Core.Combinatorics.Symmetries;
using SymmetriesContract = NRedberry.Core.Combinatorics.Symmetries.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class AbstractSymmetriesTests
{
    [Fact]
    public void ShouldReturnCopyOfBasisSymmetries()
    {
        TestDummySymmetries symmetries = new(2, [new Symmetry([0, 1], false)]);

        List<Symmetry> copy = symmetries.BasisSymmetries;
        copy.Clear();

        Assert.Single(symmetries.BasisSymmetries);
        Assert.Equal(2, ((SymmetriesContract)symmetries).Dimension);
    }
}

public sealed class DummySymmetriesTests
{
    [Fact]
    public void ShouldAddUniqueSymmetriesAndCloneAsSelf()
    {
        TestDummySymmetries symmetries = new(2, [new Symmetry([0, 1], false)]);
        Symmetry transposition = new([1, 0], false);

        Assert.True(symmetries.Add(transposition));
        Assert.False(symmetries.Add(transposition));
        Assert.Same(symmetries, symmetries.Clone());
    }
}

public sealed class EmptySymmetriesTests
{
    [Fact]
    public void ShouldOnlyAllowZeroOrOneDimensions()
    {
        EmptySymmetries zero = new(0);
        EmptySymmetries one = new(1);

        Assert.True(zero.IsEmpty);
        Assert.True(one.IsEmpty);
        Assert.Throws<ArgumentException>(() => new EmptySymmetries(2));
    }
}

public sealed class FullSymmetriesTests
{
    [Fact]
    public void ShouldEnumerateAllDimensionTwoSymmetries()
    {
        FullSymmetries symmetries = new(2);

        List<int[]> visited = symmetries.Select(symmetry => symmetry.OneLine()).ToList();

        Assert.False(symmetries.IsEmpty);
        Assert.Equal(2, visited.Count);
        Assert.Contains(visited, permutation => permutation.SequenceEqual([0, 1]));
        Assert.Contains(visited, permutation => permutation.SequenceEqual([1, 0]));
    }
}

public sealed class ISymmetriesTests
{
    [Fact]
    public void ShouldExposeSharedInterfaceMembers()
    {
        SymmetriesContract symmetries = new FullSymmetries(2);

        Assert.Equal(2, symmetries.Dimension);
        Assert.False(symmetries.IsEmpty);
        Assert.NotEmpty(symmetries.BasisSymmetries);
    }
}

file sealed class TestDummySymmetries(int dimension, List<Symmetry> basis)
    : DummySymmetries(dimension, basis)
{
    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return Basis.GetEnumerator();
    }
}
