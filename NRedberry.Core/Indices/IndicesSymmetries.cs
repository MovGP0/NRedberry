using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Combinatorics.Symmetries;
using NRedberry.Groups;

namespace NRedberry.Indices;

/// <summary>
/// Wrapper of <see cref="PermutationGroup"/> that holds symmetries of indices.
/// Mirrors cc.redberry.core.indices.IndicesSymmetries.
/// </summary>
public sealed partial class IndicesSymmetries : IEnumerable<Symmetry>
{
    public StructureOfIndices StructureOfIndices { get; }

    private readonly Symmetries _symmetries;
    private readonly List<Permutation> _generators;
    private PermutationGroup? _permutationGroup;
    private short[]? _diffIds;

    private IndicesSymmetries(StructureOfIndices structureOfIndices)
    {
        StructureOfIndices = structureOfIndices;
        _symmetries = SymmetriesFactory.CreateSymmetries(structureOfIndices.Size);
        _generators = [];
    }

    private IndicesSymmetries(StructureOfIndices structureOfIndices, List<Permutation> generators, PermutationGroup? permutationGroup)
    {
        StructureOfIndices = structureOfIndices;
        _symmetries = SymmetriesFactory.CreateSymmetries(structureOfIndices.Size);
        _generators = generators;
        _permutationGroup = permutationGroup;
        foreach (var gen in generators)
        {
            _symmetries.AddUnsafe(new Symmetry(gen.Cast<int>().ToArray(), gen.IsAntisymmetry));
        }
    }

    public StructureOfIndices GetStructureOfIndices() => StructureOfIndices;

    public IReadOnlyList<Permutation> Generators => _generators.ToImmutableList();

    public PermutationGroup PermutationGroup
    {
        get
        {
            if (_permutationGroup == null)
            {
                _permutationGroup = _generators.Count == 0
                    ? PermutationGroup.TrivialGroup()
                    : PermutationGroup.CreatePermutationGroup(_generators);
            }

            return _permutationGroup;
        }
    }

    public short[]? PositionsInOrbits
    {
        [return: NotNull]
        get
        {
            if (field == null)
            {
                int[] ints = PermutationGroup.PositionsInOrbits;
                field = new short[StructureOfIndices.Size];
                int i = 0;
                for (; i < ints.Length; ++i)
                {
                    field[i] = (short)ints[i];
                }

                for (; i < StructureOfIndices.Size; ++i)
                {
                    field[i] = (short)i;
                }
            }

            return field;
        }
        private set;
    }

    public bool IsTrivial()
    {
        foreach (var p in _generators)
        {
            if (!p.IsIdentity)
            {
                return false;
            }
        }

        return true;
    }

    public bool AvailableForModification() => _permutationGroup == null;

    public bool AddSymmetry(params int[] permutation) => Add(false, permutation);

    public bool AddAntiSymmetry(params int[] permutation) => Add(true, permutation);

    public bool Add(IndexType type, bool sign, params int[] permutation) => Add((byte)type, sign, permutation);

    public bool Add(byte type, bool sign, params int[] permutation) => Add(type, new Symmetry(permutation, sign));

    public bool Add(bool sign, params int[] permutation)
    {
        byte type = 255;
        TypeData typeData;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            typeData = StructureOfIndices.GetTypeData((byte)i);
            if (typeData.Length != 0)
            {
                if (type != 255)
                    throw new ArgumentException();
                if (typeData.Length != permutation.Length)
                    throw new ArgumentException();
                type = (byte)i;
            }
        }

