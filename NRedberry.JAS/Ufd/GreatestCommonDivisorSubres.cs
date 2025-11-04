using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with subresultant polynomial remainder sequence.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorSubres
/// </remarks>
public class GreatestCommonDivisorSubres<C> : GreatestCommonDivisorAbstract<C>
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

        r = r.Abs();
        q = q.Abs();
        C contentR = BaseContent(r);
        C contentQ = BaseContent(q);
        C scale = Gcd(contentR, contentQ);
        r = Divide(r, contentR);
        q = Divide(q, contentQ);

        if (r.IsOne())
        {
            return r.Multiply(scale);
        }

        if (q.IsOne())
        {
            return q.Multiply(scale);
        }

        C g = r.Ring.GetOneCoefficient();
        C h = r.Ring.GetOneCoefficient();

        while (!r.IsZero())
        {
            long delta = q.Degree(0) - r.Degree(0);
            GenPolynomial<C> remainder = PolyUtil.BaseDensePseudoRemainder(q, r);
            q = r;
            if (!remainder.IsZero())
            {
                C multiplier = g.Multiply(PowerCoefficient(first.Ring.CoFac, h, delta));
                r = remainder.Divide(multiplier);
                g = q.LeadingBaseCoefficient();
                C gPower = PowerCoefficient(first.Ring.CoFac, g, delta);
                h = gPower.Divide(PowerCoefficient(first.Ring.CoFac, h, delta - 1));
            }
            else
            {
                r = remainder;
            }
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

        r = r.Abs();
        q = q.Abs();
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

        GenPolynomial<C> g = r.Ring.GetOneCoefficient();
        GenPolynomial<C> h = r.Ring.GetOneCoefficient();

        while (!r.IsZero())
        {
            long delta = q.Degree(0) - r.Degree(0);
            GenPolynomial<GenPolynomial<C>> remainder = PolyUtil.RecursiveDensePseudoRemainder(q, r);
            q = r;
            if (!remainder.IsZero())
            {
                GenPolynomial<C> multiplier = g.Multiply(PowerPolynomial(first.Ring.CoFac, h, delta));
                r = PolyUtil.RecursiveDivide(remainder, multiplier);
                g = q.LeadingBaseCoefficient();
                GenPolynomial<C> gPower = PowerPolynomial(first.Ring.CoFac, g, delta);
                h = gPower.Divide(PowerPolynomial(first.Ring.CoFac, h, delta - 1));
            }
            else
            {
                r = remainder;
            }
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

        if (first.Ring.Nvar > 1)
        {
            throw new ArgumentException("Polynomial must be univariate.", nameof(first));
        }

        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);
        GenPolynomial<C> q;
        GenPolynomial<C> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            long swap = degreeSecond;
            degreeSecond = degreeFirst;
            degreeFirst = swap;
        }
        else
        {
            q = first;
            r = second;
        }

        r = r.Abs();
        q = q.Abs();
        C contentR = BaseContent(r);
        C contentQ = BaseContent(q);
        r = Divide(r, contentR);
        q = Divide(q, contentQ);

        RingFactory<C> coefficientFactory = first.Ring.CoFac;
        C g = coefficientFactory.FromInteger(1);
        C h = coefficientFactory.FromInteger(1);
        C resultantFactor = PowerCoefficient(coefficientFactory, contentR, degreeFirst)
            .Multiply(PowerCoefficient(coefficientFactory, contentQ, degreeSecond));
        long sign = 1;

        while (r.Degree(0) > 0)
        {
            long delta = q.Degree(0) - r.Degree(0);
            if ((q.Degree(0) & 1L) == 1L && (r.Degree(0) & 1L) == 1L)
            {
                sign = -sign;
            }

            GenPolynomial<C> remainder = PolyUtil.BaseDensePseudoRemainder(q, r);
            q = r;
            if (remainder.Degree(0) > 0)
            {
                C multiplier = g.Multiply(PowerCoefficient(coefficientFactory, h, delta));
                r = remainder.Divide(multiplier);
                g = q.LeadingBaseCoefficient();
                C gPower = PowerCoefficient(coefficientFactory, g, delta);
                h = gPower.Divide(PowerCoefficient(coefficientFactory, h, delta - 1));
            }
            else
            {
                r = remainder;
            }
        }

        C z = PowerCoefficient(coefficientFactory, r.LeadingBaseCoefficient(), q.Degree(0));
        h = z.Divide(PowerCoefficient(coefficientFactory, h, q.Degree(0) - 1));
        z = h.Multiply(resultantFactor);
        if (sign < 0)
        {
            z = z.Negate();
        }

        return first.Ring.FromInteger(1).Multiply(z);
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

        if (first.Ring.Nvar > 1)
        {
            throw new ArgumentException("Polynomial must be univariate.", nameof(first));
        }

        long degreeFirst = first.Degree(0);
        long degreeSecond = second.Degree(0);
        GenPolynomial<GenPolynomial<C>> q;
        GenPolynomial<GenPolynomial<C>> r;
        if (degreeSecond > degreeFirst)
        {
            r = first;
            q = second;
            long swap = degreeSecond;
            degreeSecond = degreeFirst;
            degreeFirst = swap;
        }
        else
        {
            q = first;
            r = second;
        }

        r = r.Abs();
        q = q.Abs();
        GenPolynomial<C> contentR = RecursiveContent(r);
        GenPolynomial<C> contentQ = RecursiveContent(q);
        r = PolyUtil.RecursiveDivide(r, contentR);
        q = PolyUtil.RecursiveDivide(q, contentQ);

        RingFactory<GenPolynomial<C>> coefficientFactory = first.Ring.CoFac;
        GenPolynomial<C> g = coefficientFactory.FromInteger(1);
        GenPolynomial<C> h = coefficientFactory.FromInteger(1);
        GenPolynomial<C> resultantFactor = PowerPolynomial(coefficientFactory, contentR, degreeFirst)
            .Multiply(PowerPolynomial(coefficientFactory, contentQ, degreeSecond));
        long sign = 1;

        if (degreeSecond == 0 && degreeFirst == 0 && g.Ring.Nvar > 0)
        {
            GenPolynomial<C> t = Resultant(contentR, contentQ);
            return first.Ring.FromInteger(1).Multiply(t);
        }

        while (r.Degree(0) > 0)
        {
            long delta = q.Degree(0) - r.Degree(0);
            if ((q.Degree(0) & 1L) == 1L && (r.Degree(0) & 1L) == 1L)
            {
                sign = -sign;
            }

            GenPolynomial<GenPolynomial<C>> remainder = PolyUtil.RecursiveDensePseudoRemainder(q, r);
            q = r;
            if (remainder.Degree(0) > 0)
            {
                GenPolynomial<C> multiplier = g.Multiply(PowerPolynomial(coefficientFactory, h, delta));
                r = PolyUtil.RecursiveDivide(remainder, multiplier);
                g = q.LeadingBaseCoefficient();
                GenPolynomial<C> gPower = PowerPolynomial(coefficientFactory, g, delta);
                h = PolyUtil.BasePseudoDivide(gPower, PowerPolynomial(coefficientFactory, h, delta - 1));
            }
            else
            {
                r = remainder;
            }
        }

        GenPolynomial<C> z = PowerPolynomial(coefficientFactory, r.LeadingBaseCoefficient(), q.Degree(0));
        h = PolyUtil.BasePseudoDivide(z, PowerPolynomial(coefficientFactory, h, q.Degree(0) - 1));
        z = h.Multiply(resultantFactor);
        if (sign < 0)
        {
            z = z.Negate();
        }

        return first.Ring.FromInteger(1).Multiply(z);
    }

    private static C PowerCoefficient(RingFactory<C> factory, C value, long exponent)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(value);

        if (exponent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exponent), exponent, "Exponent must be non-negative.");
        }

        if (exponent == 0)
        {
            return factory.FromInteger(1);
        }

        return Power<C>.PowerMethod(factory, value, exponent);
    }

    private static GenPolynomial<C> PowerPolynomial(
        RingFactory<GenPolynomial<C>> factory,
        GenPolynomial<C> value,
        long exponent)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(value);

        if (exponent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exponent), exponent, "Exponent must be non-negative.");
        }

        if (exponent == 0)
        {
            return factory.FromInteger(1);
        }

        return Power<GenPolynomial<C>>.PowerMethod(factory, value, exponent);
    }
}
