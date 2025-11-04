using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with monic polynomial remainder sequence.
/// If C is a field, then the monic PRS (on coefficients) is computed otherwise
/// no simplifications in the reduction are made.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorSimple
/// </remarks>
public class GreatestCommonDivisorSimple<C> : GreatestCommonDivisorAbstract<C>
    where C : GcdRingElem<C>
{
    public override GenPolynomial<C> BaseGcd(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        if (first.Ring.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(first));
        }

        bool field = first.Ring.CoFac.IsField();
        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);
        GenPolynomial<C> q;
        GenPolynomial<C> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
        }
        else
        {
            q = first;
            r = second;
        }

        C scale;
        if (field)
        {
            r = r.Monic();
            q = q.Monic();
            scale = first.Ring.GetOneCoefficient();
        }
        else
        {
            r = r.Abs();
            q = q.Abs();
            C contentR = BaseContent(r);
            C contentQ = BaseContent(q);
            scale = Gcd(contentR, contentQ);
            r = Divide(r, contentR);
            q = Divide(q, contentQ);
        }

        if (r.IsOne())
        {
            return r.Multiply(scale);
        }

        if (q.IsOne())
        {
            return q.Multiply(scale);
        }

        while (!r.IsZero())
        {
            GenPolynomial<C> remainder = PolyUtil.BaseSparsePseudoRemainder(q, r);
            q = r;
            r = field ? remainder.Monic() : remainder;
        }

        q = BasePrimitivePart(q);
        return q.Multiply(scale).Abs();
    }

    public override GenPolynomial<GenPolynomial<C>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<C>> first,
        GenPolynomial<GenPolynomial<C>> second)
    {
        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        if (first.Ring.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials are supported.", nameof(first));
        }

        bool field = first.LeadingBaseCoefficient().Ring.CoFac.IsField();
        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);
        GenPolynomial<GenPolynomial<C>> q;
        GenPolynomial<GenPolynomial<C>> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
        }
        else
        {
            q = first;
            r = second;
        }

        if (field)
        {
            r = PolyUtil.Monic(r)!;
            q = PolyUtil.Monic(q)!;
        }
        else
        {
            r = r.Abs();
            q = q.Abs();
        }

        GenPolynomial<C> contentR = RecursiveContent(r);
        GenPolynomial<C> contentQ = RecursiveContent(q);
        GenPolynomial<C> scale = Gcd(contentR, contentQ);
        r = PolyUtil.RecursiveDivide(r, contentR);
        q = PolyUtil.RecursiveDivide(q, contentQ);

        if (r.IsOne())
        {
            return r.Multiply(scale);
        }

        if (q.IsOne())
        {
            return q.Multiply(scale);
        }

        while (!r.IsZero())
        {
            GenPolynomial<GenPolynomial<C>> remainder = PolyUtil.RecursivePseudoRemainder(q, r);
            q = r;
            r = field ? PolyUtil.Monic(remainder)! : remainder;
        }

        q = RecursivePrimitivePart(q);
        return q.Abs().Multiply(scale);
    }

    public override GenPolynomial<C> BaseResultant(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        if (first.Ring.Nvar > 1 || first.Ring.Nvar == 0)
        {
            throw new ArgumentException("Polynomial must be univariate.", nameof(first));
        }

        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);
        if (degreeSecond == 0 && degreeFirst == 0)
        {
            return first.Ring.FromInteger(1);
        }

        if (degreeFirst == 0)
        {
            return Power<GenPolynomial<C>>.PositivePower(first, degreeSecond);
        }

        if (degreeSecond == 0)
        {
            return Power<GenPolynomial<C>>.PositivePower(second, degreeFirst);
        }

        GenPolynomial<C> q;
        GenPolynomial<C> r;
        int sign = 0;
        if (degreeFirst < degreeSecond)
        {
            r = first;
            q = second;
            (degreeSecond, degreeFirst) = (degreeFirst, degreeSecond);
            if ((degreeFirst & 1L) == 1L && (degreeSecond & 1L) == 1L)
            {
                sign = 1;
            }
        }
        else
        {
            q = first;
            r = second;
        }

        RingFactory<C> coefficientFactory = first.Ring.CoFac;
        bool field = coefficientFactory.IsField();
        C factor = coefficientFactory.FromInteger(1);
        long currentDegree;
        do
        {
            GenPolynomial<C> remainder = field ? q.Remainder(r) : PolyUtil.BaseSparsePseudoRemainder(q, r);
            if (remainder.IsZero())
            {
                return remainder;
            }

            degreeFirst = q.Degree(0);
            degreeSecond = r.Degree(0);
            if ((degreeFirst & 1L) == 1L && (degreeSecond & 1L) == 1L)
            {
                sign = 1 - sign;
            }

            currentDegree = remainder.Degree(0);
            C leading = r.LeadingBaseCoefficient();
            for (long i = 0; i < degreeFirst - currentDegree; i++)
            {
                factor = factor.Multiply(leading);
            }

            q = r;
            r = remainder;
        }
        while (currentDegree != 0);

        C tail = r.LeadingBaseCoefficient();
        for (long i = 0; i < degreeSecond; i++)
        {
            factor = factor.Multiply(tail);
        }

        if (sign == 1)
        {
            factor = factor.Negate();
        }

        return first.Ring.FromInteger(1).Multiply(factor);
    }

    public override GenPolynomial<GenPolynomial<C>> RecursiveUnivariateResultant(
        GenPolynomial<GenPolynomial<C>> first,
        GenPolynomial<GenPolynomial<C>> second)
    {
        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        if (first.Ring.Nvar > 1 || first.Ring.Nvar == 0)
        {
            throw new ArgumentException("Polynomial must be univariate.", nameof(first));
        }

        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);
        if (degreeSecond == 0 && degreeFirst == 0)
        {
            GenPolynomial<C> leadingResultant = Resultant(first.LeadingBaseCoefficient(), second.LeadingBaseCoefficient());
            return first.Ring.FromInteger(1).Multiply(leadingResultant);
        }

        if (degreeFirst == 0)
        {
            return Power<GenPolynomial<GenPolynomial<C>>>.PositivePower(first, degreeSecond);
        }

        if (degreeSecond == 0)
        {
            return Power<GenPolynomial<GenPolynomial<C>>>.PositivePower(second, degreeFirst);
        }

        GenPolynomial<GenPolynomial<C>> q;
        GenPolynomial<GenPolynomial<C>> r;
        int sign = 0;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            (degreeSecond, degreeFirst) = (degreeFirst, degreeSecond);
            if ((degreeFirst & 1L) == 1L && (degreeSecond & 1L) == 1L)
            {
                sign = 1;
            }
        }
        else
        {
            q = first;
            r = second;
        }

        RingFactory<GenPolynomial<C>> coefficientFactory = first.Ring.CoFac;
        GenPolynomial<C> factor = coefficientFactory.FromInteger(1);
        long currentDegree;
        do
        {
            GenPolynomial<GenPolynomial<C>> remainder = PolyUtil.RecursiveSparsePseudoRemainder(q, r);
            if (remainder.IsZero())
            {
                return remainder;
            }

            degreeFirst = q.Degree(0);
            degreeSecond = r.Degree(0);
            if ((degreeFirst & 1L) == 1L && (degreeSecond & 1L) == 1L)
            {
                sign = 1 - sign;
            }

            currentDegree = remainder.Degree(0);
            GenPolynomial<C> leading = r.LeadingBaseCoefficient();
            for (long i = 0; i < degreeFirst - currentDegree; i++)
            {
                factor = factor.Multiply(leading);
            }

            q = r;
            r = remainder;
        }
        while (currentDegree != 0);

        GenPolynomial<C> tail = r.LeadingBaseCoefficient();
        for (long i = 0; i < degreeSecond; i++)
        {
            factor = factor.Multiply(tail);
        }

        if (sign == 1)
        {
            factor = factor.Negate();
        }

        return first.Ring.FromInteger(1).Multiply(factor);
    }
}
