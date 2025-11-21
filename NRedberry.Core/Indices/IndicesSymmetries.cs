using System.Collections;
using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Combinatorics.Symmetries;

namespace NRedberry.Indices;

/// <summary>
/// Representation of permutational symmetries of indices of simple tensors.
/// </summary>
public class IndicesSymmetries : IEnumerable<Symmetry>
{
    public StructureOfIndices StructureOfIndices { get; }
    private Symmetries Symmetries { get; }

    private IList<Permutation> _generators;

    public IList<Permutation> Generators => _generators.ToImmutableList();

    public IList<Permutation> GetGenerators() => _generators.ToImmutableList();

    [Obsolete("Use Symmetries property instead", true)]
    public Symmetries GetInnerSymmetries() => Symmetries;

    private short[]? _diffIds;

    public IndicesSymmetries(StructureOfIndices structureOfIndices)
    {
        StructureOfIndices = structureOfIndices;
        Symmetries = SymmetriesFactory.CreateSymmetries(structureOfIndices.Size);
    }

    private IndicesSymmetries(StructureOfIndices structureOfIndices, Symmetries symmetries, short[] diffIds)
    {
        StructureOfIndices = structureOfIndices;
        Symmetries = symmetries;
        _diffIds = diffIds;
    }

    public IndicesSymmetries(StructureOfIndices structureOfIndices, Symmetries symmetries)
    {
        StructureOfIndices = structureOfIndices;
        Symmetries = symmetries;
    }

