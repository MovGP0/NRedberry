using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Polynomial utilities, conversion between different representations and properties of polynomials.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolyUtil
/// </remarks>
public static class PolyUtil
{
    public static GenPolynomial<GenPolynomial<C>> Recursive<C>(GenPolynomialRing<GenPolynomial<C>> recursiveRing, GenPolynomial<C> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(recursiveRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomial<GenPolynomial<C>> result = GenPolynomialRing<GenPolynomial<C>>.Zero.Clone();
        if (polynomial.IsZero())
        {
            return result;
        }

        int split = recursiveRing.Nvar;
        GenPolynomial<C> zeroCoefficient = GenPolynomialRing<C>.Zero;

        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;
        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector exponent = term.Key;
            C coefficient = term.Value;

            ExpVector head = exponent.Contract(0, split);
            ExpVector tail = exponent.Contract(split, exponent.Length() - split);

            if (!resultTerms.TryGetValue(head, out GenPolynomial<C>? existing))
            {
                existing = zeroCoefficient;
            }

            resultTerms[head] = existing.Sum(coefficient, tail);
        }

        return result;
    }

    public static GenPolynomial<C> Distribute<C>(GenPolynomialRing<C> distributedRing, GenPolynomial<GenPolynomial<C>> recursivePolynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(distributedRing);
        ArgumentNullException.ThrowIfNull(recursivePolynomial);

        GenPolynomial<C> result = GenPolynomialRing<C>.Zero.Clone();
        if (recursivePolynomial.IsZero())
        {
            return result;
        }

        SortedDictionary<ExpVector, C> resultTerms = result.Terms;
        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> outer in recursivePolynomial.Terms)
        {
            ExpVector head = outer.Key;
            GenPolynomial<C> inner = outer.Value;

            foreach (KeyValuePair<ExpVector, C> innerTerm in inner.Terms)
            {
                ExpVector exponent = head.Combine(innerTerm.Key);
                if (resultTerms.ContainsKey(exponent))
                {
                    throw new InvalidOperationException("Duplicate exponent encountered during distribution.");
                }

                resultTerms[exponent] = innerTerm.Value;
            }
        }

