using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<C> CoefficientBasePseudoDivide<C>(GenPolynomial<C> polynomial, C divisor)
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

        GenPolynomial<C> result = new (polynomial.Ring);
        SortedDictionary<ExpVector, C> resultTerms = result.Terms;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            C quotient = term.Value.Divide(divisor);
            if (!quotient.IsZero())
            {
                resultTerms[term.Key] = quotient;
            }
        }

        return result;
    }
}
