using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Quotient ring factory based on GenPolynomial with RingElem interface. Objects
/// of this class are immutable.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.QuotientRing
/// </remarks>
public class QuotientRing<C> : RingFactory<Quotient<C>> where C : GcdRingElem<C>
{
    private static readonly Random RandomSource = new();

    public readonly GenPolynomialRing<C> Ring;
    public readonly GreatestCommonDivisor<C> Engine;
    public readonly bool UfdGcd;

    public QuotientRing(GenPolynomialRing<C> ring, bool ufdGcd)
    {
        ArgumentNullException.ThrowIfNull(ring);
        Ring = ring;
        UfdGcd = ufdGcd;
        Engine = GCDFactory.GetProxy(ring.CoFac);
    }

    internal GenPolynomial<C> Divide(GenPolynomial<C> numerator, GenPolynomial<C> denominator)
    {
        ArgumentNullException.ThrowIfNull(numerator);
        ArgumentNullException.ThrowIfNull(denominator);
        return PolyUtil.BasePseudoDivide(numerator, denominator);
    }

    internal GenPolynomial<C> Gcd(GenPolynomial<C> first, GenPolynomial<C> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        return Engine.Gcd(first, second);
    }

    public Quotient<C> Zero => new(this, Ring.FromInteger(0));

    public Quotient<C> One => new(this, Ring.FromInteger(1));

    public bool IsFinite() => false;

    public static Quotient<C> Clone(Quotient<C> c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return c.Clone();
    }

    public List<Quotient<C>> Generators()
    {
        List<GenPolynomial<C>> generators = Ring.Generators();
        List<Quotient<C>> result = new(generators.Count);
        foreach (GenPolynomial<C> polynomial in generators)
        {
            result.Add(new Quotient<C>(this, polynomial));
        }

        return result;
    }

    public bool IsCommutative() => Ring.IsCommutative();

    public bool IsAssociative() => Ring.IsAssociative();

    public bool IsField() => true;

    public Quotient<C> FromInteger(long a) => new(this, Ring.FromInteger(a));

    public Quotient<C> FromInteger(BigInteger a) => new(this, Ring.FromInteger(a));

    Quotient<C> ElemFactory<Quotient<C>>.FromInteger(BigInteger a) => FromInteger(a);

    public Quotient<C> Random(int n) => Random(n, RandomSource);

    public Quotient<C> Random(int n, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        GenPolynomial<C> numerator = Ring.Random(n).Monic();
        GenPolynomial<C> denominator = Ring.Random(n).Monic();
        while (denominator.IsZero())
        {
            denominator = Ring.Random(n, rnd).Monic();
        }

        return new Quotient<C>(this, numerator, denominator, false);
    }

    public BigInteger Characteristic() => Ring.Characteristic();

    BigInteger RingFactory<Quotient<C>>.Characteristic() => Characteristic();

    public Quotient<C> Parse(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentException("Input string must not be empty.", nameof(s));
        }

        string trimmed = s.Trim();
        if (trimmed.StartsWith("{", StringComparison.Ordinal) && trimmed.EndsWith("}", StringComparison.Ordinal))
        {
            trimmed = trimmed.Substring(1, trimmed.Length - 2).Trim();
        }

        string[] parts = trimmed.Split('|', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        GenPolynomial<C> numerator = ParseConstantPolynomial(parts.Length > 0 ? parts[0] : trimmed);
        GenPolynomial<C> denominator = parts.Length > 1 ? ParseConstantPolynomial(parts[1]) : Ring.FromInteger(1);
        return new Quotient<C>(this, numerator, denominator, false);
    }

    private GenPolynomial<C> ParseConstantPolynomial(string text)
    {
        string value = text.Trim();
        if (value.Length == 0)
        {
            throw new FormatException("Polynomial text must not be empty.");
        }

        if (long.TryParse(value, out long longValue))
        {
            return Ring.FromInteger(longValue);
        }

        if (BigInteger.TryParse(value, out BigInteger bigIntegerValue))
        {
            return Ring.FromInteger(bigIntegerValue);
        }

        return value switch
        {
            "0" => Ring.FromInteger(0),
            "1" => Ring.FromInteger(1),
            _ => throw new NotSupportedException("Only constant polynomial parsing is supported for Quotient.")
        };
    }

    public override string ToString()
    {
        string prefix = Ring.CoFac.Characteristic().Sign == 0 ? "RatFunc" : "ModFunc";
        return $"{prefix}( {Ring} )";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not QuotientRing<C> other)
        {
            return false;
        }

        return Ring.Equals(other.Ring);
    }

    public override int GetHashCode() => Ring.GetHashCode();

    public string ToScript() => ToString();
}