        return result;
    }

    public static List<GenPolynomial<GenPolynomial<C>>> Recursive<C>(GenPolynomialRing<GenPolynomial<C>> recursiveRing, List<GenPolynomial<C>> polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(recursiveRing);
        ArgumentNullException.ThrowIfNull(polynomials);

        return polynomials.Select(p => Recursive(recursiveRing, p)).ToList();
    }

    public static GenPolynomial<BigInteger> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> resultRing, GenPolynomial<C> polynomial)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        GenPolynomial<BigInteger> result = GenPolynomialRing<BigInteger>.Zero.Clone();
        SortedDictionary<ExpVector, BigInteger> terms = result.Terms;
        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            terms[term.Key] = term.Value.GetSymmetricInteger();
        }

        return result;
    }

    public static List<GenPolynomial<BigInteger>> IntegerFromModularCoefficients<C>(GenPolynomialRing<BigInteger> resultRing, List<GenPolynomial<C>> polynomials)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomials);

        return polynomials.Select(p => IntegerFromModularCoefficients(resultRing, p)).ToList();
    }

    public static GenPolynomial<BigInteger> IntegerFromRationalCoefficients(GenPolynomialRing<BigInteger> resultRing, GenPolynomial<BigRational> polynomial)
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return GenPolynomialRing<BigInteger>.Zero.Clone();
        }

        System.Numerics.BigInteger lcm = System.Numerics.BigInteger.Zero;
        foreach (BigRational coefficient in polynomial.Terms.Values)
        {
            System.Numerics.BigInteger denominator = coefficient.Den;
            if (denominator.IsZero)
            {
                continue;
            }

            if (lcm.IsZero)
            {
                lcm = denominator;
            }
            else
            {
                System.Numerics.BigInteger gcd = System.Numerics.BigInteger.GreatestCommonDivisor(lcm, denominator);
                lcm = lcm / gcd * denominator;
            }
        }

        if (lcm.IsZero)
        {
            return GenPolynomialRing<BigInteger>.Zero.Clone();
        }

        GenPolynomial<BigInteger> result = GenPolynomialRing<BigInteger>.Zero.Clone();
        SortedDictionary<ExpVector, BigInteger> terms = result.Terms;
        foreach (KeyValuePair<ExpVector, BigRational> term in polynomial.Terms)
        {
            BigRational coefficient = term.Value;
            System.Numerics.BigInteger numerator = coefficient.Num;
            System.Numerics.BigInteger denominator = coefficient.Den;
            System.Numerics.BigInteger value = numerator * (lcm / denominator);
            terms[term.Key] = new BigInteger(value);
        }

        return result;
    }

    public static object[] IntegerFromRationalCoefficientsFactor(GenPolynomialRing<BigInteger> resultRing, GenPolynomial<BigRational> polynomial)
    {
        ArgumentNullException.ThrowIfNull(resultRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        object[] result = new object[3];
        if (polynomial.IsZero())
        {
            result[0] = System.Numerics.BigInteger.One;
            result[1] = System.Numerics.BigInteger.Zero;
            result[2] = GenPolynomialRing<BigInteger>.Zero.Clone();
            return result;
        }

        System.Numerics.BigInteger? gcd = null;
        System.Numerics.BigInteger? lcm = null;
        int signLcm = 0;
        int signGcd = 0;

        foreach (BigRational coefficient in polynomial.Terms.Values)
        {
            System.Numerics.BigInteger numerator = coefficient.Num;
            System.Numerics.BigInteger denominator = coefficient.Den;

            if (lcm is null)
            {
                lcm = denominator;
                signLcm = denominator.Sign;
            }
            else
            {
                System.Numerics.BigInteger current = lcm.Value;
                System.Numerics.BigInteger d = System.Numerics.BigInteger.GreatestCommonDivisor(current, denominator);
                lcm = current / d * denominator;
            }

            if (gcd is null)
            {
                gcd = numerator;
                signGcd = numerator.Sign;
            }
            else
            {
                gcd = System.Numerics.BigInteger.GreatestCommonDivisor(gcd.Value, numerator);
            }
        }

        System.Numerics.BigInteger gcdValue = gcd ?? System.Numerics.BigInteger.One;
        System.Numerics.BigInteger lcmValue = lcm ?? System.Numerics.BigInteger.Zero;

        if (signLcm < 0)
        {
            lcmValue = System.Numerics.BigInteger.Negate(lcmValue);
        }

        if (signGcd < 0)
        {
            gcdValue = System.Numerics.BigInteger.Negate(gcdValue);
        }

        GenPolynomial<BigInteger> converted = GenPolynomialRing<BigInteger>.Zero.Clone();
        SortedDictionary<ExpVector, BigInteger> terms = converted.Terms;
        foreach (KeyValuePair<ExpVector, BigRational> term in polynomial.Terms)
        {
            BigRational coefficient = term.Value;
            System.Numerics.BigInteger numerator = coefficient.Num / gcdValue;
            System.Numerics.BigInteger denominator = coefficient.Den;
            System.Numerics.BigInteger value = numerator * (lcmValue / denominator);
            terms[term.Key] = new BigInteger(value);
        }

        result[0] = gcdValue;
        result[1] = lcmValue;
        result[2] = converted;
        return result;
    }

    public static C EvaluateMain<C>(RingFactory<C> coefficientFactory, GenPolynomial<C> polynomial, C value)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(coefficientFactory);
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return coefficientFactory.FromInteger(0);
        }

        if (polynomial.Ring.Nvar != 1)
        {
            throw new ArgumentException("evaluateMain requires a univariate polynomial.", nameof(polynomial));
        }

        if (value is null || value.IsZero())
        {
            return GetTrailingCoefficient(polynomial);
        }

        C? result = default;
        long previousExponent = -1;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector exponent = term.Key;
            long currentExponent = exponent.GetVal(0);
            if (result is null)
            {
                result = term.Value;
            }
            else
            {
                for (long i = currentExponent; i < previousExponent; i++)
                {
                    result = result.Multiply(value);
                }

                result = result.Sum(term.Value);
            }

            previousExponent = currentExponent;
        }

        if (result is null)
        {
            return coefficientFactory.FromInteger(0);
        }

        for (long i = 0; i < previousExponent; i++)
        {
            result = result.Multiply(value);
        }

        return result;
    }

    public static GenPolynomial<C> BaseDeriviative<C>(GenPolynomial<C> polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials supported.", nameof(polynomial));
        }

        RingFactory<C> coefficientFactory = ring.CoFac;
        GenPolynomial<C> derivative = GenPolynomialRing<C>.Zero.Clone();
        SortedDictionary<ExpVector, C> derivativeTerms = derivative.Terms;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector exponent = term.Key;
            long power = exponent.GetVal(0);
            if (power <= 0)
            {
                continue;
            }

            C factor = coefficientFactory.FromInteger(power);
            C value = term.Value.Multiply(factor);
            if (value.IsZero())
            {
                continue;
            }

            ExpVector newExponent = ExpVector.Create(1, 0, power - 1);
            derivativeTerms[newExponent] = value;
        }

        return derivative;
    }

    private static C GetTrailingCoefficient<C>(GenPolynomial<C> polynomial)
        where C : RingElem<C>
    {
        if (polynomial.Terms.Count == 0)
        {
            return polynomial.Ring.CoFac.FromInteger(0);
        }

        return polynomial.Terms.Last().Value;
    }
}
