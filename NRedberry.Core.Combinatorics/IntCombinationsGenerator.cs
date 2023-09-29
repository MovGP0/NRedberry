using System.Collections;

namespace NRedberry.Core.Combinatorics;

///<summary>
/// This class represents an iterator over all unordered combinations (i.e. [0,1] and [1,0] are
/// considered as same, so only [0,1] will appear in the sequence) of k numbers, which can be chosen from the set of
/// n numbers (0,1,2,...,n). The total number of such combinations is a
/// binomial coefficient n!/(k!(n-k)!). Each returned array is sorted.
///</summary>
public sealed class IntCombinationsGenerator : IIntCombinatorialGenerator, IIntCombinatorialPort
{
    private readonly int[] combination;
    private readonly int n;
    private readonly int k;
    private bool onFirst = true;

    public IntCombinationsGenerator(int n, int k)
    {
        if (n < k)
            throw new ArgumentException(" n < k ");
        this.n = n;
        this.k = k;
        combination = new int[k];
        Reset();
    }

    public int[]? Take()
    {
        return MoveNext() ? Current : null;
    }

    public bool MoveNext()
    {
        return onFirst || !isLast();
    }

    public void Reset()
    {
        onFirst = true;
        for (var i = 0; i < k; ++i)
            combination[i] = i;
    }

    private bool isLast()
    {
        for (var i = 0; i < k; ++i)
            if (combination[i] != i + n - k)
                return false;
        return true;
    }

    public int[] Current
    {
        get
        {
            if (onFirst)
                onFirst = false;
            else
            {
                int i;
                for (i = k - 1; i >= 0; --i)
                    if (combination[i] != i + n - k)
                        break;
                var m = ++combination[i++];
                for (; i < k; ++i)
                    combination[i] = ++m;
            }
            return combination;
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        // Not supported yet.
    }

    public IEnumerator<int[]> GetEnumerator()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int[] GetReference()
    {
        return combination;
    }
}