using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Quotient ring element, basically a rational function.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.Quotient
/// </remarks>
public class Quotient<C> : GcdRingElem<Quotient<C>> where C : GcdRingElem<C>
{
    public readonly QuotientRing<C> Ring;
    public readonly GenPolynomial<C> Num;
    public readonly GenPolynomial<C> Den;

    public Quotient(QuotientRing<C> ring, GenPolynomial<C> numerator)
        : this(ring, numerator, (ring ?? throw new ArgumentNullException(nameof(ring))).Ring.FromInteger(1), true)
    {
    }

    public Quotient(QuotientRing<C> ring, GenPolynomial<C> numerator, GenPolynomial<C> denominator)
        : this(ring, numerator, denominator, false)
    {
    }

    internal Quotient(QuotientRing<C> ring, GenPolynomial<C> numerator, GenPolynomial<C> denominator, bool isReduced)
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(numerator);
        ArgumentNullException.ThrowIfNull(denominator);
        if (denominator.IsZero())
        {
            throw new ArgumentException("Denominator may not be zero.", nameof(denominator));
        }

        Ring = ring;
        GenPolynomial<C> localNumerator = numerator;
        GenPolynomial<C> localDenominator = denominator;

        if (localDenominator.Signum() < 0)
        {
            localNumerator = localNumerator.Negate();
            localDenominator = localDenominator.Negate();
        }

        if (!isReduced)
        {
            GenPolynomial<C> gcd = Ring.Gcd(localNumerator, localDenominator);
            if (!gcd.IsOne())
            {
                localNumerator = Ring.Divide(localNumerator, gcd);
                localDenominator = Ring.Divide(localDenominator, gcd);
            }
        }

        C leadingDenominator = localDenominator.LeadingBaseCoefficient();
        if (!leadingDenominator.IsOne() && leadingDenominator.IsUnit())
        {
            C normalization = leadingDenominator.Inverse().Abs();
            localNumerator = localNumerator.Multiply(normalization);
            localDenominator = localDenominator.Multiply(normalization);
        }

