using System.Collections;

namespace NRedberry.Core.Combinatorics;

///<summary>
/// This class represents an iterator over all possible unique
/// combinations with permutations (i.e. {0,1} and {1,0} both will appear in the iteration) of k numbers, which
/// can be chosen from the set of n numbers (0,1,2,...,n). The total number of such combinations will be
/// n!/(n-k)!.
///</summary>
public sealed class IntCombinationPermutationGenerator : IIntCombinatorialGenerator, IIntCombinatorialPort
{
    private readonly int[] permutation;
    private readonly int[] combination;
    private readonly int[] combinationPermutation;
    private readonly IntPermutationsGenerator permutationsGenerator;
    private readonly IntCombinationsGenerator combinationsGenerator;
    private readonly int k;

    public IntCombinationPermutationGenerator(int n, int k)
    {
        this.k = k;
        combinationsGenerator = new IntCombinationsGenerator(n, k);
        combination = combinationsGenerator.GetReference();
        permutationsGenerator = new IntPermutationsGenerator(k);
        permutation = permutationsGenerator.GetReference();
        combinationPermutation = new int[k];
        combinationsGenerator.MoveNext();
        Array.Copy(combination, 0, combinationPermutation, 0, k);
    }

    public int[]? Take()
    {
        return MoveNext() ? Current : null;
    }

    public bool MoveNext()
    {
        return combinationsGenerator.MoveNext() || permutationsGenerator.MoveNext();
    }

    public int[] Current
    {
        get
        {
            if (!permutationsGenerator.MoveNext())
            {
                permutationsGenerator.Reset();
                combinationsGenerator.MoveNext();
            }

            permutationsGenerator.MoveNext();
            for (var i = 0; i < k; ++i)
                combinationPermutation[i] = combination[permutation[i]];
            return combinationPermutation;
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        // Not supported yet.
    }

    public void Reset()
    {
        permutationsGenerator.Reset();
        combinationsGenerator.Reset();
        combinationsGenerator.MoveNext();
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
        return combinationPermutation;
    }
}
