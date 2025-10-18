using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

/// <summary>
/// An iterator over group elements, scanning the group in increasing order
/// permutations according to the ordering induced by a base.
/// </summary>
public sealed class BacktrackSearch
{
    private readonly List<BSGSElement> _bsgs;
    private readonly int[] _tuple;
    private readonly int[][] _sortedOrbits;
    private readonly Permutation[] _word;
    private readonly int _size;
    private readonly IComparer<int> _ordering;
    private readonly int[][] _cachedSortedOrbits;

    private int _level = 0;

    /// <summary>
    /// Creates an iterator over group elements that satisfy the specified property.
    /// </summary>
    /// <param name="bsgs">Base and strong generating set.</param>
    /// <param name="test">Test function to apply at each level of the search tree.</param>
    /// <param name="property">Property of permutations.</param>
    public BacktrackSearch(List<BSGSElement> bsgs, IBacktrackSearchTestFunction test, IIndicator<Permutation> property)
    {
        throw new NotImplementedException();
        // if (bsgs.Count == 0)
        //     throw new ArgumentException("Empty BSGS.");
        // 
        // _bsgs = bsgs;
        // _size = bsgs.Count;
        // _tuple = new int[_size];
        // Array.Fill(_tuple, -1);
        // 
        // _ordering = new InducedOrdering(AlgorithmsBase.GetBaseAsArray(bsgs));
        // 
        // _word = new Permutation[_size];
        // _sortedOrbits = new int[_size][];
        // _cachedSortedOrbits = new int[_size][];
        // 
        // for (int i = _size - 1; i >= 0; --i)
        // {
        //     _cachedSortedOrbits[i] = bsgs[i].OrbitList.ToArray();
        //     Array.Sort(_cachedSortedOrbits[i], _ordering.Compare);
        // }
        // 
        // _sortedOrbits[0] = _cachedSortedOrbits[0];
        // _testFunction = test;
        // _property = property;
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
    public IComparer<int> InducedOrdering => _ordering;

    /// <summary>
    /// Gets the level of the last changed element.
    /// </summary>
    public int LastModifiedLevel => _level;

    /// <summary>
    /// Gets a reference to the current permutation word.
    /// </summary>
    public Permutation[] WordReference => _word;

    /// <summary>
    /// Searches and returns the next element in the group.
    /// </summary>
    /// <returns>The next element in the group.</returns>
    public Permutation? Take()
    {
        throw new NotImplementedException();
        // if (_level == -1)
        //     return null;
        // 
        // while (true)
        // {
        //     Backtrack();
        // 
        //     if (_level == -1)
        //         return null;
        // 
        //     while (_level < _size - 1 && _testFunction.Test(_word[_level], _level))
        //     {
        //         ++_level;
        //         CalculateSortedOrbit(_level);
        //         _tuple[_level] = 0;
        //         _word[_level] = _bsgs[_level].GetTransversalOf(
        //             _word[_level - 1].NewIndexOfUnderInverse(_sortedOrbits[_level][_tuple[_level]]))
        //             .Composition(_word[_level - 1]);
        //     }
        // 
        //     if (_level != _size - 1 || !_testFunction.Test(_word[_level], _level) || !_property.Is(_word[_level]))
        //         continue;
        // 
        //     return _word[_level];
        // }
    }

    private void Backtrack()
    {
        throw new NotImplementedException();
        // while (_level >= 0 && _tuple[_level] == _bsgs[_level].OrbitList.Count - 1)
        //     --_level;
        // 
        // if (_level == -1)
        //     return;
        // 
        // ++_tuple[_level];
        // 
        // if (_level == 0)
        // {
        //     _word[0] = _bsgs[0].GetTransversalOf(_sortedOrbits[0][_tuple[0]]);
        // }
        // else
        // {
        //     _word[_level] = _bsgs[_level].GetTransversalOf(
        //             _word[_level - 1].NewIndexOfUnderInverse(_sortedOrbits[_level][_tuple[_level]]))
        //         .Composition(_word[_level - 1]);
        // }
    }

    private void CalculateSortedOrbit(int level)
    {
        throw new NotImplementedException();
        // if (_word[level - 1].IsIdentity())
        // {
        //     _sortedOrbits[level] = _cachedSortedOrbits[level];
        // }
        // else
        // {
        //     _sortedOrbits[level] = _word[level - 1].ImageOf(_bsgs[level].OrbitList.ToArray());
        //     Array.Sort(_sortedOrbits[level], _ordering.Compare);
        // }
    }
}