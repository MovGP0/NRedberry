using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorAbstract
/// </remarks>
public abstract class GreatestCommonDivisorAbstract<C> : GreatestCommonDivisor<C>
    where C : GcdRingElem<C>
{
    public override string ToString()
    {
        return GetType().FullName ?? GetType().Name;
    }

    public abstract GenPolynomial<C> BaseGcd(GenPolynomial<C> first, GenPolynomial<C> second);

    public abstract GenPolynomial<GenPolynomial<C>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<C>> first,
        GenPolynomial<GenPolynomial<C>> second);

    public virtual GenPolynomial<C> BaseResultant(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        throw new NotSupportedException("Base resultant not implemented.");
    }

    public virtual GenPolynomial<GenPolynomial<C>> RecursiveUnivariateResultant(
        GenPolynomial<GenPolynomial<C>> first,
        GenPolynomial<GenPolynomial<C>> second)
    {
        throw new NotSupportedException("Recursive univariate resultant not implemented.");
    }

    public C BaseContent(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial.Ring.GetZeroCoefficient();
        }

        var hasResult = false;
        C result = default!;
        foreach (C coefficient in polynomial.Terms.Values)
        {
            result = hasResult ? result.Gcd(coefficient) : coefficient;
            hasResult = true;

            if (result.IsOne())
            {
                return result;
            }
        }

        if (!hasResult)
        {
            return polynomial.Ring.GetZeroCoefficient();
        }

        if (result.Signum() < 0)
        {
            result = result.Negate();
        }

        return result;
    }

    public GenPolynomial<C> BasePrimitivePart(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        C content = BaseContent(polynomial);
        return content.IsOne() ? polynomial : polynomial.Divide(content);
    }

    public GenPolynomial<C> RecursiveContent(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial.Ring.GetZeroCoefficient();
        }

        GenPolynomial<C>? result = null;
        foreach (GenPolynomial<C> coefficient in polynomial.Terms.Values)
        {
            result = result is null ? coefficient : Gcd(result, coefficient);
            if (result.IsOne())
            {
                return result;
            }
        }

        return result!.Abs();
    }

    public GenPolynomial<GenPolynomial<C>> RecursivePrimitivePart(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomial<C> content = RecursiveContent(polynomial);
        return content.IsOne() ? polynomial : PolyUtil.RecursiveDivide(polynomial, content);
    }

    public C BaseRecursiveContent(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            var coefficientRing = (GenPolynomialRing<C>)polynomial.Ring.CoFac;
            return coefficientRing.CoFac.FromInteger(0);
        }

        var hasResult = false;
        C result = default!;
        foreach (GenPolynomial<C> coefficient in polynomial.Terms.Values)
        {
            C coefficientContent = BaseContent(coefficient);
            result = hasResult ? Gcd(result, coefficientContent) : coefficientContent;
            hasResult = true;

            if (result.IsOne())
            {
                return result;
            }
        }

        if (result.Signum() < 0)
        {
            result = result.Negate();
        }

        return result;
    }

    public GenPolynomial<GenPolynomial<C>> BaseRecursivePrimitivePart(GenPolynomial<GenPolynomial<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        C content = BaseRecursiveContent(polynomial);
        return content.IsOne() ? polynomial : PolyUtil.BaseRecursiveDivide(polynomial, content);
    }

    public GenPolynomial<C> Content(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar <= 1)
        {
            throw new ArgumentException("Use BaseContent for univariate polynomials.", nameof(polynomial));
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new(coefficientRing, 1);
        GenPolynomial<GenPolynomial<C>> recursivePolynomial = PolyUtil.Recursive(recursiveRing, polynomial);
        return RecursiveContent(recursivePolynomial);
    }

    public GenPolynomial<C> PrimitivePart(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar <= 1)
        {
            return BasePrimitivePart(polynomial);
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new(coefficientRing, 1);
        GenPolynomial<GenPolynomial<C>> recursivePolynomial = PolyUtil.Recursive(recursiveRing, polynomial);
        GenPolynomial<GenPolynomial<C>> primitive = RecursivePrimitivePart(recursivePolynomial);
        return PolyUtil.Distribute(ring, primitive);
    }

    public GenPolynomial<C> Divide(GenPolynomial<C> dividend, C divisor)
    {
        if (divisor.IsZero())
        {
            throw new ArgumentException("Division by zero.", nameof(divisor));
        }

        if (dividend.IsZero())
        {
            return dividend;
        }

        return dividend.Divide(divisor);
    }

    public C Gcd(C first, C second)
    {
        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        return first.Gcd(second);
    }

    public virtual GenPolynomial<C> Gcd(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        if (second.IsZero())
        {
            return first;
        }

        if (first.IsZero())
        {
            return second;
        }

        GenPolynomialRing<C> ring = first.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseGcd(first, second);
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing;
        string[]? variables = ring.GetVars();
        if (variables?.Length > 0)
        {
            recursiveRing = new GenPolynomialRing<GenPolynomial<C>>(coefficientRing, 1, new[] { variables[ring.Nvar - 1] });
        }
        else
        {
            recursiveRing = new GenPolynomialRing<GenPolynomial<C>>(coefficientRing, 1);
        }

        GenPolynomial<GenPolynomial<C>> recursiveFirst = PolyUtil.Recursive(recursiveRing, first);
        GenPolynomial<GenPolynomial<C>> recursiveSecond = PolyUtil.Recursive(recursiveRing, second);
        GenPolynomial<GenPolynomial<C>> recursiveGcd = RecursiveUnivariateGcd(recursiveFirst, recursiveSecond);
        return PolyUtil.Distribute(ring, recursiveGcd);
    }

    public virtual GenPolynomial<C> Lcm(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        GenPolynomial<C> gcd = Gcd(first, second);
        GenPolynomial<C> product = first.Multiply(second);
        return PolyUtil.BasePseudoDivide(product, gcd);
    }

    public virtual GenPolynomial<C> Resultant(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        if (second.IsZero())
        {
            return second;
        }

        if (first.IsZero())
        {
            return first;
        }

        GenPolynomialRing<C> ring = first.Ring;
        if (ring.Nvar <= 1)
        {
            return BaseResultant(first, second);
        }

        GenPolynomialRing<C> coefficientRing = ring.Contract(1);
        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new(coefficientRing, 1);

        GenPolynomial<GenPolynomial<C>> recursiveFirst = PolyUtil.Recursive(recursiveRing, first);
        GenPolynomial<GenPolynomial<C>> recursiveSecond = PolyUtil.Recursive(recursiveRing, second);
        GenPolynomial<GenPolynomial<C>> recursiveResultant = RecursiveUnivariateResultant(recursiveFirst, recursiveSecond);
        return PolyUtil.Distribute(ring, recursiveResultant);
    }

    public virtual GenPolynomial<GenPolynomial<C>> RecursiveResultant(
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

        GenPolynomialRing<GenPolynomial<C>> recursiveRing = first.Ring;
        var coefficientRing = (GenPolynomialRing<C>)recursiveRing.CoFac;

        string[]? variables = recursiveRing.GetVars();
        GenPolynomialRing<C> distributedRing = variables is not null
            ? coefficientRing.Extend(variables)
            : coefficientRing.Extend(recursiveRing.Nvar);

        GenPolynomial<C> distributedFirst = PolyUtil.Distribute(distributedRing, first);
        GenPolynomial<C> distributedSecond = PolyUtil.Distribute(distributedRing, second);
        GenPolynomial<C> resultant = Resultant(distributedFirst, distributedSecond);
        return PolyUtil.Recursive(recursiveRing, resultant);
    }

    public virtual List<GenPolynomial<C>> CoPrime(List<GenPolynomial<C>> polynomials)
    {
        if (polynomials is null || polynomials.Count == 0)
        {
            return polynomials;
        }

        List<GenPolynomial<C>> result = new(polynomials.Count);
        GenPolynomial<C> current = polynomials[0];
        if (!current.IsZero() && !current.IsConstant())
        {
            for (int i = 1; i < polynomials.Count; i++)
            {
                GenPolynomial<C> candidate = polynomials[i];
                GenPolynomial<C> gcd = Gcd(current, candidate).Abs();
                if (!gcd.IsOne())
                {
                    current = PolyUtil.BasePseudoDivide(current, gcd);
                    candidate = PolyUtil.BasePseudoDivide(candidate, gcd);
                    GenPolynomial<C> gcdPart = Gcd(current, gcd).Abs();
                    while (!gcdPart.IsOne())
                    {
                        current = PolyUtil.BasePseudoDivide(current, gcdPart);
                        gcd = PolyUtil.BasePseudoDivide(gcd, gcdPart);
                        result.Add(gcd);
                        gcd = gcdPart;
                        gcdPart = Gcd(current, gcdPart).Abs();
                    }

                    if (!gcd.IsZero() && !gcd.IsConstant())
                    {
                        result.Add(gcd);
                    }
                }

                if (!candidate.IsZero() && !candidate.IsConstant())
                {
                    result.Add(candidate);
                }
            }
        }
        else if (polynomials.Count > 1)
        {
            result.AddRange(polynomials.GetRange(1, polynomials.Count - 1));
        }

        result = CoPrime(result);
        if (!current.IsZero() && !current.IsConstant())
        {
            result.Add(current.Abs());
        }

        return result;
    }

    public virtual GenPolynomial<C> Gcd(List<GenPolynomial<C>> polynomials)
    {
        if (polynomials is null || polynomials.Count == 0)
        {
            throw new ArgumentException("Polynomial list must not be empty.", nameof(polynomials));
        }

        GenPolynomial<C> gcd = polynomials[0];
        for (int i = 1; i < polynomials.Count; i++)
        {
            gcd = Gcd(gcd, polynomials[i]);
        }

        return gcd;
    }
}
