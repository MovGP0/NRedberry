using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static BigInteger FactorBound(ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(exponent);
        if (exponent.IsZero())
        {
            return BigInteger.One;
        }

        int bits = 0;
        System.Numerics.BigInteger product = System.Numerics.BigInteger.One;

        for (int i = 0; i < exponent.Length(); i++)
        {
            long value = exponent.GetVal(i);
            if (value <= 0)
            {
                continue;
            }

            bits += (int)(2 * value - 1);
            System.Numerics.BigInteger factor = new(value - 1);
            product *= factor;
        }

        bits += PopCount(product) + 1;
        bits /= 2;

        System.Numerics.BigInteger result = System.Numerics.BigInteger.One << (bits + 1);
        return new BigInteger(result);
    }
}
