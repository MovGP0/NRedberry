using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Iterator over the term map of a polynomial; adapts <c>IEnumerable</c> enumerator semantics to <see cref="Monomial{C}"/>.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolyIterator
/// </remarks>
public sealed class PolyIterator<C> : IEnumerator<Monomial<C>> where C : RingElem<C>
{
    private readonly IEnumerator<KeyValuePair<ExpVector, C>> innerEnumerator;

    /// <summary>
    /// Initializes the iterator over the provided exponent-to-coefficient map.
    /// </summary>
    /// <param name="map">Polynomial term map.</param>
    public PolyIterator(IDictionary<ExpVector, C> map)
    {
        ArgumentNullException.ThrowIfNull(map);
        innerEnumerator = map.GetEnumerator();
    }

    /// <summary>
    /// Gets the current monomial.
    /// </summary>
    public Monomial<C> Current => new Monomial<C>(innerEnumerator.Current);

    object IEnumerator.Current => Current;

    /// <summary>
    /// Advances the iterator to the next monomial.
    /// </summary>
    public bool MoveNext()
    {
        return innerEnumerator.MoveNext();
    }

    /// <summary>
    /// Resets the iteration to the beginning.
    /// </summary>
    public void Reset()
    {
        innerEnumerator.Reset();
    }

    public void Dispose()
    {
        innerEnumerator.Dispose();
    }
}
