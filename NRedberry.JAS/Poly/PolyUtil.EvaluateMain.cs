using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Evaluates a univariate polynomial at the supplied coefficient value (its main variable).
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="coefficientFactory">Coefficient ring factory.</param>
    /// <param name="polynomial">Univariate polynomial to evaluate.</param>
    /// <param name="value">Value substituted for the main variable.</param>
    /// <returns>The scalar result <c>polynomial(value)</c>.</returns>
    /// <remarks>Original Java method: PolyUtil#evaluateMain (RingFactory overload).</remarks>
    public static C EvaluateMain<C>(RingFactory<C> coefficientFactory, GenPolynomial<C> polynomial, C value)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(coefficientFactory);
        ArgumentNullException.ThrowIfNull(polynomial);

        if (polynomial.IsZero())
        {
            return coefficientFactory.FromInteger(0);
        }

        if (polynomial.Ring.Nvar != 1)
        {
            throw new ArgumentException("evaluateMain requires a univariate polynomial.", nameof(polynomial));
        }

        if (value is null || value.IsZero())
        {
            return GetTrailingCoefficient(polynomial);
        }

        C? result = default;
        long previousExponent = -1;

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector exponent = term.Key;
            long currentExponent = exponent.GetVal(0);
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
        }

        if (result is null)
        {
            return coefficientFactory.FromInteger(0);
        }

        for (long i = 0; i < previousExponent; i++)
        {
            result = result.Multiply(value);
        }

        return result;
    }
}
