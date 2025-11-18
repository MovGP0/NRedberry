using System.Collections;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Brute-force iterator over all permutations in a group specified by the generation set.
/// </summary>
public sealed class BruteForcePermutationIterator : IEnumerator<Permutation>
{
    private static readonly Comparison<Permutation> JustPermutationComparator =
        (o1, o2) => o1.CompareTo(o2);

    private readonly SortedSet<Permutation> set;
    private List<Permutation> upperLayer;
    private List<Permutation> lowerLayer;
    private List<Permutation> nextLayer;
    private bool forward;
    private int upperIndex;
    private int  lowerIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="BruteForcePermutationIterator"/> class.
    /// </summary>
    /// <param name="permutations">The list of permutations to iterate over.</param>
    public BruteForcePermutationIterator(List<Permutation> permutations)
    {
        set = new SortedSet<Permutation>(Comparer<Permutation>.Create(JustPermutationComparator));
        upperLayer = [Permutations.CreateIdentityPermutation(Permutations.InternalDegree(permutations))];
        lowerLayer = permutations;
        nextLayer = [];
    }

    /// <summary>
    /// Gets the current permutation.
    /// </summary>
    public Permutation Current { get; private set; }

    object IEnumerator.Current => Current;

    /// <summary>
    /// Advances the iterator to the next permutation, if available.
    /// </summary>
    /// <returns><c>true</c> if there is a next permutation; otherwise, <c>false</c>.</returns>
    public bool MoveNext()
    {
        Current = GetNextPermutation();
        return Current != null;
    }

    /// <summary>
    /// Resets the iterator.
    /// </summary>
    public void Reset()
    {
        throw new NotSupportedException("Reset operation is not supported.");
    }

    /// <summary>
    /// Disposes of resources.
    /// </summary>
    public void Dispose()
    {
        // No resources to dispose
    }

    /// <summary>
    /// named "next1" in source code
    /// </summary>
    /// <returns></returns>
    private Permutation GetNextPermutation()
    {
        Permutation? composition = null;

        while (composition == null)
        {
            if (forward)
            {
                composition = TryPair(upperLayer[upperIndex], lowerLayer[lowerIndex]);
                AdvanceIndices();

                if (lowerLayer.Count == 0)
                    break;

                forward = !forward;
            }
            else
            {
                composition = TryPair(lowerLayer[lowerIndex], upperLayer[upperIndex]);
                forward = !forward;
            }
        }

        return composition!;
    }

    /// <summary>
    /// named "nexIndices" in source code
    /// </summary>
    private void AdvanceIndices()
    {
        if (++upperIndex < upperLayer.Count)
            return;

        upperIndex = 0;

        if (++lowerIndex < lowerLayer.Count)
            return;

        lowerIndex = 0;
        upperLayer = set.ToList();
        lowerLayer = nextLayer;
        nextLayer = [];
    }

    private Permutation? TryPair(Permutation p0, Permutation p1)
    {
        var composition = p0.Composition(p1);
        var setComposition = set.GetViewBetween(composition, composition).FirstOrDefault();

        if (setComposition != null)
        {
            if (setComposition.Equals(composition))
                return null;

            throw new InconsistentGeneratorsException($"{composition} and {setComposition}");
        }

        set.Add(composition);
        nextLayer.Add(composition);
        return composition;
    }

    /// <summary>
    /// Removes the current permutation.
    /// </summary>
    public void Remove()
    {
        throw new NotSupportedException("Remove operation is not supported.");
    }
}
