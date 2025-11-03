using System;
using System.Collections;
using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Iterator over monomials of a polynomial. Adaptor for val.entrySet().iterator().
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolyIterator
/// </remarks>
public sealed class PolyIterator<C> : IEnumerator<Monomial<C>> where C : RingElem<C>
{
    private readonly IEnumerator<KeyValuePair<ExpVector, C>> innerEnumerator;

    public PolyIterator(IDictionary<ExpVector, C> map)
    {
        ArgumentNullException.ThrowIfNull(map);
        innerEnumerator = map.GetEnumerator();
    }

    public Monomial<C> Current => new Monomial<C>(innerEnumerator.Current);

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        return innerEnumerator.MoveNext();
    }

    public void Reset()
    {
        innerEnumerator.Reset();
    }

    public void Dispose()
    {
        innerEnumerator.Dispose();
    }
}
