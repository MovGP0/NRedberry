namespace NRedberry.Core.Combinatorics;

public sealed class IntPermutationsSpanGenerator
    : IntCombinatorialGenerator, IIntCombinatorialPort
{
    private PermutationsSpanIterator<Permutation> _innerIterator;
    private readonly List<Permutation> _permutations;

    public IntPermutationsSpanGenerator(params int[][] permutations)
    {
        if (permutations == null || permutations.Length == 0)
        {
            throw new ArgumentException("At least one permutation is required.", nameof(permutations));
        }

        _permutations = new List<Permutation>(permutations.Length);
        foreach (var p in permutations)
        {
            _permutations.Add(new Symmetry(p, false, true));
        }

        _innerIterator = new PermutationsSpanIterator<Permutation>(_permutations);
    }

    public override void Reset() => _innerIterator = new PermutationsSpanIterator<Permutation>(_permutations);

    public int[] GetReference() => _innerIterator.Current.OneLine();

    private int[] Next() => _innerIterator.Current.OneLine();

    public int[]? Take() => MoveNext() ? Next() : null;

    public override bool MoveNext() => _innerIterator.MoveNext();

    public override int[] Current => _innerIterator.Current.OneLine();

    public override void Dispose()
    {
    }
}
