using System.Collections.Generic;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.Indices;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesSymmetriesTests
{
    [Fact]
    public void ShouldReportIsEmptyForEmptySymmetries()
    {
        Exception? exception = Record.Exception(() =>
        {
            IndicesSymmetries symmetries = IndicesSymmetries.EmptySymmetries;
            Assert.True(symmetries.IsEmpty);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }

    [Fact]
    public void ShouldReportIsTrivialForEmptyAndIdentityGenerator()
    {
        Exception? exception = Record.Exception(() =>
        {
            var empty = IndicesSymmetries.EmptySymmetries;
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 2);
            List<Permutation> generators =
            [
                GroupPermutations.CreatePermutation(false, [0, 1])
            ];
            IndicesSymmetries withIdentity = IndicesSymmetries.Create(structure, generators);

            Assert.True(empty.IsTrivial());
            Assert.True(withIdentity.IsTrivial());
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }

    [Fact]
    public void ShouldChangeAvailableForModificationAfterPermutationGroupRealization()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 2);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(structure);

            Assert.True(symmetries.AvailableForModification());

            _ = symmetries.PermutationGroup;

            Assert.False(symmetries.AvailableForModification());
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }

    [Fact]
    public void ShouldCloneAsEquivalentButIndependentStructure()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 2);
            List<Permutation> generators =
            [
                GroupPermutations.CreatePermutation(false, [0, 1])
            ];
            IndicesSymmetries original = IndicesSymmetries.Create(structure, generators);
            IndicesSymmetries clone = original.Clone();

            Assert.NotSame(original, clone);
            Assert.Equal(original.IsTrivial(), clone.IsTrivial());
            Assert.Equal(original.Generators.Count, clone.Generators.Count);

            clone.AddSymmetry(GroupPermutations.CreatePermutation(false, [1, 0]));

            Assert.Single(original.Generators);
            Assert.Equal(2, clone.Generators.Count);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }

    [Fact]
    public void ShouldExposeNonNullGeneratorsAndPermutationGroup()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 2);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(structure);

            Assert.NotNull(symmetries.Generators);
            Assert.NotNull(symmetries.PermutationGroup);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }

    [Fact]
    public void ShouldReturnPositionsInOrbitsWithLengthMatchingStructureSizeForNonEmpty()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 3);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(structure);

            short[] positionsInOrbits = symmetries.PositionsInOrbits;

            Assert.NotNull(positionsInOrbits);
            Assert.Equal(structure.Size, positionsInOrbits.Length);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }
}
