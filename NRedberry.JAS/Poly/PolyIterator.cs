using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Iterator over monomials of a polynomial. Adaptor for val.entrySet().iterator().
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolyIterator
/// </remarks>
public class PolyIterator<C> : IEnumerator<Monomial<C>> where C : RingElem<C>
{
    /// <summary>
    /// Internal iterator over polynomial map.
    /// </summary>
    protected readonly IEnumerator<KeyValuePair<ExpVector, C>> ms;

    /// <summary>
    /// Constructor of polynomial iterator.
    /// </summary>
    /// <param name="m">SortedMap of a polynomial</param>
    public PolyIterator(SortedDictionary<ExpVector, C> m)
    {
        ms = m.GetEnumerator();
    }

    /// <summary>
    /// Test for availability of a next monomial.
    /// </summary>
    /// <returns>true if the iteration has more monomials, else false.</returns>
    public bool MoveNext()
    {
        return ms.MoveNext();
    }

    /// <summary>
    /// Get current monomial element.
    /// </summary>
    public Monomial<C> Current => new Monomial<C>(ms.Current);

    object IEnumerator.Current => Current;

    public void Reset()
    {
        ms.Reset();
    }

    public void Dispose()
    {
        ms.Dispose();
    }
}
