using System.Numerics;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Algebraic number ring factory based on <see cref="GenPolynomial{C}"/> with <see cref="RingFactory{T}"/> interface.
/// </summary>
/// <typeparam name="C">Coefficient type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.AlgebraicNumberRing
/// </remarks>
public class AlgebraicNumberRing<C> : RingFactory<AlgebraicNumber<C>> where C : RingElem<C>
{
    private int _isField = -1;

    /// <summary>
    /// Underlying polynomial ring used for the coefficients.
    /// </summary>
    public GenPolynomialRing<C> Ring { get; }

    /// <summary>
    /// Modulus polynomial defining the algebraic extension.
    /// </summary>
    public GenPolynomial<C> Modul { get; }

    /// <summary>
    /// Creates an algebraic number ring for the provided modulus polynomial.
    /// </summary>
    /// <param name="modul">Polynomial that defines the extension.</param>
    public AlgebraicNumberRing(GenPolynomial<C> modul)
    {
        ArgumentNullException.ThrowIfNull(modul);

        Ring = modul.Ring ?? throw new ArgumentException("Modulus must carry a polynomial ring.", nameof(modul));
        Modul = modul;

        if (Ring.Nvar > 1)
        {
            throw new ArgumentException("Only univariate polynomials allowed.", nameof(modul));
        }
    }

    /// <summary>
    /// Creates an algebraic number ring and optionally hints whether the modulus is irreducible.
    /// </summary>
    /// <param name="modul">Polynomial that defines the extension.</param>
    /// <param name="isField"><c>true</c> if the modulus is known to be a field polynomial.</param>
    public AlgebraicNumberRing(GenPolynomial<C> modul, bool isField)
        : this(modul)
    {
        _isField = isField ? 1 : 0;
    }

    /// <summary>
    /// Returns the modulus polynomial that defines this algebraic extension.
    /// </summary>
    public GenPolynomial<C> GetModul()
    {
        return Modul;
    }

    /// <summary>
    /// Returns a copy of the algebraic number by invoking its own <see cref="AlgebraicNumber{C}.Clone"/> logic.
    /// </summary>
    /// <param name="value">Element to copy.</param>
    public static AlgebraicNumber<C> Clone(AlgebraicNumber<C> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Clone();
    }

    /// <summary>
    /// Static zero is undefined for algebraic number rings without context.
    /// </summary>
    public static AlgebraicNumber<C> Zero => throw new InvalidOperationException("Use a specific AlgebraicNumberRing instance to obtain zero.");

    /// <summary>
    /// Static one is undefined for algebraic number rings without context.
    /// </summary>
    public static AlgebraicNumber<C> One => throw new InvalidOperationException("Use a specific AlgebraicNumberRing instance to obtain one.");

    /// <summary>
    /// Returns the main algebraic generator (the root of the modulus polynomial).
    /// </summary>
    public AlgebraicNumber<C> GetGenerator()
    {
        GenPolynomial<C> generator = Ring.Univariate(0);
        return new AlgebraicNumber<C>(this, generator);
    }

    /// <summary>
    /// Returns a list of generating elements for this algebraic structure.
    /// </summary>
    public List<AlgebraicNumber<C>> Generators()
    {
        List<GenPolynomial<C>> polynomialGenerators = Ring.Generators();
        List<AlgebraicNumber<C>> generators = new(polynomialGenerators.Count);

        foreach (GenPolynomial<C> generator in polynomialGenerators)
        {
            generators.Add(new AlgebraicNumber<C>(this, generator));
        }

        return generators;
    }

    /// <summary>
    /// Queries whether this algebraic number ring is finite.
    /// </summary>
    public bool IsFinite()
    {
        return Ring.CoFac.IsFinite();
    }

    /// <summary>
    /// Queries whether the underlying coefficient ring is commutative.
    /// </summary>
    public bool IsCommutative()
    {
        return Ring.IsCommutative();
    }

    /// <summary>
    /// Queries whether the underlying coefficient ring is associative.
    /// </summary>
    public bool IsAssociative()
    {
        return Ring.IsAssociative();
    }

    /// <summary>
    /// Returns true when the modulus is known to generate a field extension.
    /// </summary>
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

        if (!Ring.CoFac.IsField())
        {
            _isField = 0;
        }

