using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// ModLongRing factory with RingFactory interface. Effectively immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModLongRing
/// </remarks>
public sealed class ModLongRing : ModularRingFactory<ModLong>, IEnumerable<ModLong>, IEquatable<ModLongRing>
{
    /// <summary>
    /// Module part of the factory data structure.
    /// </summary>
    public long Modul { get; }

    private static readonly Random s_random = new();

    /// <summary>
    /// Indicator if this ring is a field.
    /// </summary>
    private int _isField = -1;

    /// <summary>
    /// maximal representable integer.
    /// </summary>
    public static readonly System.Numerics.BigInteger MaxLong = new(int.MaxValue);

    /// <summary>
    /// The constructor creates a ModLongRing object from a long integer as module part.
    /// </summary>
    /// <param name="m">long integer.</param>
    public ModLongRing(long m)
    {
        Modul = m;
    }

    /// <summary>
    /// The constructor creates a ModLongRing object from a long integer as module part.
    /// </summary>
    /// <param name="m">long integer.</param>
    /// <param name="isFieldArg">indicator if m is prime.</param>
    public ModLongRing(long m, bool isFieldArg)
    {
        Modul = m;
        _isField = isFieldArg ? 1 : 0;
    }

    /// <summary>
    /// The constructor creates a ModLongRing object from a BigInteger converted to long as module part.
    /// </summary>
    /// <param name="m">System.Numerics.BigInteger.</param>
    public ModLongRing(System.Numerics.BigInteger m)
        : this((long)m)
    {
        if (MaxLong.CompareTo(m) <= 0)
        {
            throw new ArgumentException("modul to large for long " + m);
        }
    }

    /// <summary>
    /// The constructor creates a ModLongRing object from a BigInteger converted to long as module part.
    /// </summary>
    /// <param name="m">System.Numerics.BigInteger.</param>
    /// <param name="isFieldArg">indicator if m is prime.</param>
    public ModLongRing(System.Numerics.BigInteger m, bool isFieldArg)
        : this((long)m, isFieldArg)
    {
        if (MaxLong.CompareTo(m) <= 0)
        {
            throw new ArgumentException("modul to large for long " + m);
        }
    }

    /// <summary>
    /// Get the module part as BigInteger.
    /// </summary>
    /// <returns>modul.</returns>
    public BigInteger GetIntegerModul() => new(Modul);

    /// <summary>
    /// Create ModLong element c.
    /// </summary>
    public ModLong Create(System.Numerics.BigInteger c) => new(this, c);

    /// <summary>
    /// Create ModLong element c.
    /// </summary>
    public ModLong Create(long c) => new(this, c);

    /// <summary>
    /// Copy ModLong element c.
    /// </summary>
    public ModLong Copy(ModLong c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return new ModLong(this, c.Val);
    }

    public static ModLong Clone(ModLong c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return c.Clone();
    }

    /// <summary>
    /// Get the zero element.
    /// </summary>
    /// <returns>0 as ModLong.</returns>
    public ModLong Zero => new(this, 0L);

    /// <summary>
    /// Get the one element.
    /// </summary>
    /// <returns>1 as ModLong.</returns>
    public ModLong One => new(this, 1L);

    /// <summary>
    /// Get a list of the generating elements.
    /// </summary>
    public List<ModLong> Generators() => [One];

    /// <summary>
    /// Is this structure finite or infinite.
    /// </summary>
    /// <returns>true if this structure is finite, else false.</returns>
    public bool IsFinite() => true;

    /// <summary>
    /// Query if this ring is commutative.
    /// </summary>
    /// <returns>true.</returns>
    public bool IsCommutative() => true;

    /// <summary>
    /// Query if this ring is associative.
    /// </summary>
    /// <returns>true.</returns>
    public bool IsAssociative() => true;

    /// <summary>
    /// Query if this ring is a field.
    /// </summary>
    /// <returns>true if module is prime, else false.</returns>
    public bool IsField()
    {
        if (_isField > 0)
        {
            return true;
        }

        if (_isField == 0)
        {
            return false;
        }

        System.Numerics.BigInteger m = new System.Numerics.BigInteger(Modul);
        if (m.IsProbablePrime((int)m.GetBitLength()))
        {
            _isField = 1;
            return true;
        }

        _isField = 0;
        return false;
    }

    /// <summary>
    /// Characteristic of this ring.
    /// </summary>
    /// <returns>characteristic of this ring.</returns>
    public BigInteger Characteristic() => new(Modul);

    System.Numerics.BigInteger RingFactory<ModLong>.Characteristic() => new(Modul);

    /// <summary>
    /// Get a ModLong element from a BigInteger value.
    /// </summary>
    public ModLong FromInteger(System.Numerics.BigInteger a) => new(this, a);

    ModLong ElemFactory<ModLong>.FromInteger(System.Numerics.BigInteger a) => FromInteger(a);

