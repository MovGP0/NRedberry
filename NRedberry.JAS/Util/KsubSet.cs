using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// K-Subset with iterator.
/// </summary>
/// <typeparam name="E">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.KsubSet
/// </remarks>
public class KsubSet<E> : IEnumerable<List<E>>
{
    /// <summary>
    /// Data structure.
    /// </summary>
    public readonly List<E> Set;

    public readonly int K;

    /// <summary>
    /// KsubSet constructor.
    /// </summary>
    /// <param name="set">generating set</param>
    /// <param name="k">size of subsets</param>
    public KsubSet(List<E> set, int k)
    {
        Set = set ?? throw new ArgumentNullException(nameof(set));
        if (k < 0 || k > set.Count)
        {
            throw new ArgumentException("k out of range", nameof(k));
        }
        K = k;
    }

    /// <summary>
    /// Get an iterator over subsets.
    /// </summary>
    /// <returns>an iterator.</returns>
    public IEnumerator<List<E>> GetEnumerator()
    {
        return GenerateSubsets().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerable<List<E>> GenerateSubsets()
    {
        if (K == 0)
        {
            yield return [];
            yield break;
        }

        foreach (List<E> subset in GenerateSubsetsRecursive(0, K))
        {
            yield return subset;
        }
    }

    private IEnumerable<List<E>> GenerateSubsetsRecursive(int start, int remaining)
    {
        if (remaining == 0)
        {
            yield return [];
            yield break;
        }

        for (int i = start; i <= Set.Count - remaining; i++)
        {
            E element = Set[i];
            foreach (List<E> tail in GenerateSubsetsRecursive(i + 1, remaining - 1))
            {
                List<E> subset = new List<E>(tail.Count + 1) { element };
                subset.AddRange(tail);
                yield return subset;
            }
        }
    }
}
