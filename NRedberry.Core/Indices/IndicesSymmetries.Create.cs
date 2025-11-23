using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

namespace NRedberry.Indices;

public sealed partial class IndicesSymmetries
{
    /// <summary>
    /// Creates symmetries with empty generating set.
    /// </summary>
    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices)
    {
        if (structureOfIndices.Size == 0)
            return EmptySymmetries;
        return new IndicesSymmetries(structureOfIndices);
    }

    /// <summary>
    /// Creates symmetries with specified permutation group.
    /// </summary>
    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices, PermutationGroup group)
    {
        if (group.Degree != structureOfIndices.Size)
        {
            throw new ArgumentException("Degree of permutation group not equal to indices size.");
        }

        if (structureOfIndices.Size == 0)
        {
            return EmptySymmetries;
        }

        return new IndicesSymmetries(structureOfIndices, group.Generators.ToList(), group);
    }

    /// <summary>
    /// Creates symmetries with specified generating set.
    /// </summary>
    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices, List<Permutation> generators)
    {
        foreach (var p in generators)
        {
            if (p.Degree > structureOfIndices.Size)
            {
                throw new ArgumentException("Permutation degree not equal to indices size.");
            }
        }

        if (structureOfIndices.Size == 0)
        {
            return EmptySymmetries;
        }

        return new IndicesSymmetries(structureOfIndices, new List<Permutation>(generators), null);
    }
}
