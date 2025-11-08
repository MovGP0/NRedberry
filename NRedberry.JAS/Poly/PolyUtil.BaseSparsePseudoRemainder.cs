using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Computes the sparse pseudo remainder of two univariate polynomials.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Dividend polynomial.</param>
    /// <param name="divisor">Non-zero divisor polynomial.</param>
    /// <returns>Remainder satisfying <c>lc(divisor)^k * dividend = quotient * divisor + remainder</c> with <c>k ≤ deg(dividend)-deg(divisor)</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#baseSparsePseudoRemainder.</remarks>
    public static GenPolynomial<C> BaseSparsePseudoRemainder<C>(GenPolynomial<C> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero())
        {
            return polynomial;
        }

        if (divisor.IsOne())
        {
            return new GenPolynomial<C>(divisor.Ring);
        }

        C leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<C> remainder = polynomial;

        while (!remainder.IsZero())
        {
            ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
            if (!leadingRemainderExponent.MultipleOf(leadingDivisorExponent))
            {
                break;
            }

            C leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
            C remainderModulo = leadingRemainderCoefficient.Remainder(leadingDivisorCoefficient);
            if (remainderModulo.IsZero())
            {
                C quotientCoefficient = leadingRemainderCoefficient.Divide(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return remainder;
    }
}
