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
        if (p1 == null || p2 == null)
        {
            throw new ArgumentNullException();
        }
        int s = p1.CompareTo(p2);
        if (Reverse)
        {
            return -s;
        }
        return s;
    }

    public override bool Equals(object? o)
    {
        if (o is not PolynomialComparator<C> pc)
        {
            return false;
        }
        return Tord.Equals(pc.Tord);
    }

    public override int GetHashCode()
    {
        return Tord.GetHashCode();
    }

    public override string ToString()
    {
        return $"PolynomialComparator({Tord})";
    }
}
