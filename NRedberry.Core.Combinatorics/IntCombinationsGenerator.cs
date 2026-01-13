namespace NRedberry.Core.Combinatorics;

///<summary>
/// This class represents an iterator over all unordered combinations (i.e. [0,1] and [1,0] are
/// considered as same, so only [0,1] will appear in the sequence) of k numbers, which can be chosen from the set of
/// n numbers (0,1,2,...,n). The total number of such combinations is a
/// binomial coefficient n!/(k!(n-k)!). Each returned array is sorted.
///</summary>
///<remarks>
/// The iterator is implemented such that each next combination will be calculated only on the invocation of Current.
///
/// Note: Current returns the same reference on each invocation. If it is needed to save the result, clone the array.
///</remarks>
public sealed class IntCombinationsGenerator : IntCombinatorialGenerator, IIntCombinatorialPort
{
    private readonly int[] combination;
    private readonly int n;
    private readonly int k;
    private bool onFirst = true;

    public IntCombinationsGenerator(int n, int k)
    {
        if (n < k)
        {
            throw new ArgumentException(" n < k ");
        }

        this.n = n;
        this.k = k;
        combination = new int[k];
        Reset();
    }

    public int[]? Take()
    {
        return MoveNext() ? Current : null;
    }

    public override bool MoveNext()
    {
        return onFirst || !IsLast();
    }

    public override void Reset()
    {
        onFirst = true;
        for (var i = 0; i < k; ++i)
        {
            combination[i] = i;
        }
    }

    private bool IsLast()
    {
        for (var i = 0; i < k; ++i)
        {
            if (combination[i] != i + n - k)
            {
                return false;
            }
        }

        return true;
    }

    public override int[] Current
    {
        get
        {
            if (onFirst)
            {
                onFirst = false;
            }
            else
            {
                int i;
                for (i = k - 1; i >= 0; --i)
                {
                    if (combination[i] != i + n - k)
                    {
                        break;
                    }
                }

                var m = ++combination[i++];
                for (; i < k; ++i)
                {
                    combination[i] = ++m;
                }
            }

            return combination;
        }
    }

    public override int[] GetReference()
    {
        return combination;
    }

    public override void Dispose()
    {
    }
}
