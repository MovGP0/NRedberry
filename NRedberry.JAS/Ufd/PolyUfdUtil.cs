using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Polynomial ufd utilities, like conversion between different representations and Hensel lifting.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.PolyUfdUtil
/// </remarks>
public class PolyUfdUtil
{
    public static GenPolynomial<GenPolynomial<C>> IntegralFromQuotientCoefficients<C>(
        GenPolynomialRing<GenPolynomial<C>> fac, GenPolynomial<Quotient<C>> A)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<Quotient<C>> QuotientFromIntegralCoefficients<C>(
        GenPolynomialRing<Quotient<C>> fac, GenPolynomial<GenPolynomial<C>> A)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static List<GenPolynomial<Quotient<C>>> QuotientFromIntegralCoefficients<C>(
        GenPolynomialRing<Quotient<C>> fac, List<GenPolynomial<GenPolynomial<C>>> A)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<GenPolynomial<C>> IntroduceLowerVariable<C>(
        GenPolynomialRing<GenPolynomial<C>> rfac, GenPolynomial<C> A)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<GenPolynomial<C>> SubstituteFromAlgebraicCoefficients<C>(
        GenPolynomialRing<GenPolynomial<C>> rfac, GenPolynomial<AlgebraicNumber<C>> A)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<AlgebraicNumber<C>> SubstituteConvertToAlgebraicCoefficients<C>(
        GenPolynomialRing<AlgebraicNumber<C>> pfac,
        GenPolynomial<GenPolynomial<C>> B)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<AlgebraicNumber<C>> SubstituteConvertToAlgebraicCoefficients<C>(
        GenPolynomialRing<AlgebraicNumber<C>> pfac,
        GenPolynomial<C> B,
        long k)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<C> Norm<C>(GenPolynomial<AlgebraicNumber<C>> A, long k)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static void EnsureFieldProperty<C>(AlgebraicNumberRing<C> afac)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<C> SubstituteKronecker<C>(GenPolynomial<C> A, long d)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }

    public static GenPolynomial<C> BackSubstituteKronecker<C>(
        GenPolynomialRing<C> pfac, GenPolynomial<C> B, long d)
        where C : GcdRingElem<C>
    {
        throw new NotImplementedException();
    }
}
