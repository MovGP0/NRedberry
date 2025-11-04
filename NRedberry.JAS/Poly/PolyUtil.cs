using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
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

    public static GenPolynomial<C> FromIntegerCoefficients<C>(GenPolynomialRing<C> ring, GenPolynomial<BigInteger>? polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);

        GenPolynomial<C> result = new (ring);
        if (polynomial is null || polynomial.IsZero())
        {
            return result;
        }

        SortedDictionary<ExpVector, C> destination = result.Terms;
        foreach (KeyValuePair<ExpVector, BigInteger> term in polynomial.Terms)
        {
            C coefficient = ring.CoFac.FromInteger(term.Value.Val);
            destination[term.Key] = coefficient;
        }

        return result;
    }

    public static List<GenPolynomial<C>>? FromIntegerCoefficients<C>(GenPolynomialRing<C> ring, List<GenPolynomial<BigInteger>>? polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        if (polynomials is null)
        {
            return null;
        }

        List<GenPolynomial<C>> result = new (polynomials.Count);
        foreach (GenPolynomial<BigInteger>? polynomial in polynomials)
        {
            result.Add(FromIntegerCoefficients(ring, polynomial));
        }

        return result;
    }

    public static GenPolynomial<GenPolynomial<C>> FromAlgebraicCoefficients<C>(GenPolynomialRing<GenPolynomial<C>> ring, GenPolynomial<AlgebraicNumber<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        return Map(ring, polynomial, new AlgToPoly<C>());
    }

    public static GenPolynomial<AlgebraicNumber<C>> ConvertToAlgebraicCoefficients<C>(GenPolynomialRing<AlgebraicNumber<C>> ring, GenPolynomial<C> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        AlgebraicNumberRing<C> algebraicRing = (AlgebraicNumberRing<C>)ring.CoFac;
        return Map(ring, polynomial, new CoeffToAlg<C>(algebraicRing));
    }

    public static GenPolynomial<Complex<C>> ComplexFromAlgebraic<C>(GenPolynomialRing<Complex<C>> ring, GenPolynomial<AlgebraicNumber<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        ComplexRing<C> complexRing = (ComplexRing<C>)ring.CoFac;
        return Map(ring, polynomial, new AlgebToCompl<C>(complexRing));
    }

    public static GenPolynomial<AlgebraicNumber<C>> AlgebraicFromComplex<C>(GenPolynomialRing<AlgebraicNumber<C>> ring, GenPolynomial<Complex<C>> polynomial)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);

        AlgebraicNumberRing<C> algebraicRing = (AlgebraicNumberRing<C>)ring.CoFac;
        return Map(ring, polynomial, new ComplToAlgeb<C>(algebraicRing));
    }

    public static GenPolynomial<C> ChineseRemainder<C>(GenPolynomialRing<C> ring, GenPolynomial<C> first, C modulusInverse, GenPolynomial<C> second)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(modulusInverse);
        ArgumentNullException.ThrowIfNull(second);

        ModularRingFactory<C> modularFactory = (ModularRingFactory<C>)ring.CoFac;
        GenPolynomial<C> result = new (ring);
        GenPolynomial<C> firstCopy = first.Clone();

        SortedDictionary<ExpVector, C> firstTerms = firstCopy.Terms;
        SortedDictionary<ExpVector, C> secondTerms = second.Terms;
        SortedDictionary<ExpVector, C> resultTerms = result.Terms;

        C zeroFirst = first.Ring.CoFac.FromInteger(0);
        C zeroSecond = second.Ring.CoFac.FromInteger(0);

        foreach (KeyValuePair<ExpVector, C> term in secondTerms)
        {
            ExpVector exponent = term.Key;
            C secondCoefficient = term.Value;
            if (firstTerms.TryGetValue(exponent, out C firstCoefficient))
            {
                firstTerms.Remove(exponent);
                C coefficient = modularFactory.ChineseRemainder(firstCoefficient, modulusInverse, secondCoefficient);
                if (!coefficient.IsZero())
                {
                    resultTerms[exponent] = coefficient;
                }
            }
            else
            {
                C coefficient = modularFactory.ChineseRemainder(zeroFirst, modulusInverse, secondCoefficient);
                if (!coefficient.IsZero())
                {
                    resultTerms[exponent] = coefficient;
                }
            }
        }

        foreach (KeyValuePair<ExpVector, C> term in firstTerms)
        {
            C coefficient = modularFactory.ChineseRemainder(term.Value, modulusInverse, zeroSecond);
            if (!coefficient.IsZero())
            {
                resultTerms[term.Key] = coefficient;
            }
        }

        return result;
    }

    public static GenPolynomial<GenPolynomial<C>>? Monic<C>(GenPolynomial<GenPolynomial<C>>? polynomial)
        where C : RingElem<C>
    {
        if (polynomial is null || polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomial<C> leadingCoefficient = polynomial.LeadingBaseCoefficient();
        C head = leadingCoefficient.LeadingBaseCoefficient();
        if (!head.IsUnit())
        {
            return polynomial;
        }

        C inverse = head.Inverse();
        GenPolynomial<C> unit = polynomial.Ring.CoFac.FromInteger(1);
        GenPolynomial<C> multiplier = unit.Multiply(inverse);
        return polynomial.Multiply(multiplier);
    }

    public static List<GenPolynomial<C>>? Monic<C>(List<GenPolynomial<C>>? polynomials)
        where C : RingElem<C>
    {
        if (polynomials is null)
        {
            return null;
        }

        List<GenPolynomial<C>> result = new (polynomials.Count);
        foreach (GenPolynomial<C>? polynomial in polynomials)
        {
            result.Add(polynomial is null ? null! : polynomial.Monic());
        }

        return result;
    }

    public static List<ExpVector?>? LeadingExpVector<C>(List<GenPolynomial<C>>? polynomials)
        where C : RingElem<C>
    {
        if (polynomials is null)
        {
            return null;
        }

        List<ExpVector?> result = new (polynomials.Count);
        foreach (GenPolynomial<C>? polynomial in polynomials)
        {
            result.Add(polynomial?.LeadingExpVector());
        }

        return result;
    }

    public static GenPolynomial<C> BaseSparsePseudoRemainder<C>(GenPolynomial<C> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        if (divisor.IsOne())
        {
            return new GenPolynomial<C>(divisor.Ring);
        }

        C leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<C> remainder = polynomial;

        while (!remainder.IsZero())
        {
            ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
            if (!leadingRemainderExponent.MultipleOf(leadingDivisorExponent))
            {
                break;
            }

            C leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
            C remainderModulo = leadingRemainderCoefficient.Remainder(leadingDivisorCoefficient);
            if (remainderModulo.IsZero())
            {
                C quotientCoefficient = leadingRemainderCoefficient.Divide(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return remainder;
    }

    public static GenPolynomial<C> BaseDensePseudoRemainder<C>(GenPolynomial<C> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        if (divisor.Degree() <= 0)
        {
            return new GenPolynomial<C>(divisor.Ring);
        }

        long dividendDegree = polynomial.Degree(0);
        long divisorDegree = divisor.Degree(0);
        C leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<C> remainder = polynomial;

        for (long i = dividendDegree; i >= divisorDegree; i--)
        {
            if (remainder.IsZero())
            {
                return remainder;
            }

            long remainderDegree = remainder.Degree(0);
            if (i == remainderDegree)
            {
                ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
                C leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
                ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
            }
        }

        return remainder;
    }

    public static GenPolynomial<C> BasePseudoDivide<C>(GenPolynomial<C> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero() || divisor.IsOne())
        {
            return polynomial;
        }

        C leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<C> remainder = polynomial;
        GenPolynomial<C> quotient = new (divisor.Ring);

        while (!remainder.IsZero())
        {
            ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
            if (!leadingRemainderExponent.MultipleOf(leadingDivisorExponent))
            {
                break;
            }

            C leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
            C remainderModulo = leadingRemainderCoefficient.Remainder(leadingDivisorCoefficient);
            if (remainderModulo.IsZero())
            {
                C quotientCoefficient = leadingRemainderCoefficient.Divide(leadingDivisorCoefficient);
                quotient = quotient.Sum(quotientCoefficient, exponentDifference);
                GenPolynomial<C> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                quotient = quotient.Multiply(leadingDivisorCoefficient);
                quotient = quotient.Sum(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return quotient;
    }

    public static GenPolynomial<GenPolynomial<C>> RecursiveDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"division by zero {polynomial}, {divisor}");
        }

        if (polynomial.IsZero() || divisor.IsOne())
        {
            return polynomial;
        }

        GenPolynomial<GenPolynomial<C>> result = new (polynomial.Ring);
        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            GenPolynomial<C> quotient = BasePseudoDivide(term.Value, divisor);
            if (quotient.IsZero())
            {
                throw new InvalidOperationException("Pseudo division produced zero coefficient unexpectedly.");
            }

            resultTerms[term.Key] = quotient;
        }

        return result;
    }

    public static GenPolynomial<GenPolynomial<C>> BaseRecursiveDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, C divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"division by zero {polynomial}, {divisor}");
        }

        if (polynomial.IsZero() || divisor.IsOne())
        {
            return polynomial;
        }

        GenPolynomial<GenPolynomial<C>> result = new (polynomial.Ring);
        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            GenPolynomial<C> quotient = CoefficientBasePseudoDivide(term.Value, divisor);
            if (quotient.IsZero())
            {
                throw new InvalidOperationException("Coefficient pseudo division produced zero coefficient unexpectedly.");
            }

            resultTerms[term.Key] = quotient;
        }

        return result;
    }

    public static GenPolynomial<GenPolynomial<C>> RecursivePseudoRemainder<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
        where C : RingElem<C>
    {
        return RecursiveSparsePseudoRemainder(polynomial, divisor);
    }

    public static GenPolynomial<GenPolynomial<C>> RecursiveSparsePseudoRemainder<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(divisor);
        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial is null || polynomial.IsZero())
        {
            return polynomial;
        }

        if (divisor.IsOne())
        {
            return new GenPolynomial<GenPolynomial<C>>(polynomial.Ring);
        }

        GenPolynomial<C> leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<GenPolynomial<C>> remainder = polynomial;

        while (!remainder.IsZero())
        {
            ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
            if (!leadingRemainderExponent.MultipleOf(leadingDivisorExponent))
            {
                break;
            }

            GenPolynomial<C> leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
            GenPolynomial<C> remainderModulo = BaseSparsePseudoRemainder(leadingRemainderCoefficient, leadingDivisorCoefficient);

            if (remainderModulo.IsZero())
            {
                GenPolynomial<C> quotientCoefficient = BasePseudoDivide(leadingRemainderCoefficient, leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return remainder;
    }

    public static GenPolynomial<GenPolynomial<C>> RecursiveDensePseudoRemainder<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        if (divisor.Degree() <= 0)
        {
            return new GenPolynomial<GenPolynomial<C>>(polynomial.Ring);
        }

        long dividendDegree = polynomial.Degree(0);
        long divisorDegree = divisor.Degree(0);
        GenPolynomial<C> leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<GenPolynomial<C>> remainder = polynomial;

        for (long i = dividendDegree; i >= divisorDegree; i--)
        {
            if (remainder.IsZero())
            {
                return remainder;
            }

            long remainderDegree = remainder.Degree(0);
            if (i == remainderDegree)
            {
                ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
                GenPolynomial<C> leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
                ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
            }
        }

        return remainder;
    }

    public static GenPolynomial<GenPolynomial<C>> RecursivePseudoDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomial<C> leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<GenPolynomial<C>> remainder = polynomial;
        GenPolynomial<GenPolynomial<C>> quotient = new (divisor.Ring);

        while (!remainder.IsZero())
        {
            ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
            if (!leadingRemainderExponent.MultipleOf(leadingDivisorExponent))
            {
                break;
            }

            GenPolynomial<C> leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
            GenPolynomial<C> remainderModulo = BaseSparsePseudoRemainder(leadingRemainderCoefficient, leadingDivisorCoefficient);

            if (remainderModulo.IsZero() && !leadingDivisorCoefficient.IsConstant())
            {
                GenPolynomial<C> quotientCoefficient = BasePseudoDivide(leadingRemainderCoefficient, leadingDivisorCoefficient);
                quotient = quotient.Sum(quotientCoefficient, exponentDifference);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                quotient = quotient.Multiply(leadingDivisorCoefficient);
                quotient = quotient.Sum(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return quotient;
    }

    public static GenPolynomial<GenPolynomial<C>> CoefficientPseudoDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomial<GenPolynomial<C>> result = new (polynomial.Ring);
        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            GenPolynomial<C> quotient = BasePseudoDivide(term.Value, divisor);
            if (!quotient.IsZero())
            {
                resultTerms[term.Key] = quotient;
            }
        }

        return result;
    }

    public static GenPolynomial<C> CoefficientBasePseudoDivide<C>(GenPolynomial<C> polynomial, C divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomial<C> result = new (polynomial.Ring);
        SortedDictionary<ExpVector, C> resultTerms = result.Terms;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            C quotient = term.Value.Divide(divisor);
            if (!quotient.IsZero())
            {
                resultTerms[term.Key] = quotient;
            }
        }

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

    public static GenPolynomial<D> Map<C, D>(GenPolynomialRing<D> ring, GenPolynomial<C> polynomial, UnaryFunctor<C, D> functor)
        where C : RingElem<C>
        where D : RingElem<D>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(functor);

        GenPolynomial<D> result = new (ring);
        SortedDictionary<ExpVector, D> destination = result.Terms;
        foreach (Monomial<C> monomial in polynomial)
        {
            D mapped = functor.Eval(monomial.C);
            if (mapped is not null && !mapped.IsZero())
            {
                destination[monomial.E] = mapped;
            }
        }

        return result;
    }

    private sealed class AlgToPoly<C> : UnaryFunctor<AlgebraicNumber<C>, GenPolynomial<C>> where C : GcdRingElem<C>
    {
        public GenPolynomial<C> Eval(AlgebraicNumber<C> value)
        {
            return value is null ? null! : value.Val;
        }
    }

    private sealed class CoeffToAlg<C> : UnaryFunctor<C, AlgebraicNumber<C>> where C : GcdRingElem<C>
    {
        private readonly AlgebraicNumberRing<C> _ring;
        private readonly GenPolynomial<C> _zeroPolynomial;

        public CoeffToAlg(AlgebraicNumberRing<C> ring)
        {
            ArgumentNullException.ThrowIfNull(ring);
            _ring = ring;
            GenPolynomialRing<C> polynomialRing = ring.Ring;
            _zeroPolynomial = new GenPolynomial<C>(polynomialRing);
        }

        public AlgebraicNumber<C> Eval(C coefficient)
        {
            if (coefficient is null)
            {
                return _ring.GetZeroElement();
            }

            GenPolynomial<C> polynomial = _zeroPolynomial.Sum(coefficient);
            return new AlgebraicNumber<C>(_ring, polynomial);
        }
    }

    private sealed class AlgebToCompl<C> : UnaryFunctor<AlgebraicNumber<C>, Complex<C>> where C : GcdRingElem<C>
    {
        private readonly ComplexRing<C> _ring;

        public AlgebToCompl(ComplexRing<C> ring)
        {
            ArgumentNullException.ThrowIfNull(ring);
            _ring = ring;
        }

        public Complex<C> Eval(AlgebraicNumber<C> value)
        {
            if (value is null || value.IsZero())
            {
                return _ring.Zero;
            }

            if (value.IsOne())
            {
                return _ring.One;
            }

            GenPolynomial<C> polynomial = value.Val;
            C real = _ring.Ring.FromInteger(0);
            C imaginary = _ring.Ring.FromInteger(0);

            foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
            {
                long exponent = term.Key.GetVal(0);
                if (exponent == 0)
                {
                    real = term.Value;
                }
                else
                {
                    imaginary = term.Value;
                }
            }

            return new Complex<C>(_ring, real, imaginary);
        }
    }

    private sealed class ComplToAlgeb<C> : UnaryFunctor<Complex<C>, AlgebraicNumber<C>> where C : GcdRingElem<C>
    {
        private readonly AlgebraicNumberRing<C> _ring;
        private readonly GenPolynomialRing<C> _polynomialRing;

        public ComplToAlgeb(AlgebraicNumberRing<C> ring)
        {
            ArgumentNullException.ThrowIfNull(ring);
            _ring = ring;
            _polynomialRing = ring.Ring;
        }

        public AlgebraicNumber<C> Eval(Complex<C> value)
        {
            if (value is null)
            {
                return _ring.GetZeroElement();
            }

            GenPolynomial<C> polynomial = new (_polynomialRing, value.Re);
            if (!value.Im.IsZero())
            {
                ExpVector exponent = ExpVector.Create(_polynomialRing.Nvar, _polynomialRing.Nvar - 1, 1);
                polynomial = polynomial.Sum(value.Im, exponent);
            }

            return new AlgebraicNumber<C>(_ring, polynomial);
        }
    }
}
