using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Performs pseudo-division for recursive polynomials.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Recursive dividend.</param>
    /// <param name="divisor">Non-zero recursive divisor.</param>
    /// <returns>Quotient satisfying the pseudo-division identity.</returns>
    /// <remarks>Original Java method: PolyUtil#recursivePseudoDivide.</remarks>
    public static GenPolynomial<GenPolynomial<C>> RecursivePseudoDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
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

        GenPolynomial<C> leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<GenPolynomial<C>> remainder = polynomial;
        GenPolynomial<GenPolynomial<C>> quotient = new (divisor.Ring);

        while (!remainder.IsZero())
        {
            ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
            if (!leadingRemainderExponent.MultipleOf(leadingDivisorExponent))
            {
                break;
            }

            GenPolynomial<C> leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
            ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
            GenPolynomial<C> remainderModulo = BaseSparsePseudoRemainder(leadingRemainderCoefficient, leadingDivisorCoefficient);

            if (remainderModulo.IsZero() && !leadingDivisorCoefficient.IsConstant())
            {
                GenPolynomial<C> quotientCoefficient = BasePseudoDivide(leadingRemainderCoefficient, leadingDivisorCoefficient);
                quotient = quotient.Sum(quotientCoefficient, exponentDifference);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                quotient = quotient.Multiply(leadingDivisorCoefficient);
                quotient = quotient.Sum(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return quotient;
    }
}
