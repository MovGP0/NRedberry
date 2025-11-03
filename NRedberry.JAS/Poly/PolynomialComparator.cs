using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Comparator for polynomials.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolynomialComparator
/// </remarks>
public class PolynomialComparator<C> : IComparer<GenPolynomial<C>> where C : RingElem<C>
{
    public readonly TermOrder Tord;
    public readonly bool Reverse;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="t">TermOrder</param>
    /// <param name="reverse">flag if reverse ordering is requested</param>
    public PolynomialComparator(TermOrder t, bool reverse)
    {
        Tord = t;
        Reverse = reverse;
    }

    /// <summary>
    /// Compare polynomials.
    /// </summary>
    /// <param name="p1">first polynomial</param>
    /// <param name="p2">second polynomial</param>
    /// <returns>0 if ( p1 == p2 ), -1 if ( p1 &lt; p2 ) and +1 if ( p1 &gt; p2 ).</returns>
    public int Compare(GenPolynomial<C>? p1, GenPolynomial<C>? p2)
    {
        if (ReferenceEquals(p1, p2))
        {
            return 0;
        }

        if (p1 is null)
        {
            return Reverse ? 1 : -1;
        }

        if (p2 is null)
        {
            return Reverse ? -1 : 1;
        }

        int result = p1.CompareTo(p2);
        return Reverse ? -result : result;
    }

    public override bool Equals(object? o)
    {
        if (o is not PolynomialComparator<C> pc)
        {
            return false;
        }

        return Reverse == pc.Reverse && Tord.Equals(pc.Tord);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Tord, Reverse);
    }

    public override string ToString()
    {
        return $"PolynomialComparator({Tord}, reverse: {Reverse})";
    }
}
