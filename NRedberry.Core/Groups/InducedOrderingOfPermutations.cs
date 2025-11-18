using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Ordering of permutations induced by the ordering on Ω(n) supplied by a base.
/// </summary>
public sealed class InducedOrderingOfPermutations : IComparer<Permutation>
{
    private readonly int[] _base;

    /// <summary>
    /// Creates ordering of permutations induced by an ordering on Ω(n) that is induced by a base.
    /// </summary>
    /// <param name="baseArray">Base of the permutation group.</param>
    public InducedOrderingOfPermutations(int[] baseArray)
    {
        ArgumentNullException.ThrowIfNull(baseArray);

        _base = (int[])baseArray.Clone();
        InducedOrdering = new InducedOrdering(_base);
    }

    /// <summary>
    /// Returns the induced ordering on Ω(n).
    /// </summary>
    public InducedOrdering InducedOrdering { get; }

    public int Compare(Permutation? x, Permutation? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        foreach (int point in _base)
        {
            int compare = InducedOrdering.Compare(x.NewIndexOf(point), y.NewIndexOf(point));
            if (compare != 0)
            {
                return compare;
            }
        }

        return 0;
    }
}
