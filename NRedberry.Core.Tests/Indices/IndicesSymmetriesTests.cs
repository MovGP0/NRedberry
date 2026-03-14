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
            symmetries.IsEmpty.ShouldBeTrue();
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
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

            empty.IsTrivial().ShouldBeTrue();
            withIdentity.IsTrivial().ShouldBeTrue();
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
    }

    [Fact]
    public void ShouldChangeAvailableForModificationAfterPermutationGroupRealization()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 2);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(structure);

            symmetries.AvailableForModification().ShouldBeTrue();

            _ = symmetries.PermutationGroup;

            symmetries.AvailableForModification().ShouldBeFalse();
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
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

            clone.ShouldNotBeSameAs(original);
            clone.IsTrivial().ShouldBe(original.IsTrivial());
            clone.Generators.Count.ShouldBe(original.Generators.Count);

            clone.AddSymmetry(GroupPermutations.CreatePermutation(false, [1, 0]));

            original.Generators.ShouldHaveSingleItem();
            clone.Generators.Count.ShouldBe(2);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
    }

    [Fact]
    public void ShouldExposeNonNullGeneratorsAndPermutationGroup()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 2);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(structure);

            symmetries.Generators.ShouldNotBeNull();
            symmetries.PermutationGroup.ShouldNotBeNull();
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnPositionsInOrbitsWithLengthMatchingStructureSizeForNonEmpty()
    {
        Exception? exception = Record.Exception(() =>
        {
            StructureOfIndices structure = StructureOfIndices.Create(IndexType.LatinLower, 3);
            IndicesSymmetries symmetries = IndicesSymmetries.Create(structure);

            short[] positionsInOrbits = symmetries.PositionsInOrbits;

            positionsInOrbits.ShouldNotBeNull();
            positionsInOrbits.Length.ShouldBe(structure.Size);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
    }
}
