using System.Numerics;
using System.Collections;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Algebraic number ring factory based on <see cref="GenPolynomial{C}"/> with <see cref="RingFactory{T}"/> interface.
/// </summary>
/// <typeparam name="C">Coefficient type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.AlgebraicNumberRing
/// </remarks>
public class AlgebraicNumberRing<C> : RingFactory<AlgebraicNumber<C>>, IEnumerable<AlgebraicNumber<C>>, IEquatable<AlgebraicNumberRing<C>> where C : RingElem<C>
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
    /// Copy AlgebraicNumber element <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Element to copy.</param>
    /// <returns>A copy of <paramref name="value"/>.</returns>
    public AlgebraicNumber<C> Copy(AlgebraicNumber<C> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new AlgebraicNumber<C>(this, value.Val);
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

    public AlgebraicNumber<C> Zero => GetZeroElement();

    public AlgebraicNumber<C> One => GetOneElement();

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
        GenPolynomial<C> aggregate = new GenPolynomial<C>(Ring);
        GenPolynomial<C> x = Ring.Univariate(0, 1L);
        GenPolynomial<C> power = new GenPolynomial<C>(Ring, Ring.CoFac.FromInteger(1));

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
        GenPolynomial<C> polynomial = Ring.Random(bits).Monic();
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
        GenPolynomial<C> polynomial = Ring.Random(bits, random).Monic();
        return new AlgebraicNumber<C>(this, polynomial);
    }

    /// <summary>
    /// Returns the additive identity for this ring.
    /// </summary>
    public AlgebraicNumber<C> GetZeroElement()
    {
        return new AlgebraicNumber<C>(this, new GenPolynomial<C>(Ring));
    }

    /// <summary>
    /// Returns the multiplicative identity for this ring.
    /// </summary>
    public AlgebraicNumber<C> GetOneElement()
    {
        return new AlgebraicNumber<C>(this, new GenPolynomial<C>(Ring, Ring.CoFac.FromInteger(1)));
    }

    /// <summary>
    /// Depth of extension field tower.
    /// </summary>
    /// <returns>Number of nested algebraic extension fields.</returns>
    public int Depth()
    {
        int depth = 1;
        RingFactory<C> coefficientFactory = Ring.CoFac;
        Type factoryType = coefficientFactory.GetType();
        if (factoryType.IsGenericType && factoryType.GetGenericTypeDefinition() == typeof(AlgebraicNumberRing<>))
        {
            dynamic nestedRing = coefficientFactory;
            depth += (int)nestedRing.Depth();
        }

        return depth;
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
    public bool Equals(AlgebraicNumberRing<C>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return Modul.Equals(other.Modul);
    }

    public override bool Equals(object? obj)
    {
        return obj is AlgebraicNumberRing<C> other && Equals(other);
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

    public static bool operator ==(AlgebraicNumberRing<C>? left, AlgebraicNumberRing<C>? right)
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

    public static bool operator !=(AlgebraicNumberRing<C>? left, AlgebraicNumberRing<C>? right)
    {
        return !(left == right);
    }

    public IEnumerator<AlgebraicNumber<C>> GetEnumerator()
    {
        return new AlgebraicNumberEnumerator<C>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal sealed class AlgebraicNumberEnumerator<C> : IEnumerator<AlgebraicNumber<C>> where C : RingElem<C>
{
    private readonly IEnumerator<List<C>> _iterator;
    private readonly List<GenPolynomial<C>> _powers;
    private readonly AlgebraicNumberRing<C> _ring;
    private AlgebraicNumber<C>? _current;

    public AlgebraicNumberEnumerator(AlgebraicNumberRing<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        _ring = ring;

        long degree = ring.Modul.Degree(0);
        _powers = new List<GenPolynomial<C>>((int)degree);
        for (long j = degree - 1; j >= 0L; j--)
        {
            _powers.Add(ring.Ring.Univariate(0, j));
        }

        RingFactory<C> coefficientFactory = ring.Ring.CoFac;
        if (coefficientFactory is not IEnumerable<C> coefficientEnumerable)
        {
            throw new ArgumentException("only for iterable coefficients implemented", nameof(ring));
        }

        List<IEnumerable<C>> components = new((int)degree);
        for (long j = 0L; j < degree; j++)
        {
            components.Add(coefficientEnumerable);
        }

        if (coefficientFactory.IsFinite())
        {
            _iterator = new CartesianProduct<C>(components).GetEnumerator();
        }
        else
        {
            _iterator = new CartesianProductInfinite<C>(components).GetEnumerator();
        }
    }

    public AlgebraicNumber<C> Current => _current ?? throw new InvalidOperationException("Enumeration has not started.");

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (!_iterator.MoveNext())
        {
            _current = null;
            return false;
        }

        List<C> coefficients = _iterator.Current;
        GenPolynomial<C> polynomial = new(_ring.Ring);
        int i = 0;
        foreach (GenPolynomial<C> power in _powers)
        {
            C coefficient = coefficients[i++];
            if (coefficient.IsZero())
            {
                continue;
            }

            polynomial = polynomial.Sum(power.Multiply(coefficient));
        }

        _current = new AlgebraicNumber<C>(_ring, polynomial);
        return true;
    }

    public void Reset()
    {
        throw new NotSupportedException("Reset is not supported for algebraic number enumerators.");
    }

    public void Dispose()
    {
        _iterator.Dispose();
    }
}
