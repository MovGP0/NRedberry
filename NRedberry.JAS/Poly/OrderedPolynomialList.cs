using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Ordered list of polynomials used for storage, printing, and conversions.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.OrderedPolynomialList
/// </remarks>
public class OrderedPolynomialList<C> : PolynomialList<C>, IComparer<GenPolynomial<C>> where C : RingElem<C>
{
    /// <summary>
    /// Constructs the ordered list using the supplied polynomial sequence.
    /// </summary>
    public OrderedPolynomialList(GenPolynomialRing<C> r, List<GenPolynomial<C>> l)
        : base(r, SortInternal(r, l))
    {
    }

    /// <summary>
    /// Sorts a polynomial list according to ascending leading exponent vectors.
    /// </summary>
    public static List<GenPolynomial<C>> Sort(GenPolynomialRing<C> ring, List<GenPolynomial<C>> polynomials)
    {
        return SortInternal(ring, polynomials);
    }

    /// <summary>
    /// Compares polynomials by their leading exponent vector lengths and the ring comparator.
    /// </summary>
    public int Compare(GenPolynomial<C>? x, GenPolynomial<C>? y)
    {
        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        ExpVector? left = x.LeadingExpVector();
        ExpVector? right = y.LeadingExpVector();
        if (left is null)
        {
            return right is null ? 0 : -1;
        }

        if (right is null)
        {
            return 1;
        }

        IComparer<ExpVector> comparer = Ring.Tord.GetAscendComparator();
        if (left.Length() != right.Length())
        {
            return left.Length().CompareTo(right.Length());
        }

        return comparer.Compare(left, right);
    }

    /// <summary>
    /// Internal helper to sort while guarding against null/empty lists.
    /// </summary>
    private static List<GenPolynomial<C>> SortInternal(GenPolynomialRing<C> ring, List<GenPolynomial<C>> polynomials)
    {
        if (polynomials == null || polynomials.Count <= 1)
        {
            return polynomials ?? [];
        }

        IComparer<ExpVector> comparer = ring.Tord.GetAscendComparator();
        List<GenPolynomial<C>> sorted = new (polynomials);
        sorted.Sort(
            (left, right) =>
            {
                ExpVector? leftVector = left.LeadingExpVector();
                ExpVector? rightVector = right.LeadingExpVector();
                if (leftVector is null)
                {
                    return -1;
                }

                if (rightVector is null)
                {
                    return 1;
                }

                if (leftVector.Length() != rightVector.Length())
                {
                    return leftVector.Length().CompareTo(rightVector.Length());
                }

                return comparer.Compare(leftVector, rightVector);
            });

        return sorted;
    }
}
