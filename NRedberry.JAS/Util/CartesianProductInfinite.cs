using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// Cartesian product of infinite components with iterator. Works also for finite iterables.
/// </summary>
/// <typeparam name="E">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.CartesianProductInfinite
/// </remarks>
public class CartesianProductInfinite<E> : IEnumerable<List<E>>
{
    /// <summary>
    /// Data structure.
    /// </summary>
    public readonly List<IEnumerable<E>> Comps;

    /// <summary>
    /// CartesianProduct constructor.
    /// </summary>
    /// <param name="comps">components of the Cartesian product</param>
    public CartesianProductInfinite(List<IEnumerable<E>> comps)
    {
        if (comps == null || comps.Count == 0)
        {
            throw new ArgumentException("null or empty components not allowed");
        }
        Comps = comps;
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