        return Add(type, new Symmetry(permutation, sign));
    }

    public bool AddSymmetry(IndexType type, params int[] permutation) => Add(type, false, permutation);

    public bool AddAntiSymmetry(IndexType type, params int[] permutation) => Add(type, true, permutation);

    public void AddSymmetry(Permutation symmetry)
    {
        if (_permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        if (symmetry.Degree > StructureOfIndices.Size)
            throw new ArgumentException("Permutation degree not equal to indices size.");
        _generators.Add(symmetry);
        _symmetries.AddUnsafe(new Symmetry(symmetry.Cast<int>().ToArray(), symmetry.IsAntisymmetry));
        _diffIds = null;
    }

    public void AddSymmetries(params Permutation[] symmetries)
    {
        if (_permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        foreach (var s in symmetries)
            AddSymmetry(s);
    }

    public void AddSymmetries(IEnumerable<Permutation> symmetries)
    {
        if (_permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        foreach (var s in symmetries)
            AddSymmetry(s);
    }

    public void AddAll(IEnumerable<Permutation> symmetries)
    {
        if (_permutationGroup != null)
            throw new InvalidOperationException();
        _generators.AddRange(symmetries);
        foreach (var s in symmetries)
            _symmetries.AddUnsafe(new Symmetry(s.Cast<int>().ToArray(), s.IsAntisymmetry));
        _diffIds = null;
    }

    public bool Add(byte type, Symmetry symmetry)
    {
        var data = StructureOfIndices.GetTypeData(type);
        if (data == null)
            throw new ArgumentException("No such type: " + IndexTypeMethods.GetType(type));
        if (data.Length < symmetry.Length)
            throw new ArgumentException("Wrong symmetry length.");

        int[] s = new int[StructureOfIndices.Size];
        int i = 0;
        for (; i < data.From; ++i)
            s[i] = i;
        for (int j = 0; j < data.Length; ++j, ++i)
            s[i] = symmetry.NewIndexOf(j) + data.From;
        for (; i < StructureOfIndices.Size; ++i)
            s[i] = i;

        if (_permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");

        try
        {
            var fullSym = new Symmetry(s, symmetry.IsAntisymmetry);
            if (_symmetries.Add(fullSym))
            {
                _generators.Add(Permutations.CreatePermutation(symmetry.IsAntisymmetry, s));
                _permutationGroup = null;
                _diffIds = null;
                PositionsInOrbits = null;
                return true;
            }

            return false;
        }
        catch (InconsistentGeneratorsException)
        {
            throw new InconsistentGeneratorsException("Adding inconsistent symmetry to tensor indices symmetries.");
        }
    }

    public bool AddUnsafe(byte type, Symmetry symmetry)
    {
        var data = StructureOfIndices.GetTypeData(type);
        if (data == null)
            throw new ArgumentException("No such type: " + IndexTypeMethods.GetType(type));
        if (data.Length < symmetry.Length)
            throw new ArgumentException("Wrong symmetry length.");

        int[] s = new int[StructureOfIndices.Size];
        int i = 0;
        for (; i < data.From; ++i)
            s[i] = i;
        for (int j = 0; j < data.Length; ++j, ++i)
            s[i] = symmetry.NewIndexOf(j) + data.From;
        for (; i < StructureOfIndices.Size; ++i)
            s[i] = i;

        _symmetries.AddUnsafe(new Symmetry(s, symmetry.IsAntisymmetry));
        _generators.Add(Permutations.CreatePermutation(symmetry.IsAntisymmetry, s));
        _permutationGroup = null;
        _diffIds = null;
        PositionsInOrbits = null;
        return true;
    }

    public bool AddUnsafe(Symmetry symmetry) => _symmetries.AddUnsafe(symmetry);

    public void SetSymmetric()
    {
        if (_permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        PermutationGroup? sym = null;
        int[] counts = StructureOfIndices.TypesCounts;
        foreach (int c in counts)
        {
            if (c == 0)
                continue;
            sym = sym == null
                ? PermutationGroup.SymmetricGroup(c)
                : sym.DirectProduct(PermutationGroup.SymmetricGroup(c));
        }

        _permutationGroup = sym ?? PermutationGroup.TrivialGroup();
        _generators.Clear();
        _generators.AddRange(_permutationGroup.Generators);
        _symmetries.AddUnsafe(new Symmetry(_permutationGroup.Generators[0].Cast<int>().ToArray(), _permutationGroup.Generators[0].IsAntisymmetry));
        _diffIds = null;
        PositionsInOrbits = null;
    }

    public void SetAntiSymmetric()
    {
        if (_permutationGroup != null)
            throw new InvalidOperationException("Permutation group is already in use.");
        PermutationGroup? sym = null;
        int[] counts = StructureOfIndices.TypesCounts;
        foreach (int c in counts)
        {
            if (c == 0)
                continue;
            sym = sym == null
                ? PermutationGroup.AntisymmetricGroup(c)
                : sym.DirectProduct(PermutationGroup.AntisymmetricGroup(c));
        }

        _permutationGroup = sym ?? PermutationGroup.TrivialGroup();
        _generators.Clear();
        _generators.AddRange(_permutationGroup.Generators);
        _symmetries.AddUnsafe(new Symmetry(_permutationGroup.Generators[0].Cast<int>().ToArray(), _permutationGroup.Generators[0].IsAntisymmetry));
        _diffIds = null;
        PositionsInOrbits = null;
    }

    public bool IsEmpty => _symmetries.IsEmpty;

    public IndicesSymmetries Clone()
    {
        if (StructureOfIndices.Size == 0)
            return EmptySymmetries;
        return new IndicesSymmetries(StructureOfIndices, new List<Permutation>(_generators), _permutationGroup);
    }

    public short[] DiffIds
    {
        get
        {
            if (_diffIds == null)
            {
                List<Symmetry> list = _symmetries.BasisSymmetries;
                _diffIds = new short[_symmetries.Dimension];
                Array.Fill(_diffIds, (short)-1);
                short number = 0;
                List<int> removed = new List<int>(2);
                int i0;
                int i1;
                foreach (var symmetry in list)
                {
                    for (i0 = _diffIds.Length - 1; i0 >= 0; --i0)
                    {
                        if ((i1 = symmetry.NewIndexOf(i0)) != i0)
                        {
                            if (_diffIds[i0] == -1 && _diffIds[i1] == -1)
                            {
                                var newNumber = number++;
                                _diffIds[i0] = newNumber;
                                _diffIds[i1] = newNumber;
                            }
                            else if (_diffIds[i0] == -1)
                            {
                                _diffIds[i0] = _diffIds[i1];
                            }
                            else if (_diffIds[i1] == -1)
                            {
                                _diffIds[i1] = _diffIds[i0];
                            }
                            else if (_diffIds[i1] != _diffIds[i0])
                            {
                                int n = _diffIds[i1];
                                for (int k = 0; k < _diffIds.Length; ++k)
                                {
                                    if (_diffIds[k] == n)
                                    {
                                        _diffIds[k] = _diffIds[i0];
                                    }
                                }

                                removed.Add(n);
                            }
                        }
                    }
                }

                for (i1 = 0; i1 < _diffIds.Length; ++i1)
                {
                    if (_diffIds[i1] == -1)
                    {
                        _diffIds[i1] = number++;
                    }
                }

                removed.Sort();
                for (i0 = _diffIds.Length - 1; i0 >= 0; --i0)
                {
                    _diffIds[i0] += (short)(Array.BinarySearch(removed.ToArray(), _diffIds[i0]) + 1);
                }
            }

            return _diffIds;
        }
    }

    public static readonly IndicesSymmetries EmptySymmetries = new(
        new StructureOfIndices(EmptySimpleIndices.emptySimpleIndicesInstance),
        new List<Permutation>(),
        null);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var symmetry in _symmetries)
        {
            hash.Add(symmetry);
        }

        return hash.ToHashCode();
    }

    public List<Symmetry> Basis => _symmetries.BasisSymmetries;

    public IEnumerator<Symmetry> GetEnumerator() => _symmetries.GetEnumerator();
    public override string ToString() => _symmetries.ToString();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
