using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics;

public sealed class IntPermutationsSpanGenerator : IntCombinatorialGenerator, IIntCombinatorialPort
{
    private PermutationsSpanIterator<Permutation> innerIterator;  // Assuming PermutationsSpanIterator<Permutation> exists in your codebase.
    private readonly List<Permutation> permutations; // Assuming Permutation exists in your codebase.

    public IntPermutationsSpanGenerator(params int[][] permutations)
    {
        this.permutations = new List<Permutation>(permutations.Length);
        foreach (int[] p in permutations)
        {
            // Assuming Permutation has a constructor that takes an int array.
            this.permutations.Add(new Permutation(p));
        }
        innerIterator = new PermutationsSpanIterator<Permutation>(this.permutations);
    }

    public override void Reset()
    {
        innerIterator = new PermutationsSpanIterator<Permutation>(permutations);
    }

    public int[] GetReference()
    {
        return innerIterator.Current!.GetPermutation();  // Assuming Permutation has a property called Permutation.
    }

    private int[] Next()
    {
        return innerIterator.Current!.GetPermutation();  // Assuming Permutation has a property called Permutation.
    }

    public int[]? Take()
    {
        return MoveNext() ? Next() : null;
    }

    public override bool MoveNext()
    {
        return innerIterator.MoveNext();
    }

    public override int[] Current
    {
        get
        {
            // Assuming Permutation.Permutation is an int[]
            // Change this if you need a different element of Permutation.Permutation
            return innerIterator.Current!.GetPermutation();
        }
    }

    public override void Dispose()
    {
    }
}