using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Generic Complex ring factory implementing the RingFactory interface. Objects of this class are immutable.
/// </summary>
/// <typeparam name="C">Base ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ComplexRing
/// </remarks>
public class ComplexRing<C> : RingFactory<Complex<C>>, IEquatable<ComplexRing<C>> where C : RingElem<C>
{
    private static readonly Random s_random = new();

    /// <summary>
    /// Initializes the complex ring factory with a factory for the real coefficients.
    /// </summary>
    /// <param name="ring">Factory for the underlying real and imaginary parts.</param>
    public ComplexRing(RingFactory<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        Ring = ring;
    }

    /// <summary>
    /// Complex class elements factory data structure.
    /// </summary>
    public RingFactory<C> Ring { get; }

    /// <inheritdoc />
    public List<Complex<C>> Generators()
    {
        List<C> gens = Ring.Generators();
        List<Complex<C>> result = new (gens.Count + 1);
        foreach (C x in gens)
        {
            result.Add(new Complex<C>(this, x));
        }

        result.Add(Imag);
        return result;
    }

    /// <summary>
    /// Corresponding algebraic number ring.
    /// </summary>
    /// <returns>Algebraic number ring.</returns>
    public AlgebraicNumberRing<C> AlgebraicRing()
    {
        GenPolynomialRing<C> polynomialRing = new(
            Ring,
            1,
            new TermOrder(TermOrder.INVLEX),
            new[] { "I" });
        GenPolynomial<C> iPolynomial = polynomialRing
            .Univariate(0, 2L)
            .Sum(polynomialRing.GetOneCoefficient());
        return new AlgebraicNumberRing<C>(iPolynomial, Ring.IsField());
    }

    /// <inheritdoc />
    public bool IsFinite()
    {
        return Ring.IsFinite();
    }

    /// <summary>
    /// Copy Complex element <paramref name="c"/>.
    /// </summary>
    /// <param name="c">Element to copy.</param>
    /// <returns>A copy of <paramref name="c"/>.</returns>
    public static Complex<C> Clone(Complex<C> c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return c.Clone();
    }

    /// <summary>
    /// Copy Complex element <paramref name="c"/>.
    /// </summary>
    /// <param name="c">Element to copy.</param>
    /// <returns>A copy of <paramref name="c"/>.</returns>
    public Complex<C> Copy(Complex<C> c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return new Complex<C>(this, c.Re, c.Im);
    }

    /// <summary>
    /// Zero element.
    /// </summary>
    public Complex<C> Zero => new (this);

    /// <summary>
    /// One element.
    /// </summary>
    public Complex<C> One => new (this, Ring.FromInteger(1));

    /// <summary>
    /// Imaginary unit.
    /// </summary>
    public Complex<C> Imag => new (this, Ring.FromInteger(0), Ring.FromInteger(1));

    /// <inheritdoc />
    public bool IsCommutative()
    {
        return Ring.IsCommutative();
    }

    /// <inheritdoc />
    public bool IsAssociative()
    {
        return Ring.IsAssociative();
    }

    /// <inheritdoc />
    public bool IsField()
    {
        return Ring.IsField();
    }

    /// <inheritdoc />
    public BigInteger Characteristic()
    {
        return Ring.Characteristic();
    }

    /// <inheritdoc />
    public Complex<C> FromInteger(long a)
    {
        return new Complex<C>(this, Ring.FromInteger(a));
    }

    /// <inheritdoc />
    public Complex<C> FromInteger(BigInteger a)
    {
        return new Complex<C>(this, Ring.FromInteger(a));
    }

    Complex<C> ElemFactory<Complex<C>>.FromInteger(BigInteger a)
    {
        return FromInteger(a);
    }

    /// <summary>
    /// Generate a random complex number with default size.
    /// </summary>
    /// <returns>A random complex number.</returns>
    public Complex<C> Random()
    {
        return Random(16);
    }

    /// <inheritdoc />
    public Complex<C> Random(int k)
    {
        return Random(k, s_random);
    }

    /// <inheritdoc />
    public Complex<C> Random(int k, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        C r = Ring.Random(k, rnd);
        C i = Ring.Random(k, rnd);
        return new Complex<C>(this, r, i);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Complex[{Ring}]";
    }

    /// <inheritdoc />
    public bool Equals(ComplexRing<C>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return Ring.Equals(other.Ring);
    }

    public override bool Equals(object? obj)
    {
        return obj is ComplexRing<C> other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Ring.GetHashCode();
    }

    public static bool operator ==(ComplexRing<C>? left, ComplexRing<C>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ComplexRing<C>? left, ComplexRing<C>? right)
    {
        return !(left == right);
    }
}
