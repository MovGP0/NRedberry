using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<C> BaseDensePseudoRemainder<C>(GenPolynomial<C> polynomial, GenPolynomial<C> divisor)
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

        if (divisor.Degree() <= 0)
        {
            return new GenPolynomial<C>(divisor.Ring);
        }

        long dividendDegree = polynomial.Degree(0);
        long divisorDegree = divisor.Degree(0);
        C leadingDivisorCoefficient = divisor.LeadingBaseCoefficient();
        ExpVector leadingDivisorExponent = divisor.LeadingExpVector() ?? throw new InvalidOperationException("Divisor must have a leading exponent.");
        GenPolynomial<C> remainder = polynomial;

        for (long i = dividendDegree; i >= divisorDegree; i--)
        {
            if (remainder.IsZero())
            {
                return remainder;
            }

            long remainderDegree = remainder.Degree(0);
            if (i == remainderDegree)
            {
                ExpVector leadingRemainderExponent = remainder.LeadingExpVector() ?? throw new InvalidOperationException("Remainder must have a leading exponent.");
                C leadingRemainderCoefficient = remainder.LeadingBaseCoefficient();
                ExpVector exponentDifference = leadingRemainderExponent.Subtract(leadingDivisorExponent);
                remainder = remainder.Multiply(leadingDivisorCoefficient);
                GenPolynomial<C> product = divisor.Multiply(leadingRemainderCoefficient, exponentDifference);
                remainder = remainder.Subtract(product);
            }
            else
            {
                remainder = remainder.Multiply(leadingDivisorCoefficient);
            }
        }

        return remainder;
    }
}
