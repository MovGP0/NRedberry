using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Groups;

namespace NRedberry.Core.Indices;

/// <summary>
/// Representation of permutational symmetries of indices of simple tensors.
/// </summary>
public class IndicesSymmetries //: IEnumerable<Symmetry>
{
    private readonly StructureOfIndices structureOfIndices;
    private readonly List<Permutation> generators;
    private PermutationGroup permutationGroup;
    private short[] positionsInOrbits;

    private IndicesSymmetries(StructureOfIndices structureOfIndices)
    {
        this.structureOfIndices = structureOfIndices;
        this.generators = new List<Permutation>();
    }

    private IndicesSymmetries(StructureOfIndices structureOfIndices, List<Permutation> generators, PermutationGroup permutationGroup)
    {
        this.structureOfIndices = structureOfIndices;
        this.generators = generators;
        this.permutationGroup = permutationGroup;
    }

    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices)
    {
        return structureOfIndices.Size() == 0 ? Empty : new IndicesSymmetries(structureOfIndices);
    }

    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices, PermutationGroup group)
    {
        if (group.Degree() != structureOfIndices.Size())
            throw new ArgumentException("Degree of permutation group not equal to indices size.");
        return structureOfIndices.Size() == 0 ? Empty : new IndicesSymmetries(structureOfIndices, group.Generators().ToList(), group);
    }

    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices, List<Permutation> generators)
    {
        foreach (var p in generators)
        {
            if (p.Degree() > structureOfIndices.Size())
                throw new ArgumentException("Permutation degree not equal to indices size.");
        }
        return structureOfIndices.Size() == 0 ? Empty : new IndicesSymmetries(structureOfIndices, new List<Permutation>(generators), null);
    }

    public StructureOfIndices StructureOfIndices
    {
        get => structureOfIndices;
    }

    public List<Permutation> Generators => new(generators);

    public PermutationGroup GetPermutationGroup()
    {
        if (permutationGroup == null)
        {
            permutationGroup = !generators.Any() ? PermutationGroup.TrivialGroup() : PermutationGroup.CreatePermutationGroup(generators);
        }
        return permutationGroup;
    }

    public short[] GetPositionsInOrbits()
    {
        if (positionsInOrbits == null)
        {
            var positionsInOrbitsInt = GetPermutationGroup().GetPositionsInOrbits();
            positionsInOrbits = new short[structureOfIndices.Size];
            var i = 0;
            for (; i < positionsInOrbitsInt.Length; ++i)
            {
                positionsInOrbits[i] = (short)positionsInOrbitsInt[i];
            }
            for (; i < structureOfIndices.Size; ++i)
            {
                positionsInOrbits[i] = (short)i;
            }
        }
        return positionsInOrbits;
    }

    public bool IsTrivial()
    {
        return generators.All(p => p.IsIdentity());
    }

    public bool AvailableForModification()
    {
        return permutationGroup == null;
    }

    public void AddSymmetry(params int[] permutation)
    {
        Add(false, permutation);
    }

    public void AddAntiSymmetry(params int[] permutation)
    {
        Add(true, permutation);
    }

    public void Add(bool sign, int[] permutation)
    {
        byte type = -1;
        StructureOfIndices.TypeData typeData;
        for (int i = 0; i < IndexType.TypesCount; ++i)
        {
            typeData = structureOfIndices.GetTypeData((byte)i);
            if (typeData.Length != 0)
            {
                if (type != -1)
                    throw new ArgumentException();
                if (typeData.Length != permutation.Length)
                    throw new ArgumentException();
                type = (byte)i;
            }
        }
        AddSymmetry(type, Permutations.CreatePermutation(sign, permutation));
    }

    public void AddSymmetry(IndexType type, params int[] permutation)
    {
        Add(type, false, permutation);
    }

    public void AddAntiSymmetry(IndexType type, params int[] permutation)
    {
        Add(type, true, permutation);
    }

    public void Add(IndexType type, bool sign, params int[] permutation)
    {
        AddSymmetry(type.GetType(), Permutations.CreatePermutation(sign, permutation));
    }

    public void Add(byte type, bool sign, params int[] permutation)
    {
        AddSymmetry(type, Permutations.CreatePermutation(sign, permutation));
    }

    public void AddSymmetry(byte type, Permutation symmetry)
    {
        if (symmetry.IsIdentity())
            return;
        if (permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");

        StructureOfIndices.TypeData data = structureOfIndices.GetTypeData(type);
        if (data == null)
            throw new ArgumentException($"No such type: {IndexTypeMethods.GetType_(type)}");
        if (data.Length < symmetry.Degree())
            throw new ArgumentException("Wrong symmetry length.");

        long[] s = new long[structureOfIndices.Size];
        long i = 0;
        for (; i < data.From; ++i)
        {
            s[i] = i;
        }
        for (int j = 0; j < data.Length; ++j, ++i)
        {
            s[i] = symmetry.NewIndexOf(j) + data.From;
        }
        for (; i < structureOfIndices.Size; ++i)
        {
            s[i] = i;
        }
        generators.Add(Permutations.CreatePermutation(symmetry.Antisymmetry(), s));
    }

    public void AddSymmetry(Permutation symmetry)
    {
        if (permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        generators.Add(symmetry);
    }

    public void AddSymmetries(params Permutation[] symmetries)
    {
        if (permutationGroup != null) throw new InvalidOperationException("Permutation group is already in use.");

        foreach (var symmetry in symmetries)
        {
            generators.Add(symmetry);
        }
    }

    public void AddSymmetries(IEnumerable<Permutation> symmetries)
    {
        if (permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        foreach (var symmetry in symmetries)
            generators.Add(symmetry);
    }

    public void AddAll(IEnumerable<Permutation> symmetry)
    {
        if (permutationGroup != null)
            throw new ArgumentException();
        generators.AddRange(symmetry);
    }

    public void SetSymmetric()
    {
        if (permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        PermutationGroup sym = null;
        int[] counts = structureOfIndices.GetTypesCounts();
        foreach (int c in counts)
        {
            if (c != 0)
            {
                if (sym == null)
                    sym = PermutationGroup.SymmetricGroup(c);
                else
                    sym = sym.DirectProduct(PermutationGroup.SymmetricGroup(c));
            }
        }

        permutationGroup = sym;
        generators.Clear();
        generators.AddRange(sym.Generators());
    }

    public void SetAntiSymmetric()
    {
        if (permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        PermutationGroup sym = null;
        int[] counts = structureOfIndices.GetTypesCounts();
        foreach (int c in counts)
        {
            if (c != 0)
            {
                if (sym == null)
                    sym = PermutationGroup.AntisymmetricGroup(c);
                else
                    sym = sym.DirectProduct(PermutationGroup.AntisymmetricGroup(c));
            }
        }

        permutationGroup = sym;
        generators.Clear();
        generators.AddRange(sym.Generators());
    }

    public IndicesSymmetries Clone()
    {
        if (structureOfIndices.Size == 0)
            return Empty;
        return new IndicesSymmetries(structureOfIndices, new List<Permutation>(generators), permutationGroup);
    }

    private static IndicesSymmetries emptyIndicesSymmetries = new(StructureOfIndices.Empty, new List<Permutation>(), null);
    public static IndicesSymmetries Empty => emptyIndicesSymmetries;

    public override string ToString()
    {
        return generators.ToString();
    }
}