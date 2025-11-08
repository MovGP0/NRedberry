using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<C> EvaluateMain<C>(
        GenPolynomialRing<C> coefficientRing,
        GenPolynomial<C> polynomial,
        C value)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(coefficientRing);
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return new GenPolynomial<C>(coefficientRing);
        }

        GenPolynomialRing<GenPolynomial<C>> recursiveRing = coefficientRing.Recursive(1);
        if (recursiveRing.Nvar + coefficientRing.Nvar != polynomial.Ring.Nvar)
        {
            throw new ArgumentException("evaluateMain number of variables mismatch", nameof(polynomial));
        }

        GenPolynomial<GenPolynomial<C>> recursivePolynomial = Recursive(recursiveRing, polynomial);
        return EvaluateMainRecursive(coefficientRing, recursivePolynomial, value);
    }

    private static GenPolynomial<C> EvaluateMainRecursive<C>(
        GenPolynomialRing<C> coefficientRing,
        GenPolynomial<GenPolynomial<C>> polynomial,
        C value)
        where C : RingElem<C>
    {
        if (polynomial.IsZero())
        {
            return new GenPolynomial<C>(coefficientRing);
        }

        if (polynomial.Ring.Nvar != 1)
        {
            throw new ArgumentException("evaluateMain requires a recursive univariate polynomial.", nameof(polynomial));
        }

        if (value is null || value.IsZero())
        {
            return polynomial.TrailingBaseCoefficient();
        }

        GenPolynomial<C>? result = null;
        long previousExponent = -1;
        long lastExponent = -1;

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            long currentExponent = term.Key.GetVal(0);
            if (result is null)
            {
                result = term.Value;
            }
            else
            {
                for (long i = currentExponent; i < previousExponent; i++)
                {
                    result = result.Multiply(value);
                }

                result = result.Sum(term.Value);
            }

            previousExponent = currentExponent;
            lastExponent = currentExponent;
        }

        if (result is null)
        {
            return new GenPolynomial<C>(coefficientRing);
        }

        for (long i = 0; i < lastExponent; i++)
        {
            result = result.Multiply(value);
        }

        return result;
    }
}
