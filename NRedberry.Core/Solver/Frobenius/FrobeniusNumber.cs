using System.Numerics;

namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/FrobeniusNumber.java
 */

public static class FrobeniusNumber
{
    public static BigInteger Calculate(int[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        var bigArray = new BigInteger[array.Length];
        for (int i = array.Length - 1; i >= 0; --i)
        {
            bigArray[i] = new BigInteger(array[i]);
        }

        return Calculate(bigArray);
    }

    public static BigInteger Calculate(long[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        var bigArray = new BigInteger[array.Length];
        for (int i = array.Length - 1; i >= 0; --i)
        {
            bigArray[i] = new BigInteger(array[i]);
        }

        return Calculate(bigArray);
    }

    public static BigInteger Calculate(BigInteger[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        return array[0].CompareTo(ArraySizeThreshold) > 0
            ? CalculateFromIntegerArray(array)
            : CalculateFromIntegerArray(array);
    }

    public static BigInteger CalculateFromBigIntegerArray(BigInteger[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        var ns = new Dictionary<BigInteger, BigInteger>(ArraySizeThresholdInt);
        ns[BigInteger.Zero] = BigInteger.Zero;
        for (int i = 1; i < array.Length; i++)
        {
            BigInteger d = Gcd(array[0], array[i]);
            for (BigInteger r = BigInteger.Zero; r.CompareTo(d) < 0; r += BigInteger.One)
            {
                BigInteger n = MinusOne;
                if (r.IsZero)
                {
                    n = BigInteger.Zero;
                }
                else
                {
                    BigInteger q = r;
                    while (q.CompareTo(array[0]) < 0)
                    {
                        if (ns.TryGetValue(q, out BigInteger value) && (value.CompareTo(n) < 0 || n.Equals(MinusOne)))
                        {
                            n = value;
                        }

                        q += d;
                    }
                }

                if (!n.Equals(MinusOne))
                {
                    int size = IntValue(BigInteger.Divide(array[0], d));
                    for (int j = 0; j < size; j++)
                    {
                        n += array[i];
                        BigInteger p = BigInteger.Remainder(n, array[0]);
                        if (ns.TryGetValue(p, out BigInteger value) && (value.CompareTo(n) < 0 || n.Equals(MinusOne)))
                        {
                            n = value;
                        }

                        ns[p] = n;
                    }
                }
            }
        }

        BigInteger max = MinusOne;
        foreach (BigInteger value in ns.Values)
        {
            if (value.CompareTo(max) > 0)
            {
                max = value;
            }
        }

        return max - array[0];
    }

    private static BigInteger CalculateFromIntegerArray(BigInteger[] array)
    {
        int array0 = IntValue(array[0]);
        var ns = new BigInteger[array0];
        Array.Fill(ns, MinusOne);
        ns[0] = BigInteger.Zero;

        for (int i = 1; i < array.Length; i++)
        {
            int d = IntValue(Gcd(array[0], array[i]));
            for (int r = 0; r < d; r++)
            {
                BigInteger n = MinusOne;
                if (r == 0)
                {
                    n = BigInteger.Zero;
                }
                else
                {
                    int q = r;
                    while (q < array0)
                    {
                        if (!ns[q].Equals(MinusOne) && (ns[q].CompareTo(n) < 0 || n.Equals(MinusOne)))
                        {
                            n = ns[q];
                        }

                        q += d;
                    }
                }

                if (!n.Equals(MinusOne))
                {
                    int size = array0 / d;
                    for (int j = 0; j < size; j++)
                    {
                        n += array[i];
                        int p = IntValue(BigInteger.Remainder(n, array[0]));
                        if (!ns[p].Equals(MinusOne) && (ns[p].CompareTo(n) < 0 || n.Equals(MinusOne)))
                        {
                            n = ns[p];
                        }

                        ns[p] = n;
                    }
                }
            }
        }

        BigInteger max = BigInteger.Zero;
        for (int i = 0; i < array0; i++)
        {
            if (ns[i].Equals(MinusOne) || ns[i].CompareTo(max) > 0)
            {
                max = ns[i];
            }
        }

        if (max.Equals(MinusOne))
        {
            return MinusOne;
        }

        return max - array[0];
    }

    private static BigInteger Gcd(BigInteger a, BigInteger b)
    {
        return BigInteger.GreatestCommonDivisor(a, b);
    }

    private static int IntValue(BigInteger integer)
    {
        if (integer.CompareTo(MaxValue) > 0)
        {
            throw new NotSupportedException("Integer overflow.");
        }

        return (int)integer;
    }

    private const int ArraySizeThresholdInt = 10000;

    private static readonly BigInteger MinusOne = new(-1);
    private static readonly BigInteger MaxValue = new(int.MaxValue);
    private static readonly BigInteger ArraySizeThreshold = new(ArraySizeThresholdInt);
}
