using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Substitutes a polynomial into the main variable of a univariate polynomial.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Univariate polynomial.</param>
    /// <param name="substitution">Polynomial to insert for the variable.</param>
    /// <returns>The composition <c>polynomial(substitution)</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#substituteMain.</remarks>
    public static GenPolynomial<C> SubstituteMain<C>(GenPolynomial<C> polynomial, GenPolynomial<C> substitution)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(substitution);
        return SubstituteUnivariate(polynomial, substitution);
    }

    /// <summary>
    /// Helper that composes two univariate polynomials via repeated multiplication.
    /// </summary>
    private static GenPolynomial<C> SubstituteUnivariate<C>(GenPolynomial<C> polynomial, GenPolynomial<C> substitution)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(substitution);

        GenPolynomialRing<C> polynomialRing = polynomial.Ring;
        if (polynomialRing.Nvar > 1)
        {
            throw new ArgumentException("substituteMain only supports univariate source polynomials.", nameof(polynomial));
        }

        if (polynomial.IsZero() || polynomial.IsConstant())
        {
            return polynomial;
        }

        GenPolynomialRing<C> resultRing = substitution.Ring.Nvar > 1 ? substitution.Ring : polynomialRing;

        GenPolynomial<C>? accumulator = null;
        long previousExponent = -1;
        long lastExponent = -1;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            long exponent = term.Key.GetVal(0);
            if (accumulator is null)
            {
                accumulator = new GenPolynomial<C>(resultRing).Sum(term.Value);
            }
            else
            {
                for (long power = exponent; power < previousExponent; power++)
                {
                    accumulator = accumulator.Multiply(substitution);
                }

                accumulator = accumulator.Sum(term.Value);
            }

            previousExponent = exponent;
            lastExponent = exponent;
        }

        if (accumulator is null)
        {
            return new GenPolynomial<C>(resultRing);
        }

        for (long i = 0; i < lastExponent; i++)
        {
            accumulator = accumulator.Multiply(substitution);
        }

        return accumulator;
    }
}
