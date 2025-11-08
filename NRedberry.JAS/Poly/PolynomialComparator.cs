using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Comparator that orders polynomials using a term order and optional reverse flag.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolynomialComparator
/// </remarks>
public class PolynomialComparator<C> : IComparer<GenPolynomial<C>> where C : RingElem<C>
{
    /// <summary>
    /// Term order guiding the comparison.
    /// </summary>
    public readonly TermOrder Tord;

    /// <summary>
    /// When <c>true</c>, the comparison result is inverted.
    /// </summary>
    public readonly bool Reverse;

    /// <summary>
    /// Creates a comparator tied to the specified term order and direction.
    /// </summary>
    /// <param name="t">Term order.</param>
    /// <param name="reverse"><c>true</c> to invert the comparison result.</param>
    public PolynomialComparator(TermOrder t, bool reverse)
    {
        Tord = t;
        Reverse = reverse;
    }

    /// <summary>
    /// Compares two polynomials according to the stored term order, optionally reversing.
    /// </summary>
    /// <param name="p1">First polynomial.</param>
    /// <param name="p2">Second polynomial.</param>
    /// <returns>Comparison sign.</returns>
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

    /// <summary>
    /// Equality considers the term order and reverse flag.
    /// </summary>
    public override bool Equals(object? o)
    {
        if (o is not PolynomialComparator<C> pc)
        {
            return false;
        }

        return Reverse == pc.Reverse && Tord.Equals(pc.Tord);
    }

    /// <summary>
    /// Hashes the term order and direction.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Tord, Reverse);
    }

    /// <summary>
    /// Returns a concise description of the comparator.
    /// </summary>
    public override string ToString()
    {
        return $"PolynomialComparator({Tord}, reverse: {Reverse})";
    }
}
