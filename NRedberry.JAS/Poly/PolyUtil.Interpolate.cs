using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    /// <summary>
    /// Performs modular interpolation on the first variable of a recursive polynomial so that evaluation and residue constraints are met.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="ring">Result polynomial ring.</param>
    /// <param name="source">Polynomial providing residue information.</param>
    /// <param name="modulus">Interpolation modulus.</param>
    /// <param name="modulusInverse">Inverse of <c>modulus(evaluationPoint)</c> in the coefficient ring.</param>
    /// <param name="evaluation">Polynomial specifying target evaluations.</param>
    /// <param name="evaluationPoint">Point at which the evaluation holds.</param>
    /// <returns>Polynomial whose residue modulo <paramref name="modulus"/> matches <paramref name="source"/> and whose evaluation matches <paramref name="evaluation"/>.</returns>
    /// <remarks>Original Java method: PolyUtil#interpolate (recursive overload).</remarks>
    public static GenPolynomial<GenPolynomial<C>> Interpolate<C>(
        GenPolynomialRing<GenPolynomial<C>> ring,
        GenPolynomial<GenPolynomial<C>> source,
        GenPolynomial<C> modulus,
        C modulusInverse,
        GenPolynomial<C> evaluation,
        C evaluationPoint)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(modulus);
        ArgumentNullException.ThrowIfNull(modulusInverse);
        ArgumentNullException.ThrowIfNull(evaluation);
        ArgumentNullException.ThrowIfNull(evaluationPoint);

        GenPolynomial<GenPolynomial<C>> result = new (ring);
        GenPolynomial<GenPolynomial<C>> working = source.Clone();

        SortedDictionary<ExpVector, GenPolynomial<C>> sourceTerms = working.Terms;
        SortedDictionary<ExpVector, C> evaluationTerms = evaluation.Terms;
        SortedDictionary<ExpVector, GenPolynomial<C>> resultTerms = result.Terms;

        GenPolynomialRing<C> coefficientRing = (GenPolynomialRing<C>)ring.CoFac;
        RingFactory<C> coefficientFactory = coefficientRing.CoFac;

        foreach (KeyValuePair<ExpVector, C> entry in evaluationTerms)
        {
            ExpVector exponent = entry.Key;
            C entryValue = entry.Value;
            if (sourceTerms.Remove(exponent, out GenPolynomial<C>? existing))
            {
                GenPolynomial<C> interpolated = Interpolate(coefficientRing, existing, modulus, modulusInverse, entryValue, evaluationPoint);
                if (!interpolated.IsZero())
                {
                    resultTerms[exponent] = interpolated;
                }
            }
            else
            {
                GenPolynomial<C> interpolated = Interpolate(
                    coefficientRing,
                    new GenPolynomial<C>(coefficientRing),
                    modulus,
                    modulusInverse,
                    entryValue,
                    evaluationPoint);
                if (!interpolated.IsZero())
                {
                    resultTerms[exponent] = interpolated;
                }
            }
        }

        foreach (KeyValuePair<ExpVector, GenPolynomial<C>> entry in sourceTerms)
        {
            GenPolynomial<C> interpolated = Interpolate(
                coefficientRing,
                entry.Value,
                modulus,
                modulusInverse,
                coefficientFactory.FromInteger(0),
                evaluationPoint);

            if (!interpolated.IsZero())
            {
                resultTerms[entry.Key] = interpolated;
            }
        }

        return result;
    }

    /// <summary>
    /// Performs univariate interpolation ensuring a polynomial satisfies both a residue and an evaluation constraint.
    /// </summary>
    /// <typeparam name="C">Coefficient type.</typeparam>
    /// <param name="ring">Result polynomial ring.</param>
    /// <param name="polynomial">Polynomial providing the residue constraint.</param>
    /// <param name="modulus">Interpolation modulus.</param>
    /// <param name="modulusInverse">Inverse of <c>modulus(evaluationPoint)</c>.</param>
    /// <param name="targetValue">Desired evaluation value.</param>
    /// <param name="evaluationPoint">Point at which the evaluation matches.</param>
    /// <returns>Interpolated polynomial.</returns>
    /// <remarks>Original Java method: PolyUtil#interpolate (univariate overload).</remarks>
    public static GenPolynomial<C> Interpolate<C>(
        GenPolynomialRing<C> ring,
        GenPolynomial<C> polynomial,
        GenPolynomial<C> modulus,
        C modulusInverse,
        C targetValue,
        C evaluationPoint)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomial);
        ArgumentNullException.ThrowIfNull(modulus);
        ArgumentNullException.ThrowIfNull(modulusInverse);
        ArgumentNullException.ThrowIfNull(targetValue);
        ArgumentNullException.ThrowIfNull(evaluationPoint);

        C evaluated = EvaluateMain(ring.CoFac, polynomial, evaluationPoint);
        C difference = targetValue.Subtract(evaluated);
        if (difference.IsZero())
        {
            return polynomial;
        }

        C scaled = difference.Multiply(modulusInverse);
        return modulus.Multiply(scaled).Sum(polynomial);
    }
}
