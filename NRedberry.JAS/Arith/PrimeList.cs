using System.Collections;
using System.Text;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// List of big primes. Provides an Iterator for generating prime numbers.
/// Similar to ALDES/SAC2 SACPOL.PRIME list.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.PrimeList
/// See Knuth vol 2, page 390, for list of known primes. See also ALDES/SAC2 SACPOL.PRIME
/// </remarks>
public sealed class PrimeList : IEnumerable<BigInteger>
{
    private static readonly object SmallLock = new();
    private static readonly object LowLock = new();
    private static readonly object MediumLock = new();
    private static readonly object LargeLock = new();
    private static readonly object MersenneLock = new();

    public enum Range
    {
        Small,
        Low,
        Medium,
        Large,
        Mersenne
    }

    private static volatile List<BigInteger>? _smallList;
    private static volatile List<BigInteger>? _lowList;
    private static volatile List<BigInteger>? _mediumList;
    private static volatile List<BigInteger>? _largeList;
    private static volatile List<BigInteger>? _mersenneList;

    private readonly List<BigInteger> _val;
    private BigInteger _last = BigInteger.Zero;

    public PrimeList() : this(Range.Medium)
    {
    }

    public PrimeList(Range r)
    {
        _val = r switch
        {
            Range.Small => EnsureList(ref _smallList, SmallLock, AddSmall),
            Range.Low => EnsureList(ref _lowList, LowLock, AddLow),
            Range.Medium => EnsureList(ref _mediumList, MediumLock, AddMedium),
            Range.Large => EnsureList(ref _largeList, LargeLock, AddLarge),
            Range.Mersenne => EnsureList(ref _mersenneList, MersenneLock, AddMersenne),
            _ => EnsureList(ref _mediumList, MediumLock, AddMedium),
        };

        if (_val.Count > 0)
        {
            _last = _val[^1];
        }
    }

    private static List<BigInteger> EnsureList(ref List<BigInteger>? cache, object syncRoot, Action<List<BigInteger>> builder)
    {
        var existing = cache;
        if (existing != null)
        {
            return existing;
        }

        lock (syncRoot)
        {
            existing = cache;
            if (existing != null)
            {
                return existing;
            }

            var list = new List<BigInteger>(50);
            builder(list);
            cache = list;
            return list;
        }
    }

    private static void AddSmall(List<BigInteger> target)
    {
        long[] primes =
        [
            2L,
            3L,
            5L,
            7L,
            11L,
            13L,
            17L,
            19L,
            23L,
            29L
        ];

        foreach (long prime in primes)
        {
            target.Add(new BigInteger(prime));
        }
    }

    private static void AddLow(List<BigInteger> target)
    {
        int[,] parameters =
        {
            { 15, 19 },
            { 15, 49 },
            { 15, 51 },
            { 15, 55 },
            { 15, 61 },
            { 15, 75 },
            { 15, 81 },
            { 15, 115 },
            { 15, 121 },
            { 15, 135 },
            { 16, 15 },
            { 16, 17 },
            { 16, 39 },
            { 16, 57 },
            { 16, 87 },
            { 16, 89 },
            { 16, 99 },
            { 16, 113 },
            { 16, 117 },
            { 16, 123 }
        };

        for (int i = 0; i < parameters.GetLength(0); i++)
        {
            target.Add(GetLongPrime(parameters[i, 0], parameters[i, 1]));
        }
    }

    private static void AddMedium(List<BigInteger> target)
    {
        int[,] parameters =
        {
            { 28, 57 },
            { 28, 89 },
            { 28, 95 },
            { 28, 119 },
            { 28, 125 },
            { 28, 143 },
            { 28, 165 },
            { 28, 183 },
            { 28, 213 },
            { 28, 273 },
            { 29, 3 },
            { 29, 33 },
            { 29, 43 },
            { 29, 63 },
            { 29, 73 },
            { 29, 75 },
            { 29, 93 },
            { 29, 99 },
            { 29, 121 },
            { 29, 133 },
            { 32, 5 },
            { 32, 17 },
            { 32, 65 },
            { 32, 99 },
            { 32, 107 },
            { 32, 135 },
            { 32, 153 },
            { 32, 185 },
            { 32, 209 },
            { 32, 267 }
        };

        for (int i = 0; i < parameters.GetLength(0); i++)
        {
            target.Add(GetLongPrime(parameters[i, 0], parameters[i, 1]));
        }
    }

    private static void AddLarge(List<BigInteger> target)
    {
        int[,] parameters =
        {
            { 59, 55 },
            { 59, 99 },
            { 59, 225 },
            { 59, 427 },
            { 59, 517 },
            { 59, 607 },
            { 59, 649 },
            { 59, 687 },
            { 59, 861 },
            { 59, 871 },
            { 60, 93 },
            { 60, 107 },
            { 60, 173 },
            { 60, 179 },
            { 60, 257 },
            { 60, 279 },
            { 60, 369 },
            { 60, 395 },
            { 60, 399 },
            { 60, 453 },
            { 63, 25 },
            { 63, 165 },
            { 63, 259 },
            { 63, 301 },
            { 63, 375 },
            { 63, 387 },
            { 63, 391 },
            { 63, 409 },
            { 63, 457 },
            { 63, 471 }
        };

        for (int i = 0; i < parameters.GetLength(0); i++)
        {
            target.Add(GetLongPrime(parameters[i, 0], parameters[i, 1]));
        }
    }

    private static void AddMersenne(List<BigInteger> target)
    {
        int[] exponents =
        [
            2,
            3,
            5,
            7,
            13,
            17,
            19,
            31,
            61,
            89,
            107,
            127,
            521,
            607,
            1279,
            2203,
            2281,
            3217,
            4253,
            4423,
            9689,
            9941,
            11213,
            19937
        ];

        foreach (int exponent in exponents)
        {
            target.Add(GetMersennePrime(exponent));
        }
    }

    public static BigInteger GetLongPrime(int n, int m)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n));
        }

        if (m < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(m));
        }

        System.Numerics.BigInteger prime = System.Numerics.BigInteger.One << n;
        prime -= m;
        return new BigInteger(prime);
    }

    public static BigInteger GetMersennePrime(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n));
        }

        System.Numerics.BigInteger value = System.Numerics.BigInteger.One << n;
        value -= System.Numerics.BigInteger.One;
        return new BigInteger(value);
    }

    public override string ToString()
    {
        if (_val.Count == 0)
        {
            return "[]";
        }

        StringBuilder builder = new();
        builder.Append('[');
        for (int i = 0; i < _val.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(", ");
            }
            builder.Append(_val[i]);
        }
        builder.Append(']');
        return builder.ToString();
    }

    public int Size() => _val.Count;

    public BigInteger Get(int i)
    {
        if (i < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(i));
        }

        while (_val.Count <= i)
        {
            _last = ComputeNextPrime(_last);
            _val.Add(_last);
        }

        return _val[i];
    }

    public IEnumerator<BigInteger> GetEnumerator()
    {
        int index = -1;
        while (true)
        {
            index++;
            yield return Get(index);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static BigInteger ComputeNextPrime(BigInteger current)
    {
        System.Numerics.BigInteger candidate = current.Val + System.Numerics.BigInteger.One;
        if (candidate < 2)
        {
            candidate = new System.Numerics.BigInteger(2);
        }

        if (candidate % 2 == 0 && candidate != 2)
        {
            candidate += System.Numerics.BigInteger.One;
        }

        while (!candidate.IsProbablePrime((int)candidate.GetBitLength()))
        {
            candidate += 2;
        }

        return new BigInteger(candidate);
    }
}
