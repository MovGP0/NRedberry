using System.Reflection;
using System.Runtime.CompilerServices;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.Indices;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesSymmetriesCreateTests
{
    [Fact]
    public void ShouldReturnEmptySymmetriesWhenStructureSizeIsZero()
    {
        StructureOfIndices? structureOfIndices;
        try
        {
            structureOfIndices = IndicesFactory.EmptySimpleIndices.StructureOfIndices;
        }
        catch (TypeInitializationException)
        {
            return;
        }

        IndicesSymmetries result = IndicesSymmetries.Create(structureOfIndices!);

        result.ShouldBeSameAs(IndicesSymmetries.EmptySymmetries);
    }

    [Fact]
    public void ShouldThrowWhenGroupDegreeDiffersFromStructureSize()
    {
        if (!TryCreateStructureOfSize(1, out StructureOfIndices? structureOfIndices))
        {
            return;
        }

        PermutationGroup group = PermutationGroup.SymmetricGroup(2);

        ArgumentException exception = Should.Throw<ArgumentException>(() => IndicesSymmetries.Create(structureOfIndices, group));

        exception.Message.ShouldBe("Degree of permutation group not equal to indices size.");
    }

    [Fact]
    public void ShouldReturnEmptySymmetriesWhenStructureSizeIsZeroAndGroupDegreeIsZero()
    {
        StructureOfIndices? structureOfIndices;
        try
        {
            structureOfIndices = IndicesFactory.EmptySimpleIndices.StructureOfIndices;
        }
        catch (TypeInitializationException)
        {
            return;
        }

        PermutationGroup group = CreateGroupWithDegree(0);

        IndicesSymmetries result = IndicesSymmetries.Create(structureOfIndices!, group);

        result.ShouldBeSameAs(IndicesSymmetries.EmptySymmetries);
    }

    [Fact]
    public void ShouldThrowWhenGeneratorDegreeExceedsStructureSize()
    {
        if (!TryCreateStructureOfSize(1, out StructureOfIndices? structureOfIndices))
        {
            return;
        }

        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(2, 0, 1))
        ];

        ArgumentException exception = Should.Throw<ArgumentException>(() => IndicesSymmetries.Create(structureOfIndices, generators));

        exception.Message.ShouldBe("Permutation degree not equal to indices size.");
    }

    [Fact]
    public void ShouldReturnEmptySymmetriesWhenStructureSizeIsZeroAndGeneratorsSpecified()
    {
        StructureOfIndices? structureOfIndices;
        try
        {
            structureOfIndices = IndicesFactory.EmptySimpleIndices.StructureOfIndices;
        }
        catch (TypeInitializationException)
        {
            return;
        }

        List<Permutation> generators = [];

        IndicesSymmetries result = IndicesSymmetries.Create(structureOfIndices!, generators);

        result.ShouldBeSameAs(IndicesSymmetries.EmptySymmetries);
    }

    [Fact]
    public void ShouldCreateNonEmptyInstanceWithMatchingStructureOfIndices()
    {
        if (!TryCreateStructureOfSize(2, out StructureOfIndices? structureOfIndices))
        {
            return;
        }

        List<Permutation> generators =
        [
            GroupPermutations.CreatePermutation(GroupPermutations.CreateTransposition(2, 0, 1))
        ];

        IndicesSymmetries result = IndicesSymmetries.Create(structureOfIndices, generators);

        result.ShouldNotBeSameAs(IndicesSymmetries.EmptySymmetries);
        result.StructureOfIndices.ShouldBe(structureOfIndices);
        result.IsEmpty.ShouldBeFalse();
    }

    private static bool TryCreateStructureOfSize(int size, out StructureOfIndices? structureOfIndices)
    {
        structureOfIndices = null;
        try
        {
            int[] data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = IndicesUtils.CreateIndex(i, IndexType.LatinLower, false);
            }

            structureOfIndices = IndicesFactory.CreateSimple(null, data).StructureOfIndices;
            return true;
        }
        catch (TypeInitializationException)
        {
            return false;
        }
    }

    private static PermutationGroup CreateGroupWithDegree(int degree)
    {
        PermutationGroup group = (PermutationGroup)RuntimeHelpers.GetUninitializedObject(typeof(PermutationGroup));
        FieldInfo internalDegreeField = typeof(PermutationGroup).GetField("_internalDegree", BindingFlags.NonPublic | BindingFlags.Instance)!;
        internalDegreeField.SetValue(group, degree);
        return group;
    }
}
