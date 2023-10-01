using System.Numerics;

namespace NRedberry.Apache.Commons.Math;

public class BigFraction : IFieldElement<BigFraction>
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

    public BigFraction Add(BigFraction other)
    {
        // Implementation
        return new BigFraction(Numerator * other.Denominator + Denominator * other.Numerator, Denominator * other.Denominator);
    }

    public BigFraction Subtract(BigFraction other)
    {
        // Implementation
        return new BigFraction(Numerator * other.Denominator - Denominator * other.Numerator, Denominator * other.Denominator);
    }

    public BigFraction Multiply(int n)
    {
        return new BigFraction(Numerator * n, Denominator);
    }

    public BigFraction Multiply(BigFraction other)
    {
        // Implementation
        return new BigFraction(Numerator * other.Numerator, Denominator * other.Denominator);
    }

    public BigFraction Divide(BigFraction other)
    {
        if (other.Numerator == 0)
            throw new ArgumentException("Cannot divide by zero.");
        // Implementation
        return new BigFraction(Numerator * other.Denominator, Denominator * other.Numerator);
    }

    public BigFraction Negate()
    {
        // Implementation
        return new BigFraction(-Numerator, Denominator);
    }

    public BigFraction Reciprocal()
    {
        if (Numerator == 0)
            throw new ArgumentException("Cannot compute the reciprocal of zero.");
        // Implementation
        return new BigFraction(Denominator, Numerator);
    }

    public IField<BigFraction> Field => throw new NotImplementedException();
}