using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial reduction sequential-use algorithm that implements the normalform.
/// </summary>
/// <typeparam name="C">Coefficient type that should represent a field element.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.ReductionSeq
/// </remarks>
public class ReductionSeq<C> : ReductionAbstract<C> where C : RingElem<C>
{
    /// <summary>
    /// Computes the normalform of <paramref name="Ap"/> with respect to the sequential list <paramref name="Pp"/>.
    /// Coefficients must come from a field.
    /// </summary>
    /// <param name="Pp">Polynomial list used as divisors.</param>
    /// <param name="Ap">Polynomial to reduce.</param>
    /// <returns>nf(Ap) with respect to <paramref name="Pp"/>.</returns>
    public override GenPolynomial<C> Normalform(List<GenPolynomial<C>> Pp, GenPolynomial<C> Ap)
    {
        if (Pp == null || Pp.Count == 0)
        {
            return Ap;
        }

        if (Ap == null || PolynomialReflectionHelpers.IsZero(Ap))
        {
            return Ap;
        }

        if (!PolynomialReflectionHelpers.CoefficientFieldIsField(Ap))
        {
            throw new ArgumentException("coefficients not from a field");
        }

        List<GenPolynomial<C>> polynomials = new(Pp.Count);
        foreach (GenPolynomial<C>? p in Pp)
        {
            if (p != null)
            {
                polynomials.Add(p);
            }
        }

        List<object?> leadingTerms = [];
        List<object?> leadingCoefficients = [];
        List<GenPolynomial<C>> filtered = [];

        foreach (GenPolynomial<C> poly in polynomials)
        {
            object? monomial = PolynomialReflectionHelpers.LeadingMonomial(poly);
            if (monomial == null)
            {
                continue;
            }

            filtered.Add(poly);
            leadingTerms.Add(PolynomialReflectionHelpers.GetMapEntryKey(monomial));
            leadingCoefficients.Add(PolynomialReflectionHelpers.GetMapEntryValue(monomial));
        }

        int count = filtered.Count;
        if (count == 0)
        {
            return Ap;
        }

        GenPolynomial<C> remainder = PolynomialReflectionHelpers.GetZeroPolynomial(Ap);
        GenPolynomial<C> current = Ap;

        while (PolynomialReflectionHelpers.Length(current) > 0)
        {
            object? leadMonomial = PolynomialReflectionHelpers.LeadingMonomial(current);
            if (leadMonomial == null)
            {
                break;
            }

            object? expVector = PolynomialReflectionHelpers.GetMapEntryKey(leadMonomial);
            if (expVector == null)
            {
                break;
            }

            C coefficient = (C)PolynomialReflectionHelpers.GetMapEntryValue(leadMonomial)!;

            int reducerIndex = -1;
            for (int i = 0; i < count; i++)
            {
                object? divisorExp = leadingTerms[i];
                if (divisorExp != null && PolynomialReflectionHelpers.ExpVectorMultipleOf(expVector, divisorExp))
                {
                    reducerIndex = i;
                    break;
                }
            }

            if (reducerIndex < 0)
            {
                remainder = PolynomialReflectionHelpers.SumMonomial(remainder, coefficient, expVector);
                current = PolynomialReflectionHelpers.SubtractMonomial(current, coefficient, expVector);
                continue;
            }

            object? divisorVector = leadingTerms[reducerIndex]!;
            object? diffVector = PolynomialReflectionHelpers.ExpVectorSubtract(expVector, divisorVector);

            C divisorCoeff = (C)leadingCoefficients[reducerIndex]!;
            C quotientCoeff = PolynomialReflectionHelpers.DivideCoefficient(coefficient, divisorCoeff);
            GenPolynomial<C> product = PolynomialReflectionHelpers.MultiplyMonomial(filtered[reducerIndex], quotientCoeff, diffVector!);
            current = PolynomialReflectionHelpers.SubtractPolynomial(current, product);
        }

        return remainder;
    }
}
