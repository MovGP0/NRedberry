using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Polynomial ufd utilities, like conversion between different representations
/// and Hensel lifting.
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
        ArgumentNullException.ThrowIfNull(fac);
        ArgumentNullException.ThrowIfNull(A);

        GenPolynomial<GenPolynomial<C>> result = new (fac);
        if (A.IsZero())
        {
            return result;
        }

        GenPolynomial<C>? lcmDenominator = null;
        int denominatorSign = 0;
        GreatestCommonDivisor<C> gcdEngine = new GreatestCommonDivisorSubres<C>();

        foreach (Quotient<C> term in A.Terms.Values)
        {
            GenPolynomial<C> denominator = term.Den;
            if (lcmDenominator is null)
            {
                lcmDenominator = denominator;
                denominatorSign = denominator.Signum();
            }
            else
            {
                GenPolynomial<C> gcd = gcdEngine.Gcd(lcmDenominator, denominator);
                GenPolynomial<C> factor = denominator.Divide(gcd);
                lcmDenominator = lcmDenominator.Multiply(factor);
            }
        }

        if (lcmDenominator is null)
        {
            return result;
        }

        if (denominatorSign < 0)
        {
            lcmDenominator = lcmDenominator.Negate();
        }

        foreach (KeyValuePair<ExpVector, Quotient<C>> term in A.Terms)
        {
            Quotient<C> quotient = term.Value;
            GenPolynomial<C> scale = lcmDenominator.Divide(quotient.Den);
            GenPolynomial<C> coefficient = quotient.Num.Multiply(scale);
            result.DoPutToMap(term.Key, coefficient);
        }

        return result;
    }

    public static GenPolynomial<Quotient<C>> QuotientFromIntegralCoefficients<C>(
        GenPolynomialRing<Quotient<C>> fac, GenPolynomial<GenPolynomial<C>> A)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        ArgumentNullException.ThrowIfNull(A);

        GenPolynomial<Quotient<C>> result = new (fac);
        if (A.IsZero())
        {
            return result;
        }

        if (fac.CoFac is not QuotientRing<C> quotientRing)
        {
            throw new ArgumentException("Coefficient factory must be a QuotientRing instance.", nameof(fac));
        }

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in A.Terms)
        {
            GenPolynomial<C> coefficient = term.Value;
            if (coefficient.IsZero())
            {
                continue;
            }

            Quotient<C> fraction = new (quotientRing, coefficient);
            if (!fraction.IsZero())
            {
                result.DoPutToMap(term.Key, fraction);
            }
        }

        return result;
    }

    public static List<GenPolynomial<Quotient<C>>> QuotientFromIntegralCoefficients<C>(
        GenPolynomialRing<Quotient<C>> fac, List<GenPolynomial<GenPolynomial<C>>> A)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(fac);
        ArgumentNullException.ThrowIfNull(A);

        List<GenPolynomial<Quotient<C>>> result = new (A.Count);
        foreach (GenPolynomial<GenPolynomial<C>> polynomial in A)
        {
            result.Add(polynomial is null ? new GenPolynomial<Quotient<C>>(fac) : QuotientFromIntegralCoefficients(fac, polynomial));
        }

        return result;
    }

    public static GenPolynomial<GenPolynomial<C>> IntroduceLowerVariable<C>(
        GenPolynomialRing<GenPolynomial<C>> rfac, GenPolynomial<C> A)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(rfac);
        ArgumentNullException.ThrowIfNull(A);

        GenPolynomial<GenPolynomial<C>> embedded = new GenPolynomial<GenPolynomial<C>>(rfac).Sum(A);
        if (embedded.IsZero())
        {
            return embedded;
        }

        return PolyUtil.SwitchVariables(embedded);
    }

    public static GenPolynomial<GenPolynomial<C>> SubstituteFromAlgebraicCoefficients<C>(
        GenPolynomialRing<GenPolynomial<C>> rfac, GenPolynomial<AlgebraicNumber<C>> A, long k)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(rfac);
        ArgumentNullException.ThrowIfNull(A);

        if (A.IsZero())
        {
            return new GenPolynomial<GenPolynomial<C>>(rfac);
        }

        GenPolynomialRing<AlgebraicNumber<C>> algebraicPolynomialRing = A.Ring;
        GenPolynomial<AlgebraicNumber<C>> variable = algebraicPolynomialRing.Univariate(0);
        if (algebraicPolynomialRing.CoFac is not AlgebraicNumberRing<C> algebraicRing)
        {
            throw new ArgumentException("Polynomial must be defined over an AlgebraicNumberRing.", nameof(A));
        }

        AlgebraicNumber<C> generator = algebraicRing.GetGenerator();
        AlgebraicNumber<C> scalar = algebraicRing.FromInteger(k);
        GenPolynomial<AlgebraicNumber<C>> substitution = variable.Subtract(scalar.Multiply(generator));

        GenPolynomial<AlgebraicNumber<C>> substituted = PolyUtil.SubstituteMain(A, substitution);
        GenPolynomial<GenPolynomial<C>> lifted = PolyUtil.FromAlgebraicCoefficients(rfac, substituted);
        return PolyUtil.SwitchVariables(lifted);
    }

    public static GenPolynomial<AlgebraicNumber<C>> SubstituteConvertToAlgebraicCoefficients<C>(
        GenPolynomialRing<AlgebraicNumber<C>> pfac,
        GenPolynomial<GenPolynomial<C>> B)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(pfac);
        ArgumentNullException.ThrowIfNull(B);

        GenPolynomial<AlgebraicNumber<C>> result = new (pfac);
        if (B.IsZero())
        {
            return result;
        }

        if (pfac.CoFac is not AlgebraicNumberRing<C> algebraicRing)
        {
            throw new ArgumentException("pfac must be backed by an AlgebraicNumberRing.", nameof(pfac));
        }

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in B.Terms)
        {
            if (term.Value.IsZero())
            {
                continue;
            }

            AlgebraicNumber<C> coefficient = new (algebraicRing, term.Value);
            result.DoPutToMap(term.Key, coefficient);
        }

        return result;
    }

    public static GenPolynomial<AlgebraicNumber<C>> SubstituteConvertToAlgebraicCoefficients<C>(
        GenPolynomialRing<AlgebraicNumber<C>> pfac,
        GenPolynomial<C> B,
        long k)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(pfac);
        ArgumentNullException.ThrowIfNull(B);

        GenPolynomial<AlgebraicNumber<C>> result = new (pfac);
        if (B.IsZero())
        {
            return result;
        }

        if (pfac.CoFac is not AlgebraicNumberRing<C> algebraicRing)
        {
            throw new ArgumentException("pfac must be backed by an AlgebraicNumberRing.", nameof(pfac));
        }

        GenPolynomial<AlgebraicNumber<C>> converted = PolyUtil.ConvertToAlgebraicCoefficients(pfac, B);
        GenPolynomial<AlgebraicNumber<C>> variable = pfac.Univariate(0);
        AlgebraicNumber<C> generator = algebraicRing.GetGenerator();
        AlgebraicNumber<C> scalar = algebraicRing.FromInteger(k);
        GenPolynomial<AlgebraicNumber<C>> substitution = variable.Sum(scalar.Multiply(generator));
        return PolyUtil.SubstituteMain(converted, substitution);
    }

    public static GenPolynomial<C> Norm<C>(GenPolynomial<AlgebraicNumber<C>> A, long k)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(A);

        GenPolynomialRing<AlgebraicNumber<C>> polynomialRing = A.Ring;
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("Norm is defined only for univariate polynomials.", nameof(A));
        }

        if (polynomialRing.CoFac is not AlgebraicNumberRing<C> algebraicRing)
        {
            throw new ArgumentException("Polynomial must be defined over an AlgebraicNumberRing.", nameof(A));
        }

        GenPolynomial<C> minimalPolynomial = algebraicRing.Modul;
        GenPolynomialRing<C> coefficientRing = algebraicRing.Ring;
        if (A.IsZero())
        {
            return new GenPolynomial<C>(coefficientRing);
        }

        AlgebraicNumber<C> leading = A.LeadingBaseCoefficient();
        if (!leading.IsOne())
        {
            A = A.Monic();
        }

        GenPolynomialRing<GenPolynomial<C>> recursiveRing = new (
            coefficientRing,
            polynomialRing.Nvar,
            polynomialRing.Tord,
            polynomialRing.GetVars());

        GenPolynomial<GenPolynomial<C>> minimalRecursive = IntroduceLowerVariable(recursiveRing, minimalPolynomial);
        GenPolynomial<GenPolynomial<C>> substituted = SubstituteFromAlgebraicCoefficients(recursiveRing, A, k);
        GenPolynomial<GenPolynomial<C>> monicSubstituted = PolyUtil.Monic(substituted) ?? substituted;

        GreatestCommonDivisorSubres<C> engine = new ();
        GenPolynomial<GenPolynomial<C>> resultant = engine.RecursiveUnivariateResultant(monicSubstituted, minimalRecursive);
        GenPolynomial<C> norm = resultant.LeadingBaseCoefficient();
        return norm.Monic();
    }

    public static void EnsureFieldProperty<C>(AlgebraicNumberRing<C> afac)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(afac);
        if (afac.GetField() != -1)
        {
            return;
        }

        if (!afac.Ring.CoFac.IsField())
        {
            afac.SetField(false);
            return;
        }

        Factorization<C> factorization = FactorFactory.GetImplementation(afac.Ring);
        bool isIrreducible = factorization.IsIrreducible(afac.Modul);
        afac.SetField(isIrreducible);
    }

    public static GenPolynomial<C> SubstituteKronecker<C>(GenPolynomial<C> A, long d)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(A);
        if (d <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(d), "Substitution base must be positive.");
        }

        RingFactory<C> coefficientFactory = A.Ring.CoFac;
        GenPolynomialRing<C> univariateRing = new (coefficientFactory, 1);
        GenPolynomial<C> result = new (univariateRing);

        if (A.IsZero())
        {
            return result;
        }

        foreach (KeyValuePair<ExpVector, C> term in A.Terms)
        {
            long packedExponent = 0L;
            long basePower = 1L;
            ExpVector exponentVector = term.Key;
            int length = exponentVector.Length();
            for (int i = 0; i < length; i++)
            {
                packedExponent += exponentVector.GetVal(i) * basePower;
                basePower *= d;
            }

            ExpVector univariateExponent = ExpVector.Create(1, 0, packedExponent);
            result.DoPutToMap(univariateExponent, term.Value);
        }

        return result;
    }

    public static GenPolynomial<C> BackSubstituteKronecker<C>(
        GenPolynomialRing<C> pfac, GenPolynomial<C> B, long d)
        where C : GcdRingElem<C>
    {
        ArgumentNullException.ThrowIfNull(pfac);
        ArgumentNullException.ThrowIfNull(B);
        if (d <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(d), "Substitution base must be positive.");
        }

        GenPolynomial<C> result = new (pfac);
        if (B.IsZero())
        {
            return result;
        }

        int variables = pfac.Nvar;
        foreach (KeyValuePair<ExpVector, C> term in B.Terms)
        {
            long packed = term.Key.GetVal(0);
            ExpVector exponentVector = ExpVector.Create(variables);
            for (int i = 0; i < variables; i++)
            {
                long exponent = packed % d;
                packed /= d;
                exponentVector = exponentVector.Subst(i, exponent);
            }

            result.DoPutToMap(exponentVector, term.Value);
        }

        return result;
    }
}
