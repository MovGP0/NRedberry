using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Performs the sparse pseudo-division of two univariate polynomials (or exact divisions in multivariate cases).
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Dividend polynomial.</param>
    /// <param name="divisor">Non-zero divisor polynomial.</param>
    /// <returns>Quotient satisfying <c>lc(divisor)^k * dividend = quotient * divisor + remainder</c> for some <c>k ≤ deg(dividend)-deg(divisor)</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#basePseudoDivide.</remarks>
    public static GenPolynomial<C> BasePseudoDivide<C>(GenPolynomial<C> polynomial, GenPolynomial<C> divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"{polynomial} division by zero {divisor}");
        }

        if (polynomial.IsZero() || divisor.IsOne())
        {
            return polynomial;
        }

        C leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<C> remainder = polynomial;
        GenPolynomial<C> quotient = new (divisor.Ring);

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
                quotient = quotient.Sum(quotientCoefficient, exponentDifference);
                GenPolynomial<C> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                quotient = quotient.Multiply(leadingDivisorCoefficient);
                quotient = quotient.Sum(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return quotient;
    }
}
