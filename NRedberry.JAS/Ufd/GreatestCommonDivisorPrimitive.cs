using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with primitive polynomial remainder sequence.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorPrimitive
/// </remarks>
public class GreatestCommonDivisorPrimitive<C> : GreatestCommonDivisorAbstract<C>
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
        C factor = Gcd(contentR, contentQ);
        r = Divide(r, contentR);
        q = Divide(q, contentQ);
        if (r.IsOne())
        {
            return r.Multiply(factor);
        }

        if (q.IsOne())
        {
            return q.Multiply(factor);
        }

        while (!r.IsZero())
        {
            GenPolynomial<C> remainder = PolyUtil.BaseSparsePseudoRemainder(q, r);
            q = r;
            r = BasePrimitivePart(remainder);
        }

        return q.Multiply(factor).Abs();
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
        GenPolynomial<C> factor = Gcd(contentR, contentQ);
        r = PolyUtil.RecursiveDivide(r, contentR);
        q = PolyUtil.RecursiveDivide(q, contentQ);
        if (r.IsOne())
        {
            return r.Multiply(factor);
        }

        if (q.IsOne())
        {
            return q.Multiply(factor);
        }

        while (!r.IsZero())
        {
            GenPolynomial<GenPolynomial<C>> remainder = PolyUtil.RecursivePseudoRemainder(q, r);
            q = r;
            r = RecursivePrimitivePart(remainder);
        }

        return q.Abs().Multiply(factor);
    }
}