        Num = localNumerator;
        Den = localDenominator;
    }

    public ElemFactory<Quotient<C>> Factory() => Ring;

    public Quotient<C> Clone() => new(Ring, Num, Den, true);

    public bool IsZero() => Num.IsZero();

    public bool IsOne() => Num.Equals(Den);

    public bool IsUnit() => !Num.IsZero();

    public bool IsConstant() => Num.IsConstant() && Den.IsConstant();

    public override string ToString()
    {
        string[]? variables = Ring.Ring.GetVars();
        string numerator = variables is null ? Num.ToString() : Num.ToString(variables);
        if (Den.IsOne())
        {
            return "{ " + numerator + " }";
        }

        string denominator = variables is null ? Den.ToString() : Den.ToString(variables);
        return "{ " + numerator + " | " + denominator + " }";
    }

    public Quotient<C> Sum(Quotient<C> other)
    {
        if (other.IsZero())
        {
            return this;
        }

        if (IsZero())
        {
            return other;
        }

        GenPolynomial<C> numerator;
        if (Den.IsOne() && other.Den.IsOne())
        {
            numerator = Num.Sum(other.Num);
            return new Quotient<C>(Ring, numerator);
        }

        if (Den.IsOne())
        {
            numerator = Num.Multiply(other.Den).Sum(other.Num);
            return new Quotient<C>(Ring, numerator, other.Den, true);
        }

        if (other.Den.IsOne())
        {
            numerator = other.Num.Multiply(Den).Sum(Num);
            return new Quotient<C>(Ring, numerator, Den, true);
        }

        GenPolynomial<C> gcd = Ring.Gcd(Den, other.Den);
        GenPolynomial<C> denominatorPart = Den;
        GenPolynomial<C> otherDenominatorPart = other.Den;

        if (!gcd.IsOne())
        {
            denominatorPart = Ring.Divide(Den, gcd);
            otherDenominatorPart = Ring.Divide(other.Den, gcd);
        }

        numerator = Num.Multiply(otherDenominatorPart)
            .Sum(denominatorPart.Multiply(other.Num));

        if (numerator.IsZero())
        {
            return Ring.Zero;
        }

        GenPolynomial<C> reducedDenominatorFactor = Den;
        if (!gcd.IsOne())
        {
            GenPolynomial<C> commonFactor = Ring.Gcd(numerator, gcd);
            if (!commonFactor.IsOne())
            {
                numerator = Ring.Divide(numerator, commonFactor);
                reducedDenominatorFactor = Ring.Divide(Den, commonFactor);
            }
        }

        GenPolynomial<C> denominator = reducedDenominatorFactor.Multiply(otherDenominatorPart);
        return new Quotient<C>(Ring, numerator, denominator, true);
    }

    public Quotient<C> Subtract(Quotient<C> other)
    {
        if (other is null)
        {
            return this;
        }

        return Sum(other.Negate());
    }

    public Quotient<C> Negate() => new(Ring, Num.Negate(), Den, true);

    public Quotient<C> Abs() => new(Ring, Num.Abs(), Den, true);

    public Quotient<C> Multiply(Quotient<C> other)
    {
        if (other.IsZero())
        {
            return other;
        }

        if (Num.IsZero())
        {
            return this;
        }

        if (other.IsOne())
        {
            return this;
        }

        if (IsOne())
        {
            return other;
        }

        if (Den.IsOne() && other.Den.IsOne())
        {
            return new Quotient<C>(Ring, Num.Multiply(other.Num));
        }

        if (Den.IsOne())
        {
            GenPolynomial<C> gcd = Ring.Gcd(Num, other.Den);
            GenPolynomial<C> numerator = Ring.Divide(Num, gcd).Multiply(other.Num);
            GenPolynomial<C> denominator = Ring.Divide(other.Den, gcd);
            return new Quotient<C>(Ring, numerator, denominator, true);
        }

        if (other.Den.IsOne())
        {
            GenPolynomial<C> gcd = Ring.Gcd(other.Num, Den);
            GenPolynomial<C> numerator = Ring.Divide(other.Num, gcd).Multiply(Num);
            GenPolynomial<C> denominator = Ring.Divide(Den, gcd);
            return new Quotient<C>(Ring, numerator, denominator, true);
        }

        GenPolynomial<C> gcdNum = Ring.Gcd(Num, other.Den);
        GenPolynomial<C> numeratorPart = Ring.Divide(Num, gcdNum);
        GenPolynomial<C> otherDenominatorPart = Ring.Divide(other.Den, gcdNum);

        GenPolynomial<C> gcdDen = Ring.Gcd(Den, other.Num);
        GenPolynomial<C> denominatorPart = Ring.Divide(Den, gcdDen);
        GenPolynomial<C> otherNumeratorPart = Ring.Divide(other.Num, gcdDen);

        return new Quotient<C>(
            Ring,
            numeratorPart.Multiply(otherNumeratorPart),
            denominatorPart.Multiply(otherDenominatorPart),
            true);
    }

    public Quotient<C> Divide(Quotient<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Multiply(other.Inverse());
    }

    public Quotient<C> Inverse() => new(Ring, Den, Num, true);

    public Quotient<C> Remainder(Quotient<C> other)
    {
        if (Num.IsZero())
        {
            throw new ArithmeticException("Element not invertible: " + this);
        }

        return Ring.Zero;
    }

    public Quotient<C> Multiply(GenPolynomial<C> polynomial)
    {
        if (polynomial.IsZero())
        {
            return Ring.Zero;
        }

        if (Num.IsZero())
        {
            return this;
        }

        if (polynomial.IsOne())
        {
            return this;
        }

        GenPolynomial<C> gcd = Ring.Gcd(polynomial, Den);
        GenPolynomial<C> denominator = Den;
        GenPolynomial<C> scaled = polynomial;
        if (!gcd.IsOne())
        {
            scaled = Ring.Divide(polynomial, gcd);
            denominator = Ring.Divide(denominator, gcd);
        }

        if (IsOne())
        {
            return new Quotient<C>(Ring, scaled, denominator, true);
        }

        GenPolynomial<C> numerator = Num.Multiply(scaled);
        return new Quotient<C>(Ring, numerator, denominator, true);
    }

    public Quotient<C> Multiply(C coefficient)
    {
        if (coefficient == null || coefficient.IsZero())
        {
            return Ring.Zero;
        }

        if (Num.IsZero() || coefficient.IsOne())
        {
            return this;
        }

        return new Quotient<C>(Ring, Num.Multiply(coefficient), Den, true);
    }

    public Quotient<C> Monic()
    {
        if (Num.IsZero())
        {
            return this;
        }

        C leadingCoefficient = Num.LeadingBaseCoefficient();
        if (!leadingCoefficient.IsUnit())
        {
            return this;
        }

        C normalization = leadingCoefficient.Inverse().Abs();
        GenPolynomial<C> numerator = Num.Multiply(normalization);
        GenPolynomial<C> denominator = Den.Multiply(normalization);
        return new Quotient<C>(Ring, numerator, denominator, true);
    }

    public Quotient<C> Gcd(Quotient<C> other)
    {
        if (other.IsZero())
        {
            return this;
        }

        if (IsZero())
        {
            return other;
        }

        return Ring.One;
    }

    public Quotient<C>[] Egcd(Quotient<C> other)
    {
        Quotient<C>[] result = new Quotient<C>[3];
        if (other.IsZero())
        {
            result[0] = this;
            return result;
        }

        if (IsZero())
        {
            result[0] = other;
            return result;
        }

        GenPolynomial<C> two = Ring.Ring.FromInteger(2);
        result[0] = Ring.One;
        result[1] = Multiply(two).Inverse();
        result[2] = other.Multiply(two).Inverse();
        return result;
    }

    public int Signum() => Num.Signum();

    public int CompareTo(Quotient<C>? other)
    {
        if (other?.IsZero() != false)
        {
            return Signum();
        }

        if (IsZero())
        {
            return -other.Signum();
        }

        int signDifference = (Num.Signum() - other.Num.Signum()) / 2;
        if (signDifference != 0)
        {
            return signDifference;
        }

        GenPolynomial<C> scaledThis = Num.Multiply(other.Den);
        GenPolynomial<C> scaledOther = Den.Multiply(other.Num);
        return scaledThis.CompareTo(scaledOther);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Quotient<C> other)
        {
            return false;
        }

        return Num.Equals(other.Num) && Den.Equals(other.Den);
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(Ring);
        hash.Add(Num);
        hash.Add(Den);
        return hash.ToHashCode();
    }
}
