namespace NRedberry.Core.Combinatorics;

public sealed class IntPermutationsSpanGenerator : IntCombinatorialGenerator, IIntCombinatorialPort
{
    private PermutationsSpanIterator<Permutation> innerIterator;
    private readonly List<Permutation> permutations;

    public IntPermutationsSpanGenerator(params int[][] permutations)
    {
        throw new NotImplementedException();
    }

    public override void Reset() => innerIterator = new PermutationsSpanIterator<Permutation>(permutations);

    public int[] GetReference() => throw new NotImplementedException();

    private int[] Next() => throw new NotImplementedException();

    public int[]? Take() => MoveNext() ? Next() : null;

    public override bool MoveNext() => innerIterator.MoveNext();

    public override int[] Current => throw new NotImplementedException();

    public override void Dispose()
    {
    }
}
