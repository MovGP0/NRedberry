namespace NRedberry.Core.Combinatorics;

///<summary>
/// This class represents an iterator over all possible unique
/// combinations with permutations (i.e. {0,1} and {1,0} both will appear in the iteration) of k numbers, which
/// can be chosen from the set of n numbers (0,1,2,...,n). The total number of such combinations will be
/// n!/(n-k)!.
///</summary>
///<remarks>
/// For example, for k=2 and n=3, it will produce the following arrays sequence: [0,1], [1,0], [0,2], [2,0], [1,2],
/// [2,1].
///
/// The iterator is implemented such that each next combination will be calculated only on the invocation of Current.
///
/// Note: Current returns the same reference on each invocation. If it is needed to save the result, clone the array.
///
/// Inner implementation of this class simply uses the combination of IntCombinationsGenerator and
/// IntPermutationsGenerator.
///</remarks>
public sealed class IntCombinationPermutationGenerator : IntCombinatorialGenerator, IIntCombinatorialPort
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
        _ = combinationsGenerator.Current;
        Array.Copy(combination, 0, combinationPermutation, 0, k);
    }

    public int[]? Take()
    {
        return MoveNext() ? Current : null;
    }

    public override bool MoveNext()
    {
        return combinationsGenerator.MoveNext() || permutationsGenerator.MoveNext();
    }

    public override int[] Current
    {
        get
        {
            if (!permutationsGenerator.MoveNext())
            {
                permutationsGenerator.Reset();
                _ = combinationsGenerator.Current;
            }

            _ = permutationsGenerator.Current;
            for (var i = 0; i < k; ++i)
            {
                combinationPermutation[i] = combination[permutation[i]];
            }

            return combinationPermutation;
        }
    }

    public override void Reset()
    {
        permutationsGenerator.Reset();
        combinationsGenerator.Reset();
        _ = combinationsGenerator.Current;
    }

    public override int[] GetReference()
    {
        return combinationPermutation;
    }

    public override void Dispose()
    {
    }
}
