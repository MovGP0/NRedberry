using System.Numerics;
using JetBrains.Annotations;

namespace NRedberry.Apache.Commons.Math;

public sealed class BigFraction : IFieldElement<BigFraction>, IEquatable<BigFraction>
{
    public BigInteger Numerator { get; }
    public BigInteger Denominator { get; }

    public BigFraction(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Denominator cannot be zero.");

        // Simplification and initialization logic...
        Numerator = numerator;
        Denominator = denominator;
    }

    [UsedImplicitly]
    public void Deconstruct(out BigInteger numerator, out BigInteger denominator)
    {
        numerator = Numerator;
        denominator = Denominator;
    }

    public BigFraction Add(BigFraction other)
    {
        return new(
            Numerator * other.Denominator + Denominator * other.Numerator,
            Denominator * other.Denominator);
    }

    public BigFraction Subtract(BigFraction other)
    {
        return new(
            Numerator * other.Denominator - Denominator * other.Numerator,
            Denominator * other.Denominator);
    }

    public BigFraction Multiply(int n)
    {
        return new(Numerator * n, Denominator);
    }

    public BigFraction Multiply(BigFraction other)
    {
        return new(
            Numerator * other.Numerator,
            Denominator * other.Denominator);
    }

    public BigFraction Divide(BigFraction other)
    {
        if (other.Numerator == 0)
            throw new ArgumentException("Cannot divide by zero.");

        return new(
            Numerator * other.Denominator,
            Denominator * other.Numerator);
    }

    public BigFraction Negate()
    {
        return new(-Numerator, Denominator);
    }

    public BigFraction Reciprocal()
    {
        if (Numerator == 0)
            throw new ArgumentException("Cannot compute the reciprocal of zero.");

        return new(Denominator, Numerator);
    }

    private static readonly BigFractionField BigFractionField = new();

    public IField<BigFraction> Field => BigFractionField;

    public static BigFraction operator +(BigFraction a, BigFraction b) => a.Add(b);

    public static BigFraction operator -(BigFraction a, BigFraction b) => a.Subtract(b);

    public static BigFraction operator *(BigFraction a, BigFraction b) => a.Multiply(b);

    public static BigFraction operator /(BigFraction a, BigFraction b) => a.Divide(b);

    public static BigFraction operator -(BigFraction a) => a.Negate();

    public static bool operator ==(BigFraction a, BigFraction b) => a.Equals(b);

    public static bool operator !=(BigFraction a, BigFraction b) => !a.Equals(b);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is BigFraction other && Equals(other);
    }

    public override int GetHashCode()
        => HashCode.Combine(Numerator, Denominator);

    public override string ToString()
        => $"{Numerator}/{Denominator}";

    /// <summary>
    /// Returns the reduced form of this fraction.
    /// </summary>
    /// <returns>The reduced fraction.</returns>
    [Pure]
    public BigFraction Reduce()
    {
        var gcd = BigInteger.GreatestCommonDivisor(Numerator, Denominator);
        return new(Numerator / gcd, Denominator / gcd);
    }

    public bool Equals(BigFraction? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        if (Numerator.Equals(other.Numerator)
            && Denominator.Equals(other.Denominator))
        {
            return true;
        }

        var reducedThis = Reduce();
        var reducedOther = other.Reduce();
        return reducedThis.Numerator == reducedOther.Numerator
            && reducedThis.Denominator == reducedOther.Denominator;
    }
}
