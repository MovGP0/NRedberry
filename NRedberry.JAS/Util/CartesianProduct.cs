using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// Cartesian product with iterator.
/// </summary>
/// <typeparam name="E">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.CartesianProduct
/// </remarks>
public class CartesianProduct<E> : IEnumerable<List<E>>
{
    /// <summary>
    /// Data structure.
    /// </summary>
    public readonly List<IEnumerable<E>> Comps;

    /// <summary>
    /// CartesianProduct constructor.
    /// </summary>
    /// <param name="comps">components of the Cartesian product</param>
    public CartesianProduct(List<IEnumerable<E>> comps)
    {
        Comps = comps ?? throw new ArgumentNullException(nameof(comps));
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
