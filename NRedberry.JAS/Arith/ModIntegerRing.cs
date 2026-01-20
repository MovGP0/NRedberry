using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// ModIntegerRing factory with RingFactory interface. Effectively immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModIntegerRing
/// </remarks>
public sealed class ModIntegerRing : ModularRingFactory<ModInteger>, IEnumerable<ModInteger>, ICloneable, IEquatable<ModIntegerRing>
{
    public BigInteger Modul { get; }

    private static readonly Random s_random = new();

    private int _isField = -1;

    public ModIntegerRing(BigInteger m)
    {
        ArgumentNullException.ThrowIfNull(m);
        Modul = m;
    }

    public ModIntegerRing(BigInteger m, bool isFieldArg)
        : this(m)
    {
        _isField = isFieldArg ? 1 : 0;
    }

    public BigInteger GetModul() => Modul;

    public BigInteger GetIntegerModul() => new(Modul.Val);

    public ModInteger Create(BigInteger c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return new ModInteger(this, c);
    }

    public ModInteger Create(long c) => new(this, c);

    public ModInteger Copy(ModInteger c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return new ModInteger(this, c.Val);
    }

    public ModIntegerRing Clone() => _isField switch
    {
        1 => new(Modul, true),
        0 => new(Modul, false),
        _ => new(Modul)
    };

    object ICloneable.Clone() => Clone();

    public static ModInteger Clone(ModInteger m)
    {
        ArgumentNullException.ThrowIfNull(m);
        return m.Clone();
    }

    public ModInteger Zero => new(this, BigInteger.Zero);

    public ModInteger One => new(this, BigInteger.One);

    public List<ModInteger> Generators() => [One];

    public bool IsFinite() => true;

    public bool IsCommutative() => true;

    public bool IsAssociative() => true;

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

        System.Numerics.BigInteger modulus = Modul.Val;
        if (modulus.IsProbablePrime((int)modulus.GetBitLength()))
        {
            _isField = 1;
            return true;
        }

        _isField = 0;
        return false;
    }

    public BigInteger Characteristic() => new(Modul.Val);

    System.Numerics.BigInteger RingFactory<ModInteger>.Characteristic() => Modul.Val;

    public ModInteger FromInteger(BigInteger a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return new ModInteger(this, a);
    }

    ModInteger ElemFactory<ModInteger>.FromInteger(System.Numerics.BigInteger a)
        => FromInteger(new BigInteger(a));

    public ModInteger FromInteger(long a) => new(this, a);

    public ModInteger Random(int n) => Random(n, s_random);

    public ModInteger Random(int n, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        if (n <= 0)
        {
            return Zero;
        }

        int length = (n + 7) / 8;
        byte[] bytes = new byte[length + 1];
        rnd.NextBytes(bytes);
        if (n % 8 != 0)
        {
            byte mask = (byte)((1 << (n % 8)) - 1);
            bytes[length - 1] &= mask;
        }

        bytes[^1] = 0;
        System.Numerics.BigInteger value = new(bytes);
        if (value.Sign < 0)
        {
            value = System.Numerics.BigInteger.Negate(value);
        }

        return new ModInteger(this, new BigInteger(value));
    }

    public ModInteger ChineseRemainder(ModInteger c, ModInteger ci, ModInteger a)
    {
        ArgumentNullException.ThrowIfNull(c);
        ArgumentNullException.ThrowIfNull(ci);
        ArgumentNullException.ThrowIfNull(a);

        ModInteger b = a.Ring.FromInteger(c.Val);
        ModInteger d = a.Subtract(b);
        if (d.IsZero())
        {
            return FromInteger(c.Val);
        }

        b = d.Multiply(ci);
        BigInteger s = (c.Ring.Modul * b.Val) + c.Val;
        return FromInteger(s);
    }

    public override string ToString() => " bigMod(" + Modul + ")";

    public override bool Equals(object? b) => Equals(b as ModIntegerRing);

    public bool Equals(ModIntegerRing? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Modul.CompareTo(other.Modul) == 0;
    }

    public static bool operator ==(ModIntegerRing? left, ModIntegerRing? right) => Equals(left, right);

    public static bool operator !=(ModIntegerRing? left, ModIntegerRing? right) => !Equals(left, right);

    public override int GetHashCode() => Modul.GetHashCode();

    public IEnumerator<ModInteger> GetEnumerator() => new ModIntegerEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal sealed class ModIntegerEnumerator : IEnumerator<ModInteger>
{
    private BigInteger _curr;
    private readonly ModIntegerRing _ring;
    private ModInteger? _current;

    public ModIntegerEnumerator(ModIntegerRing ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        _ring = ring;
        _curr = BigInteger.Zero;
    }

    public ModInteger Current => _current ?? throw new InvalidOperationException("Enumeration has not started.");

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (_curr.CompareTo(_ring.Modul) >= 0)
        {
            return false;
        }

        _current = new ModInteger(_ring, _curr);
        _curr += BigInteger.One;
        return true;
    }

    public void Reset()
    {
        _curr = BigInteger.Zero;
        _current = null;
    }

    public void Dispose()
    {
    }
}
