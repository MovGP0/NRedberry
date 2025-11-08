using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Divides a recursive polynomial by an element of the coefficient ring using coefficient pseudo-division.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Recursive polynomial to divide.</param>
    /// <param name="divisor">Coefficient ring element.</param>
    /// <returns>The quotient polynomial <c>polynomial / divisor</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#baseRecursiveDivide.</remarks>
    public static GenPolynomial<GenPolynomial<C>> BaseRecursiveDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, C divisor)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(divisor);

        if (divisor.IsZero())
        {
            throw new ArithmeticException($"division by zero {polynomial}, {divisor}");
        }

        if (polynomial.IsZero() || divisor.IsOne())
        {
            return polynomial;
        }

        GenPolynomial<GenPolynomial<C>> result = new (polynomial.Ring);
        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> term in polynomial.Terms)
        {
            GenPolynomial<C> quotient = CoefficientBasePseudoDivide(term.Value, divisor);
            if (quotient.IsZero())
            {
                throw new InvalidOperationException("Coefficient pseudo division produced zero coefficient unexpectedly.");
            }

            resultTerms[term.Key] = quotient;
        }

        return result;
    }
}
