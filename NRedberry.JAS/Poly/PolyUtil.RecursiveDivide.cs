using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Divides a recursive polynomial by a polynomial from the coefficient ring.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="polynomial">Recursive polynomial to divide.</param>
    /// <param name="divisor">Non-zero polynomial divisor.</param>
    /// <returns>The quotient recursive polynomial.</returns>
    /// <remarks>Original Java method: PolyUtil#recursiveDivide.</remarks>
    public static GenPolynomial<GenPolynomial<C>> RecursiveDivide<C>(GenPolynomial<GenPolynomial<C>> polynomial, GenPolynomial<C> divisor)
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
            GenPolynomial<C> quotient = BasePseudoDivide(term.Value, divisor);
            if (quotient.IsZero())
            {
                throw new InvalidOperationException("Pseudo division produced zero coefficient unexpectedly.");
            }

            resultTerms[term.Key] = quotient;
        }

        return result;
    }
}
