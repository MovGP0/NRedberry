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
            throw new ArgumentException("k out of range");
        }
        K = k;
    }

    /// <summary>
    /// Get an iterator over subsets.
    /// </summary>
    /// <returns>an iterator.</returns>
    public IEnumerator<List<E>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
