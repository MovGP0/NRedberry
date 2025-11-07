using System.Collections.Generic;
using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for infinite coefficient fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeInfiniteFieldCharP
/// </remarks>
public class SquarefreeInfiniteFieldCharP<C> : SquarefreeFieldCharP<Quotient<C>>
    where C : GcdRingElem<C>
{
    protected readonly Quotient<C> qone;
    protected readonly Quotient<C> qzero;
    protected readonly SquarefreeAbstract<C> qengine;
    protected readonly QuotientRing<C> quotientRing;

    public SquarefreeInfiniteFieldCharP(RingFactory<Quotient<C>> fac)
        : base(fac)
    {
        if (fac is not QuotientRing<C> quotient)
        {
            throw new ArgumentException("fac must be a QuotientRing instance.", nameof(fac));
        }

        if (fac.IsFinite())
        {
            throw new ArgumentException("fac must represent an infinite field.", nameof(fac));
        }

        quotientRing = quotient;
        qengine = SquarefreeFactory.GetImplementation(quotientRing.Ring);

        GenPolynomial<C> one = quotientRing.Ring.FromInteger(1);
        GenPolynomial<C> zero = quotientRing.Ring.FromInteger(0);
        qone = new Quotient<C>(quotientRing, one);
        qzero = new Quotient<C>(quotientRing, zero);
    }

    public override GenPolynomial<Quotient<C>>? BaseSquarefreePRoot(GenPolynomial<Quotient<C>> polynomial)
    {
        return RootCharacteristic(polynomial);
    }

    public override GenPolynomial<GenPolynomial<Quotient<C>>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<Quotient<C>>> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<Quotient<C>>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for univariate recursive polynomials.", nameof(polynomial));
        }

        BigInteger characteristic = ring.Characteristic();
        if (characteristic.Sign <= 0)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for char p > 0 {ring.CoFac}.", nameof(polynomial));
        }

        long modulus = (long)characteristic;
        GenPolynomial<GenPolynomial<Quotient<C>>> result = new(ring);
        foreach (Monomial<GenPolynomial<Quotient<C>>> term in polynomial)
        {
            long exponent = term.E.GetVal(0);
            if (exponent % modulus != 0)
            {
                return null;
            }

            long adjustedExponent = exponent / modulus;
            GenPolynomial<Quotient<C>>? coefficientRoot = RootCharacteristic(term.C);
            if (coefficientRoot is null)
            {
                return null;
            }

            ExpVector newExponent = ExpVector.Create(1, 0, adjustedExponent);
            result.DoPutToMap(newExponent, coefficientRoot);
        }

        return result;
    }

    public override SortedDictionary<Quotient<C>, long> SquarefreeFactors(Quotient<C> coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);

        SortedDictionary<Quotient<C>, long> factors = new();
        if (coefficient.IsZero())
        {
            return factors;
        }

        if (coefficient.IsOne())
        {
            factors[coefficient] = 1L;
            return factors;
        }

        GenPolynomial<C> numerator = coefficient.Num;
        GenPolynomial<C> denominator = coefficient.Den;
        if (!numerator.IsOne())
        {
            SortedDictionary<GenPolynomial<C>, long> numeratorFactors = qengine.SquarefreeFactors(numerator);
            foreach (KeyValuePair<GenPolynomial<C>, long> entry in numeratorFactors)
            {
                if (entry.Key.IsZero())
                {
                    continue;
                }

                Quotient<C> factor = new(quotientRing, entry.Key);
                factors[factor] = entry.Value;
            }
        }

        if (denominator.IsOne())
        {
            if (factors.Count == 0)
            {
                factors[coefficient] = 1L;
            }

            return factors;
        }

        SortedDictionary<GenPolynomial<C>, long> denominatorFactors = qengine.SquarefreeFactors(denominator);
        GenPolynomial<C> unit = quotientRing.Ring.FromInteger(1);
        foreach (KeyValuePair<GenPolynomial<C>, long> entry in denominatorFactors)
        {
            if (entry.Key.IsZero())
            {
                continue;
            }

            Quotient<C> factor = new(quotientRing, unit, entry.Key);
            factors[factor] = entry.Value;
        }

        if (factors.Count == 0)
        {
            factors[coefficient] = 1L;
        }

        return factors;
    }

    private SortedDictionary<Quotient<C>, long>? RootCharacteristic(Quotient<C> quotient)
    {
        ArgumentNullException.ThrowIfNull(quotient);

        BigInteger characteristic = quotient.Ring.Characteristic();
        if (characteristic.Sign == 0)
        {
            return null;
        }

        SortedDictionary<Quotient<C>, long> root = new();
        if (quotient.IsZero())
        {
            return root;
        }

        if (quotient.IsOne())
        {
            root[quotient] = 1L;
            return root;
        }

        SortedDictionary<Quotient<C>, long> squarefree = SquarefreeFactors(quotient);
        if (squarefree.Count == 0)
        {
            return null;
        }

        long characteristicValue = (long)characteristic;
        long? smallestExponent = null;
        foreach (KeyValuePair<Quotient<C>, long> entry in squarefree)
        {
            Quotient<C> factor = entry.Key;
            if (factor.IsConstant())
            {
                continue;
            }

            long exponent = entry.Value;
            if (exponent % characteristicValue != 0)
            {
                return null;
            }

            smallestExponent = smallestExponent.HasValue
                ? Math.Min(smallestExponent.Value, exponent)
                : exponent;
        }

        long scalingExponent = smallestExponent ?? 1L;
        foreach (KeyValuePair<Quotient<C>, long> entry in squarefree)
        {
            long exponent = entry.Value;
            if (exponent >= scalingExponent)
            {
                root[entry.Key] = exponent / characteristicValue;
            }
            else
            {
                root[entry.Key] = exponent;
            }
        }

        return root;
    }

    private GenPolynomial<Quotient<C>>? RootCharacteristic(GenPolynomial<Quotient<C>> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<Quotient<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            GenPolynomialRing<Quotient<C>> coefficientRing = ring.Contract(1);
            GenPolynomialRing<GenPolynomial<Quotient<C>>> recursiveRing = new(coefficientRing, 1);
            GenPolynomial<GenPolynomial<Quotient<C>>> recursive = PolyUtil.Recursive(recursiveRing, polynomial);
            GenPolynomial<GenPolynomial<Quotient<C>>>? recursiveRoot = RecursiveUnivariateRootCharacteristic(recursive);
            return recursiveRoot is null ? null : PolyUtil.Distribute(ring, recursiveRoot);
        }

        BigInteger characteristic = ring.Characteristic();
        if (characteristic.Sign <= 0)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for ModInteger polynomials {ring.CoFac}.", nameof(polynomial));
        }

        long modulus = (long)characteristic;
        GenPolynomial<Quotient<C>> result = new(ring);
        foreach (Monomial<Quotient<C>> term in polynomial)
        {
            long exponent = term.E.GetVal(0);
            if (exponent % modulus != 0)
            {
                return null;
            }

            long adjustedExponent = exponent / modulus;
            SortedDictionary<Quotient<C>, long>? coefficientRoots = RootCharacteristic(term.C);
            if (coefficientRoots is null)
            {
                return null;
            }

            Quotient<C> coefficient = ring.CoFac.FromInteger(1);
            foreach (KeyValuePair<Quotient<C>, long> entry in coefficientRoots)
            {
                Quotient<C> factor = entry.Key;
                long factorExponent = entry.Value;
                if (factorExponent > 1)
                {
                    factor = Power<Quotient<C>>.PositivePower(factor, factorExponent);
                }

                coefficient = coefficient.Multiply(factor);
            }

            ExpVector newExponent = ExpVector.Create(1, 0, adjustedExponent);
            result.DoPutToMap(newExponent, coefficient);
        }

        return result;
    }
}