    /// <summary>
    /// Get a ModLong element from a long value.
    /// </summary>
    public ModLong FromInteger(long a) => new(this, a);

    /// <summary>
    /// ModLong random.
    /// </summary>
    public ModLong Random(int n) => Random(n, s_random);

    /// <summary>
    /// ModLong random.
    /// </summary>
    public ModLong Random(int n, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        byte[] bytes = new byte[(n + 7) / 8];
        rnd.NextBytes(bytes);
        System.Numerics.BigInteger v = new System.Numerics.BigInteger(bytes);
        return new ModLong(this, v);
    }

    /// <summary>
    /// ModLong chinese remainder algorithm. This is a factory method.
    /// </summary>
    public ModLong ChineseRemainder(ModLong c, ModLong ci, ModLong a)
    {
        ArgumentNullException.ThrowIfNull(c);
        ArgumentNullException.ThrowIfNull(ci);
        ArgumentNullException.ThrowIfNull(a);
        ModLong b = a.Ring.FromInteger(c.Val);
        ModLong d = a.Subtract(b);
        if (d.IsZero())
        {
            return new ModLong(this, c.Val);
        }

        b = d.Multiply(ci);
        long s = c.Ring.Modul * b.Val;
        s += c.Val;
        return new ModLong(this, s);
    }

    /// <summary>
    /// Get the String representation.
    /// </summary>
    public override string ToString() => " mod(" + Modul + ")";

    /// <summary>
    /// Comparison with any other object.
    /// </summary>
    public override bool Equals(object? b) => Equals(b as ModLongRing);

    public bool Equals(ModLongRing? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Modul == other.Modul;
    }

    public static bool operator ==(ModLongRing? left, ModLongRing? right) => Equals(left, right);

    public static bool operator !=(ModLongRing? left, ModLongRing? right) => !Equals(left, right);

    /// <summary>
    /// Hash code for this ModLongRing.
    /// </summary>
    public override int GetHashCode() => Modul.GetHashCode();

    /// <summary>
    /// Get a ModLong iterator.
    /// </summary>
    /// <returns>a iterator over all modular integers in this ring.</returns>
    public IEnumerator<ModLong> GetEnumerator() => new ModLongIterator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Modular integer iterator.
/// </summary>
internal class ModLongIterator : IEnumerator<ModLong>
{
    private long _curr;
    private readonly ModLongRing _ring;
    private ModLong? _current;

    /// <summary>
    /// ModLong iterator constructor.
    /// </summary>
    public ModLongIterator(ModLongRing fac)
    {
        ArgumentNullException.ThrowIfNull(fac);
        _curr = 0L;
        _ring = fac;
    }

    public ModLong Current => _current!;

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_curr >= _ring.Modul)
        {
            return false;
        }

        _current = new ModLong(_ring, _curr);
        _curr++;
        return true;
    }

    public void Reset()
    {
        _curr = 0L;
        _current = null;
    }

    public void Dispose()
    {
    }
}

/// <summary>
/// Extension methods for BigInteger primality testing.
/// </summary>
internal static class BigIntegerExtensions
{
    public static bool IsProbablePrime(this System.Numerics.BigInteger value, int bitLength)
    {
        if (value < 2)
        {
            return false;
        }

        if (value == 2 || value == 3)
        {
            return true;
        }

        if (value % 2 == 0)
        {
            return false;
        }

        // Miller-Rabin primality test
        System.Numerics.BigInteger d = value - 1;
        int s = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            s++;
        }

        // Number of rounds based on bit length
        int k = Math.Max(bitLength / 16, 5);
        Random rnd = new();

        for (int i = 0; i < k; i++)
        {
            byte[] bytes = value.ToByteArray();
            System.Numerics.BigInteger a;
            do
            {
                rnd.NextBytes(bytes);
                a = new System.Numerics.BigInteger(bytes);
                a = System.Numerics.BigInteger.Abs(a);
            } while (a < 2 || a >= value - 2);

            System.Numerics.BigInteger x = System.Numerics.BigInteger.ModPow(a, d, value);
            if (x == 1 || x == value - 1)
            {
                continue;
            }

            bool composite = true;
            for (int r = 0; r < s - 1; r++)
            {
                x = System.Numerics.BigInteger.ModPow(x, 2, value);
                if (x == value - 1)
                {
                    composite = false;
                    break;
                }
            }

            if (composite)
            {
                return false;
            }
        }

        return true;
    }

    public static int GetBitLength(this System.Numerics.BigInteger value)
    {
        if (value.IsZero)
        {
            return 0;
        }

        byte[] bytes = System.Numerics.BigInteger.Abs(value).ToByteArray();
        byte msb = bytes[^1];
        int bitLength = (bytes.Length - 1) * 8;
        while (msb != 0)
        {
            msb >>= 1;
            bitLength++;
        }

        return bitLength;
    }
}
