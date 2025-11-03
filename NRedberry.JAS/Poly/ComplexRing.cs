using System;
using System.Collections.Generic;
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
public class ComplexRing<C> : RingFactory<Complex<C>> where C : RingElem<C>
{
    private static readonly Random RandomSource = new ();

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
        throw new NotImplementedException("Algebraic number ring construction requires GenPolynomial support.");
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
        return Random(k, RandomSource);
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
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not ComplexRing<C> other)
        {
            return false;
        }

        return Ring.Equals(other.Ring);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Ring.GetHashCode();
    }
}
