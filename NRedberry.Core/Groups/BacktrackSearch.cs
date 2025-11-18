using NRedberry.Core.Combinatorics;
using NRedberry.Core.Concurrent;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

/// <summary>
/// An iterator over group elements that scans the group in the ordering induced by the base.
/// </summary>
public sealed class BacktrackSearch : IOutputPort<Permutation>
{
    private readonly IReadOnlyList<BSGSElement> _bsgs;
    private readonly int[] _tuple;
    private readonly int[][] _sortedOrbits;
    private readonly int[][] _cachedSortedOrbits;
    private readonly int _size;

    private int _level;

    /// <summary>
    /// Creates an iterator over group elements that satisfy the specified property.
    /// </summary>
    /// <param name="bsgs">Base and strong generating set.</param>
    /// <param name="test">Test function to apply at each level of the search tree.</param>
    /// <param name="property">Property of permutations.</param>
    /// <exception cref="ArgumentException">Thrown when the BSGS list is empty.</exception>
    public BacktrackSearch(
        List<BSGSElement> bsgs,
        IBacktrackSearchTestFunction test,
        IIndicator<Permutation> property)
    {
        if (bsgs.Count == 0)
        {
            throw new ArgumentException("Empty BSGS.", nameof(bsgs));
        }

        _bsgs = bsgs;
        _size = bsgs.Count;
        _tuple = new int[_size];
        Array.Fill(_tuple, -1);

        InducedOrdering = new InducedOrdering(AlgorithmsBase.GetBaseAsArray(bsgs));
        WordReference = new Permutation[_size];
        _sortedOrbits = new int[_size][];
        _cachedSortedOrbits = new int[_size][];

        for (int i = _size - 1; i >= 0; --i)
        {
            _cachedSortedOrbits[i] = bsgs[i].OrbitListReference.ToArray();
            Array.Sort(_cachedSortedOrbits[i], InducedOrdering.Compare);
        }

        _sortedOrbits[0] = _cachedSortedOrbits[0];
        TestFunction = test ?? throw new ArgumentNullException(nameof(test));
        Property = property ?? throw new ArgumentNullException(nameof(property));
        _level = 0;
    }

    /// <summary>
    /// Creates an iterator over group elements.
    /// </summary>
    /// <param name="bsgs">Base and strong generating set.</param>
    public BacktrackSearch(List<BSGSElement> bsgs)
        : this(bsgs, new TrueBacktrackSearchTestFunction(), new TrueIndicator<Permutation>())
    {
    }

    /// <summary>
    /// Gets or sets the test function used to rule out unnecessary tree branches.
    /// </summary>
    public IBacktrackSearchTestFunction TestFunction { get; set; }

    /// <summary>
    /// Gets or sets the property of elements that are searched for in the group.
    /// </summary>
    public IIndicator<Permutation> Property { get; set; }

    /// <summary>
    /// Gets the ordering on Ω(n) induced by a base.
    /// </summary>
    public InducedOrdering InducedOrdering { get; }

    /// <summary>
    /// Gets the level of the last changed element.
    /// </summary>
    public int LastModifiedLevel => _level;

    /// <summary>
    /// Gets a reference to the current permutation word.
    /// </summary>
    public Permutation[] WordReference { get; }

    /// <summary>
    /// Searches and returns the next element in the group.
    /// </summary>
    /// <returns>The next element in the group or <c>null</c> when the traversal is complete.</returns>
    public Permutation? Take()
    {
        if (_level == -1)
        {
            return null;
        }

        while (true)
        {
            Backtrack();

            if (_level == -1)
            {
                return null;
            }

            while (_level < _size - 1 && TestFunction.Test(WordReference[_level], _level))
            {
                ++_level;
                CalculateSortedOrbit(_level);
                _tuple[_level] = 0;
                WordReference[_level] = _bsgs[_level]
                    .GetTransversalOf(WordReference[_level - 1].NewIndexOfUnderInverse(_sortedOrbits[_level][_tuple[_level]]))
                    .Composition(WordReference[_level - 1]);
            }

            if (_level != _size - 1 || !TestFunction.Test(WordReference[_level], _level) || !Property.Is(WordReference[_level]))
            {
                continue;
            }

            return WordReference[_level];
        }
    }

    private void Backtrack()
    {
        while (_level >= 0 && _tuple[_level] == _bsgs[_level].OrbitListReference.Count - 1)
        {
            --_level;
        }

        if (_level == -1)
        {
            return;
        }

        ++_tuple[_level];

        if (_level == 0)
        {
            WordReference[0] = _bsgs[0].GetTransversalOf(_sortedOrbits[0][_tuple[0]]);
        }
        else
        {
            WordReference[_level] = _bsgs[_level]
                .GetTransversalOf(WordReference[_level - 1].NewIndexOfUnderInverse(_sortedOrbits[_level][_tuple[_level]]))
                .Composition(WordReference[_level - 1]);
        }
    }

    private void CalculateSortedOrbit(int level)
    {
        if (WordReference[level - 1].IsIdentity)
        {
            _sortedOrbits[level] = _cachedSortedOrbits[level];
        }
        else
        {
            _sortedOrbits[level] = WordReference[level - 1].ImageOf(_bsgs[level].OrbitListReference.ToArray());
            Array.Sort(_sortedOrbits[level], InducedOrdering.Compare);
        }
    }

    Permutation IOutputPort<Permutation>.Take()
    {
        return Take()!;
    }
}
