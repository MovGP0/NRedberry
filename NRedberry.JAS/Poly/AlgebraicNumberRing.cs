using System;
using System.Collections.Generic;
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

    public GenPolynomialRing<C> Ring { get; }

    public GenPolynomial<C> Modul { get; }

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

    public AlgebraicNumberRing(GenPolynomial<C> modul, bool isField)
        : this(modul)
    {
        _isField = isField ? 1 : 0;
    }

    public GenPolynomial<C> GetModul()
    {
        return Modul;
    }

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

    public AlgebraicNumber<C> GetGenerator()
    {
        GenPolynomial<C> generator = Ring.Univariate(0);
        return new AlgebraicNumber<C>(this, generator);
    }

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

    public bool IsFinite()
    {
        return Ring.CoFac.IsFinite();
    }

    public bool IsCommutative()
    {
        return Ring.IsCommutative();
    }

    public bool IsAssociative()
    {
        return Ring.IsAssociative();
    }

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

    public int GetField()
    {
        return _isField;
    }

    public BigInteger Characteristic()
    {
        return Ring.Characteristic();
    }

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

    public AlgebraicNumber<C> FillFromInteger(long value)
    {
        return FillFromInteger(new BigInteger(value));
    }

    public AlgebraicNumber<C> FromInteger(long value)
    {
        return new AlgebraicNumber<C>(this, Ring.FromInteger(value));
    }

    public AlgebraicNumber<C> FromInteger(BigInteger value)
    {
        return new AlgebraicNumber<C>(this, Ring.FromInteger(value));
    }

    AlgebraicNumber<C> ElemFactory<AlgebraicNumber<C>>.FromInteger(BigInteger value)
    {
        return FromInteger(value);
    }

    public AlgebraicNumber<C> Random()
    {
        return Random(3);
    }

    public AlgebraicNumber<C> Random(int bits)
    {
        GenPolynomial<C> polynomial = Ring.Random(bits);
        return new AlgebraicNumber<C>(this, polynomial);
    }

    public AlgebraicNumber<C> Random(int bits, Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
        GenPolynomial<C> polynomial = Ring.Random(bits, random);
        return new AlgebraicNumber<C>(this, polynomial);
    }

    public AlgebraicNumber<C> GetZeroElement()
    {
        return new AlgebraicNumber<C>(this, GenPolynomialRing<C>.Zero);
    }

    public AlgebraicNumber<C> GetOneElement()
    {
        return new AlgebraicNumber<C>(this, GenPolynomialRing<C>.One);
    }

    public override string ToString()
    {
        return $"AlgebraicNumberRing[ {Modul} | isField={_isField} :: {Ring} ]";
    }

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

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Modul);
        hashCode.Add(Ring);
        hashCode.Add(_isField);
        return hashCode.ToHashCode();
    }
}
