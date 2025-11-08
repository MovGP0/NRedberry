using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Computes the sparse pseudo remainder for recursive polynomials.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Recursive dividend.</param>
    /// <param name="divisor">Non-zero recursive divisor.</param>
    /// <returns>The pseudo remainder satisfying <c>lc(divisor)^k * polynomial = quotient * divisor + remainder</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#recursiveSparsePseudoRemainder.</remarks>
    public static GenPolynomial<GenPolynomial<C>> RecursiveSparsePseudoRemainder<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<GenPolynomial<C>> divisor)
        where C : RingElem<C>
    {
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
            return new GenPolynomial<GenPolynomial<C>>(polynomial.Ring);
        }

        GenPolynomial<C> leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<GenPolynomial<C>> remainder = polynomial;

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

            if (remainderModulo.IsZero())
            {
                GenPolynomial<C> quotientCoefficient = BasePseudoDivide(leadingRemainderCoefficient, leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(quotientCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<GenPolynomial<C>> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
        }

        return remainder;
    }
}