    /// <summary>
    /// Gets the diffIds for indices as specified in Indices.getDiffIds().
    /// </summary>
    /// <returns>diffIds for indices.</returns>
    public short[] DiffIds
    {
        get
        {
            //TODO synchronize
            if (_diffIds == null)
            {
                List<Symmetry> list = Symmetries.BasisSymmetries;
                _diffIds = new short[Symmetries.Dimension];
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

    /// <summary>
    /// Adds permutational symmetry.
    /// </summary>
    /// <param name="permutation">Permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If there are more than one type of indices in corresponding indices.</exception>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size().</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool AddSymmetry(params int[] permutation) => Add(false, permutation);

    public bool AddSymmetry(Permutation permutation) => throw new NotImplementedException();

    /// <summary>
    /// Adds permutational antisymmetry.
    /// </summary>
    /// <param name="permutation">Permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If there are more than one type of indices in corresponding indices.</exception>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size().</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool AddAntiSymmetry(params int[] permutation) => Add(true, permutation);

    /// <summary>
    /// Adds permutational symmetry for a particular type of indices.
    /// </summary>
    /// <param name="type">Type of indices</param>
    /// <param name="permutation">Permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size(type).</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool AddSymmetry(IndexType type, params int[] permutation) => Add(type, false, permutation);

    /// <summary>
    /// Adds permutational antisymmetry for a particular type of indices.
    /// </summary>
    /// <param name="type">Type of indices</param>
    /// <param name="permutation">Permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size(type).</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool AddAntiSymmetry(IndexType type, params int[] permutation) => Add(type, true, permutation);

    /// <summary>
    /// Adds permutational (anti)symmetry for a particular type of indices.
    /// </summary>
    /// <param name="type">Type of indices</param>
    /// <param name="sign">Sign of symmetry (true means '-', false means '+')</param>
    /// <param name="permutation">Permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size(type).</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool Add(IndexType type, bool sign, params int[] permutation) => Add((byte)type, new Symmetry(permutation, sign));

    /// <summary>
    /// Adds permutational (anti)symmetry for a particular type of indices.
    /// </summary>
    /// <param name="type">Type of indices</param>
    /// <param name="sign">Sign of symmetry (true means '-', false means '+')</param>
    /// <param name="permutation">Permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size(type).</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool Add(byte type, bool sign, params int[] permutation) => Add(type, new Symmetry(permutation, sign));

    /// <summary>
    /// Adds permutational (anti)symmetry.
    /// </summary>
    /// <param name="sign">sign of symmetry (true means '-', false means '+')</param>
    /// <param name="permutation">permutation</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from
    /// already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If there are more than one type of indices in corresponding indices.</exception>
    /// <exception cref="ArgumentException">If permutation.length() != indices.size(type).</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool Add(bool sign, params int[] permutation)
    {
        byte type = 255;
        TypeData typeData;
        for (int i = 0; i < IndexTypeMethods.TypesCount; ++i)
        {
            typeData = StructureOfIndices.GetTypeData((byte) i);
            if (typeData.Length != 0)
            {
                if (type != 255)
                    throw new ArgumentException();
                if (typeData.Length != permutation.Length)
                    throw new ArgumentException();
                type = (byte) i;
            }
        }

        return Add(type, new Symmetry(permutation, sign));
    }

    /// <summary>
    /// Adds permutational (anti)symmetry.
    /// </summary>
    /// <param name="type">Type of the symmetry.</param>
    /// <param name="symmetry">Symmetry object.</param>
    /// <returns>true if it is a new symmetry of indices and false if it follows from already defined symmetries.</returns>
    /// <exception cref="ArgumentException">If symmetry.dimension() != indices.size(type).</exception>
    /// <exception cref="InconsistentGeneratorsException">If the specified symmetry is inconsistent with already defined.</exception>
    public bool Add(byte type, Symmetry symmetry)
    {
        var data = StructureOfIndices.GetTypeData(type);
        if (data == null)
        {
            throw new ArgumentException("No such type: " + IndexTypeMethods.GetType(type));
        }

        if (data.Length != symmetry.Length)
        {
            throw new ArgumentException("Wrong symmetry length.");
        }

        int[] s = new int[StructureOfIndices.Size];
        int i = 0;
        for (; i < data.From; ++i)
            s[i] = i;
        for (int j = 0; j < data.Length; ++j, ++i)
            s[i] = symmetry.NewIndexOf(j) + data.From;
        for (; i < StructureOfIndices.Size; ++i)
            s[i] = i;
        try
        {
            if (Symmetries.Add(new Symmetry(s, symmetry.IsAntiSymmetry())))
            {
                _diffIds = null;
                return true;
            }

            return false;
        }
        catch (InconsistentGeneratorsException exception)
        {
            throw new InconsistentGeneratorsException("Adding inconsistent symmetry to tensor indices symmetries.");
        }
    }

    public bool AddUnsafe(byte type, Symmetry symmetry)
    {
        var data = StructureOfIndices.GetTypeData(type);
        if (data == null)
            throw new ArgumentException("No such type: " + IndexTypeMethods.GetType(type));
        if (data.Length != symmetry.Length)
            throw new ArgumentException("Wrong symmetry length.");
        int[] s = new int[StructureOfIndices.Size];
        int i = 0;
        for (; i < data.From; ++i)
            s[i] = i;
        for (int j = 0; j < data.Length; ++j, ++i)
            s[i] = symmetry.NewIndexOf(j) + data.From;
        for (; i < StructureOfIndices.Size; ++i)
            s[i] = i;
        Symmetries.AddUnsafe(new Symmetry(s, symmetry.IsAntiSymmetry()));
        return true;
    }

    /// <summary>
    /// Adds permutational (anti)symmetry of indices without any checks.
    /// </summary>
    /// <param name="symmetry"></param>
    /// <returns></returns>
    public bool AddUnsafe(Symmetry symmetry) => Symmetries.AddUnsafe(symmetry);

    /// <summary>
    /// Returns true if and only if this set contains only identity symmetry and false otherwise.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty => Symmetries.IsEmpty;

    public IndicesSymmetries Clone() => new(StructureOfIndices, Symmetries.Clone(), _diffIds);

    public static IndicesSymmetries Create(StructureOfIndices structureOfIndices)
        => structureOfIndices.Size == 0
            ? EmptySymmetries
            : new IndicesSymmetries(structureOfIndices);

    public static readonly IndicesSymmetries EmptySymmetries = new(
        new StructureOfIndices(EmptySimpleIndices.emptySimpleIndicesInstance),
        SymmetriesFactory.CreateSymmetries(0),
        []);

    public override int GetHashCode()
    {
        const int Prime1 = 113;
        const int Prime2 = 193;
        var hash = Prime1;
        unchecked
        {
            foreach (var symmetry in Symmetries)
            {
                hash = hash * Prime2 + symmetry.GetHashCode();
            }
        }

        return hash;
    }

    public List<Symmetry> Basis => Symmetries.BasisSymmetries;

    public IEnumerator<Symmetry> GetEnumerator() => Symmetries.GetEnumerator();
    public override string ToString() => Symmetries.ToString();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
