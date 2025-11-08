using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Converts a polynomial with rational coefficients into one with integer coefficients by clearing denominators.
    /// </summary>
    /// <param name="resultRing">Target polynomial ring over integers.</param>
    /// <param name="polynomial">Polynomial with <see cref="BigRational"/> coefficients.</param>
    /// <returns>Polynomial with integer coefficients obtained via the common denominator.</returns>
    /// <remarks>Original Java method: PolyUtil#integerFromRationalCoefficients.</remarks>
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
}
