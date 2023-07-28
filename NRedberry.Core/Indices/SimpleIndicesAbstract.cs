using System.Diagnostics;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Indices;

public abstract class SimpleIndicesAbstract : AbstractIndices, SimpleIndices
{
    protected IndicesSymmetries? symmetries;

    protected SimpleIndicesAbstract(int[] data, IndicesSymmetries symmetries) : base(data)
    {
        Debug.Assert(data.Length != 0);

        int[] toSort = new int[data.Length];
        for (int i = 0; i < data.Length; ++i)
            toSort[i] = data[i] & 0x7F000000;
        ArraysUtils.StableSort(toSort, data);
        this.symmetries = symmetries;
        TestConsistentWithException();
    }

    protected SimpleIndicesAbstract(bool notResort, int[] data, IndicesSymmetries symmetries) : base(data)
    {
        Debug.Assert(data.Length != 0);
        this.symmetries = symmetries;
    }

    // TODO: protected UpperLowerIndices CalculateUpperLower()
    // TODO: int Size(IndexType type)
    // TODO: int this[IndexType type, int position]
    // TODO: SimpleIndices GetInverted()
    // TODO: SimpleIndices Free { get; }
    // TODO: SimpleIndices OfType(IndexType type)
    // TODO: SimpleIndices applyIndexMapping(IndexMapping mapping)
    // TODO: protected abstract SimpleIndices Create(int[] data, IndicesSymmetries symmetries);
    // TODO: int[] SortedData { get; }
    // TODO: void TestConsistentWithException()

    public bool EqualsWithSymmetries(SimpleIndices indices) => EqualsWithSymmetriesDetailed(indices) == false;

    /// <summary>
    /// More informative method, comparing indices using their symmetries lists.
    /// It returns false if indices are equals this, true if indices differs from this on -1 (i.e. on odd transposition)
    /// and null in other case.
    /// </summary>
    /// <param name="indices>indices to compare with this</param>
    /// <returns>false if indices are equals this, true if indices differs from this on -1 (i.e. on odd transposition)
    /// and null in other case.</returns>
    public bool? EqualsWithSymmetriesDetailed(SimpleIndices indices)
    {
        if (indices.GetType() != this.GetType())
            return null;

        if (Data.Length != indices.Size())
            return null;

        if (indices is not SimpleIndicesOfTensor indices_)
        {
            return null;
        }

        bool sign1;

        foreach (Symmetry s1 in symmetries)
        {
            sign1 = s1.IsAntiSymmetry();
            for (int i = 0; i < Data.Length; ++i)
            {
                if (Data[s1.NewIndexOf(i)] != indices_.Data[i])
                {
                    goto outer;
                }
            }

            return sign1;
#warning check this goto jump
            outer:
                continue;
        }

        return null;
    }

    public IndicesSymmetries? Symmetries
    {
        get => symmetries;
        set => symmetries = value;
    }

    public short[]? DiffIds => Symmetries?.DiffIds;
    public StructureOfIndices StructureOfIndices => new(this);
}