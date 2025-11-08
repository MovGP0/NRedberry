using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition for finite coefficient fields of characteristic p.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.SquarefreeFiniteFieldCharP
/// </remarks>
public class SquarefreeFiniteFieldCharP<C> : SquarefreeFieldCharP<C> where C : GcdRingElem<C>
{
    public SquarefreeFiniteFieldCharP(RingFactory<C> fac)
        : base(fac)
    {
        if (!fac.IsFinite())
        {
            throw new ArgumentException("fac must be finite", nameof(fac));
        }
    }

    public SortedDictionary<C, long> RootCharacteristic(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        SortedDictionary<C, long> root = new();
        if (!coefficient.IsZero())
        {
            root[coefficient] = 1L;
        }

        return root;
    }

    public C CoeffRootCharacteristic(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        if (coefficient.IsZero())
        {
            return coefficient;
        }

        if (TryGetAlgebraicExtensionFactory(out object algebraicFactory))
        {
            dynamic ring = algebraicFactory;
            long degree = (long)ring.TotalExtensionDegree();
            if (degree <= 1)
            {
                return coefficient;
            }

            Arith.BigInteger characteristic = new(ring.Characteristic());
            Arith.BigInteger exponent = Power<Arith.BigInteger>.PositivePower(characteristic, degree - 1);
            long exponentValue = exponent.LongValue();
            return Power<C>.PositivePower(coefficient, exponentValue);
        }

        if (TryGetQuotientExtensionFactory(out _))
        {
            throw new NotSupportedException("Characteristic roots for quotient extensions are not implemented.");
        }

        return coefficient;
    }

    public SortedDictionary<GenPolynomial<C>, long>? RootCharacteristic(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);

        System.Numerics.BigInteger characteristic = polynomial.Ring.Characteristic();
        if (characteristic.Sign == 0)
        {
            return null;
        }

        SortedDictionary<GenPolynomial<C>, long> root = new();
        if (polynomial.IsZero())
        {
            return root;
        }

        if (polynomial.IsOne())
        {
            root[polynomial] = 1L;
            return root;
        }

        SortedDictionary<GenPolynomial<C>, long> factors = SquarefreeFactors(polynomial);
        long? minExponent = null;
        foreach (KeyValuePair<GenPolynomial<C>, long> entry in factors)
        {
            GenPolynomial<C> factor = entry.Key;
            if (factor.IsConstant())
            {
                continue;
            }

            long exponent = entry.Value;
            System.Numerics.BigInteger exponentValue = new(exponent);
            if (exponentValue % characteristic != 0)
            {
                return null;
            }

            if (!minExponent.HasValue || exponent < minExponent.Value)
            {
                minExponent = exponent;
            }
        }

        long characteristicLong = (long)characteristic;
        GenPolynomial<C> accumulator = polynomial.Ring.FromInteger(1);
        foreach (KeyValuePair<GenPolynomial<C>, long> entry in factors)
        {
            GenPolynomial<C> factor = entry.Key;
            long exponent = entry.Value;
            if (factor.IsConstant())
            {
                C coefficient = factor.LeadingBaseCoefficient();
                if (exponent > 1)
                {
                    coefficient = Power<C>.PositivePower(coefficient, exponent);
                }

                C rootCoefficient = CoeffRootCharacteristic(coefficient);
                GenPolynomial<C> constant = new GenPolynomial<C>(polynomial.Ring, rootCoefficient);
                root[constant] = 1L;
                continue;
            }

            if (minExponent.HasValue && exponent > minExponent.Value)
            {
                long quotientExponent = exponent / characteristicLong;
                factor = Power<GenPolynomial<C>>.PositivePower(factor, quotientExponent);
            }

            accumulator = accumulator.Multiply(factor);
        }

        if (minExponent.HasValue)
        {
            root[accumulator] = minExponent.Value / characteristicLong;
        }

        return root;
    }

    public override GenPolynomial<C>? BaseSquarefreePRoot(GenPolynomial<C> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<C> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        System.Numerics.BigInteger characteristic = ring.Characteristic();
        if (characteristic.Sign <= 0)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for char p > 0 {ring.CoFac}", nameof(polynomial));
        }

        long modulus = (long)characteristic;
        GenPolynomial<C> result = new(ring);
        foreach (Monomial<C> term in polynomial)
        {
            long exponent = term.E.GetVal(0);
            if (exponent % modulus != 0)
            {
                return null;
            }

            long adjustedExponent = exponent / modulus;
            ExpVector newExponent = ExpVector.Create(1, 0, adjustedExponent);
            C coefficient = CoeffRootCharacteristic(term.C);
            result.DoPutToMap(newExponent, coefficient);
        }

        return result;
    }

    public override GenPolynomial<GenPolynomial<C>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<C>> polynomial)
    {
        if (polynomial.IsZero())
        {
            return polynomial;
        }

        GenPolynomialRing<GenPolynomial<C>> ring = polynomial.Ring;
        if (ring.Nvar > 1)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for univariate polynomials.", nameof(polynomial));
        }

        System.Numerics.BigInteger characteristic = ring.Characteristic();
        if (characteristic.Sign <= 0)
        {
            throw new ArgumentException($"{polynomial.GetType().Name} only for char p > 0 {ring.CoFac}", nameof(polynomial));
        }

        long modulus = (long)characteristic;
        GenPolynomial<GenPolynomial<C>> result = new(ring);
        foreach (Monomial<GenPolynomial<C>> term in polynomial)
        {
            long exponent = term.E.GetVal(0);
            if (exponent % modulus != 0)
            {
                return null;
            }

            long adjustedExponent = exponent / modulus;
            SortedDictionary<GenPolynomial<C>, long>? roots = RootCharacteristic(term.C);
            if (roots is null)
            {
                return null;
            }

            GenPolynomial<C> coefficient = ring.CoFac.FromInteger(1);
            foreach (KeyValuePair<GenPolynomial<C>, long> entry in roots)
            {
                GenPolynomial<C> factor = entry.Key;
                long factorExponent = entry.Value;
                if (factorExponent > 1)
                {
                    factor = Power<GenPolynomial<C>>.PositivePower(factor, factorExponent);
                }

                coefficient = coefficient.Multiply(factor);
            }

            ExpVector newExponent = ExpVector.Create(1, 0, adjustedExponent);
            result.DoPutToMap(newExponent, coefficient);
        }

        return result;
    }
}
