using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Polynomial utilities, conversion between different representations and properties of polynomials.
/// </summary>
/// <remarks>
/// <ul>
/// <li>Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolyUtil</li>
/// <li>there is one file per method group (<c>PolyUtil.METHODNAME.cs</c>) because the file would be too large otherwise</li>
/// </ul>
/// </remarks>
public static partial class PolyUtil;

internal sealed class AlgToPoly<C> : UnaryFunctor<AlgebraicNumber<C>, GenPolynomial<C>> where C : GcdRingElem<C>
{
    public GenPolynomial<C> Eval(AlgebraicNumber<C> value)
    {
        return value is null ? null! : value.Val;
    }
}

internal sealed class CoeffToAlg<C> : UnaryFunctor<C, AlgebraicNumber<C>> where C : GcdRingElem<C>
{
    private readonly AlgebraicNumberRing<C> _ring;
    private readonly GenPolynomial<C> _zeroPolynomial;

    public CoeffToAlg(AlgebraicNumberRing<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        _ring = ring;
        GenPolynomialRing<C> polynomialRing = ring.Ring;
        _zeroPolynomial = new GenPolynomial<C>(polynomialRing);
    }

    public AlgebraicNumber<C> Eval(C coefficient)
    {
        if (coefficient is null)
        {
            return _ring.GetZeroElement();
        }

        GenPolynomial<C> polynomial = _zeroPolynomial.Sum(coefficient);
        return new AlgebraicNumber<C>(_ring, polynomial);
    }
}

internal sealed class AlgebToCompl<C> : UnaryFunctor<AlgebraicNumber<C>, Complex<C>> where C : GcdRingElem<C>
{
    private readonly ComplexRing<C> _ring;

    public AlgebToCompl(ComplexRing<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        _ring = ring;
    }

    public Complex<C> Eval(AlgebraicNumber<C> value)
    {
        if (value.IsZero())
        {
            return _ring.Zero;
        }

        if (value.IsOne())
        {
            return _ring.One;
        }

        GenPolynomial<C> polynomial = value.Val;
        C real = _ring.Ring.FromInteger(0);
        C imaginary = _ring.Ring.FromInteger(0);

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            long exponent = term.Key.GetVal(0);
            if (exponent == 0)
            {
                real = term.Value;
            }
            else
            {
                imaginary = term.Value;
            }
        }

        return new Complex<C>(_ring, real, imaginary);
    }
}

internal sealed class ComplToAlgeb<C> : UnaryFunctor<Complex<C>, AlgebraicNumber<C>> where C : GcdRingElem<C>
{
    private readonly AlgebraicNumberRing<C> _ring;
    private readonly GenPolynomialRing<C> _polynomialRing;

    public ComplToAlgeb(AlgebraicNumberRing<C> ring)
    {
        ArgumentNullException.ThrowIfNull(ring);
        _ring = ring;
        _polynomialRing = ring.Ring;
    }

    public AlgebraicNumber<C> Eval(Complex<C> value)
    {
        if (value is null)
        {
            return _ring.GetZeroElement();
        }

        GenPolynomial<C> polynomial = new (_polynomialRing, value.Re);
        if (!value.Im.IsZero())
        {
            ExpVector exponent = ExpVector.Create(_polynomialRing.Nvar, _polynomialRing.Nvar - 1, 1);
            polynomial = polynomial.Sum(value.Im, exponent);
        }

        return new AlgebraicNumber<C>(_ring, polynomial);
    }
}