        return false;
    }

    /// <summary>
    /// Updates the cached information whether the modulus defines a field.
    /// </summary>
    /// <param name="value"><c>true</c> to mark as field, <c>false</c> otherwise.</param>
    public void SetField(bool value)
    {
        if (_isField > 0 && value)
        {
            return;
        }

        if (_isField == 0 && !value)
        {
            return;
        }

        _isField = value ? 1 : 0;
    }

    /// <summary>
    /// Retrieves the cached field indicator (-1 for unknown, 0 for non-field, 1 for field).
    /// </summary>
    public int GetField()
    {
        return _isField;
    }

    /// <summary>
    /// Retrieves the characteristic of the coefficient ring.
    /// </summary>
    public BigInteger Characteristic()
    {
        return Ring.Characteristic();
    }

    /// <summary>
    /// Builds an algebraic number representing the given integer reduced modulo the modulus.
    /// </summary>
    /// <param name="value">Integer to encode.</param>
    public AlgebraicNumber<C> FillFromInteger(BigInteger value)
    {
        BigInteger characteristic = Characteristic();
        if (characteristic.Sign == 0)
        {
            return new AlgebraicNumber<C>(this, Ring.FromInteger(value));
        }

        BigInteger remaining = value;
        GenPolynomial<C> aggregate = GenPolynomialRing<C>.Zero;
        GenPolynomial<C> x = Ring.Univariate(0, 1L);
        GenPolynomial<C> power = GenPolynomialRing<C>.One;

        while (remaining != BigInteger.Zero)
        {
            BigInteger quotient = BigInteger.DivRem(remaining, characteristic, out BigInteger remainder);
            GenPolynomial<C> coefficient = Ring.FromInteger(remainder);
            aggregate = aggregate.Sum(power.Multiply(coefficient));
            power = power.Multiply(x);
            remaining = quotient;
        }

        return new AlgebraicNumber<C>(this, aggregate);
    }

    /// <summary>
    /// Builds an algebraic number representing a 64-bit integer.
    /// </summary>
    /// <param name="value">Integer to encode.</param>
    public AlgebraicNumber<C> FillFromInteger(long value)
    {
        return FillFromInteger(new BigInteger(value));
    }

    /// <summary>
    /// Embeds a 64-bit integer into the algebraic number ring.
    /// </summary>
    /// <param name="value">Integer to embed.</param>
    public AlgebraicNumber<C> FromInteger(long value)
    {
        return new AlgebraicNumber<C>(this, Ring.FromInteger(value));
    }

    /// <summary>
    /// Embeds a <see cref="BigInteger"/> into the algebraic number ring.
    /// </summary>
    /// <param name="value">Integer to embed.</param>
    public AlgebraicNumber<C> FromInteger(BigInteger value)
    {
        return new AlgebraicNumber<C>(this, Ring.FromInteger(value));
    }

    AlgebraicNumber<C> ElemFactory<AlgebraicNumber<C>>.FromInteger(BigInteger value)
    {
        return FromInteger(value);
    }

    /// <summary>
    /// Generates a pseudo-random algebraic number with default bit length.
    /// </summary>
    public AlgebraicNumber<C> Random()
    {
        return Random(3);
    }

    /// <summary>
    /// Generates a pseudo-random algebraic number with the specified bit length.
    /// </summary>
    /// <param name="bits">Bit length for the random polynomial.</param>
    public AlgebraicNumber<C> Random(int bits)
    {
        GenPolynomial<C> polynomial = Ring.Random(bits);
        return new AlgebraicNumber<C>(this, polynomial);
    }

    /// <summary>
    /// Generates a pseudo-random algebraic number with the specified bit length using the supplied RNG.
    /// </summary>
    /// <param name="bits">Bit length for the random polynomial.</param>
    /// <param name="random">Random source.</param>
    public AlgebraicNumber<C> Random(int bits, Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
        GenPolynomial<C> polynomial = Ring.Random(bits, random);
        return new AlgebraicNumber<C>(this, polynomial);
    }

    /// <summary>
    /// Returns the additive identity for this ring.
    /// </summary>
    public AlgebraicNumber<C> GetZeroElement()
    {
        return new AlgebraicNumber<C>(this, GenPolynomialRing<C>.Zero);
    }

    /// <summary>
    /// Returns the multiplicative identity for this ring.
    /// </summary>
    public AlgebraicNumber<C> GetOneElement()
    {
        return new AlgebraicNumber<C>(this, GenPolynomialRing<C>.One);
    }

    /// <summary>
    /// Computes the total degree of the algebraic extension, taking nested extensions into account.
    /// </summary>
    public long TotalExtensionDegree()
    {
        long degree = Modul.Degree(0);
        RingFactory<C> coefficientFactory = Ring.CoFac;
        Type factoryType = coefficientFactory.GetType();
        if (factoryType.IsGenericType && factoryType.GetGenericTypeDefinition() == typeof(AlgebraicNumberRing<>))
        {
            dynamic nestedRing = coefficientFactory;
            long nestedDegree = (long)nestedRing.TotalExtensionDegree();
            degree = degree == 0 ? nestedDegree : degree * nestedDegree;
        }

        return degree;
    }

    /// <summary>
    /// Returns a debug-friendly representation of the modulus and coefficient ring.
    /// </summary>
    public override string ToString()
    {
        return $"AlgebraicNumberRing[ {Modul} | isField={_isField} :: {Ring} ]";
    }

    /// <summary>
    /// Compares two algebraic number rings by their modulus.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not AlgebraicNumberRing<C> other)
        {
            return false;
        }

        return Modul.Equals(other.Modul);
    }

    /// <summary>
    /// Computes the hash code from the modulus, coefficient ring, and cached field flag.
    /// </summary>
    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Modul);
        hashCode.Add(Ring);
        hashCode.Add(_isField);
        return hashCode.ToHashCode();
    }
}
