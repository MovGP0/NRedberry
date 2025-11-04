namespace NRedberry.Core.Combinatorics;

public sealed class IntPermutationsSpanGenerator : IntCombinatorialGenerator, IIntCombinatorialPort
{
    private PermutationsSpanIterator<Permutation> innerIterator;
    private readonly List<Permutation> permutations;

    public IntPermutationsSpanGenerator(params int[][] permutations)
    {
        this.permutations = new List<Permutation>(permutations.Length);
        foreach (var p in permutations)
        {
            this.permutations.Add(new Permutation(p));
        }

        innerIterator = new PermutationsSpanIterator<Permutation>(this.permutations);
    }

    public override void Reset() => innerIterator = new PermutationsSpanIterator<Permutation>(permutations);

    public int[] GetReference() => innerIterator.Current!.GetPermutation();

    private int[] Next() => innerIterator.Current!.GetPermutation();

    public int[]? Take() => MoveNext() ? Next() : null;

    public override bool MoveNext() => innerIterator.MoveNext();

    public override int[] Current => innerIterator.Current!.GetPermutation();

    public override void Dispose()
    {
    }
}
