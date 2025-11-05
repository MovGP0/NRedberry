using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
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
}
