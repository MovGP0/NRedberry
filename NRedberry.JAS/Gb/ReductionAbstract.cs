using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial reduction abstract base that implements the shared S-polynomial evaluation,
/// normalform, criterion 4 module test, and irreducible-set helpers.
/// </summary>
/// <typeparam name="C">Coefficient type that implements <see cref="RingElem{T}"/>.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.ReductionAbstract
/// </remarks>
public abstract class ReductionAbstract<C> : Reduction<C> where C : RingElem<C>
{
    /// <summary>
    /// Builds the irreducible, monic basis of <paramref name="Pp"/> so that every result is in normal form and the ideals agree.
    /// </summary>
    /// <param name="Pp">Polynomial list.</param>
    /// <returns>A list <c>P</c> of monic polynomials that are in normal form with respect to each other and satisfy ideal(Pp) = ideal(P).</returns>
    public virtual List<GenPolynomial<C>> IrreducibleSet(List<GenPolynomial<C>> Pp)
    {
        List<GenPolynomial<C>> result = [];
        if (Pp is not null)
        {
            foreach (GenPolynomial<C>? poly in Pp)
            {
                if (poly is null)
                {
                    continue;
                }

                if (PolynomialReflectionHelpers.Length(poly) != 0)
                {
                    GenPolynomial<C> monic = PolynomialReflectionHelpers.Monic(poly);
                    if (PolynomialReflectionHelpers.IsOne(monic))
                    {
                        result.Clear();
                        result.Add(monic);
                        return result;
                    }

                    result.Add(monic);
                }
            }
        }

        int l = result.Count;
        if (l <= 1)
        {
            return result;
        }

        int irreducibleCount = 0;
        while (irreducibleCount != l)
        {
            GenPolynomial<C> a = result[0];
            result.RemoveAt(0);
            object? leadingBefore = PolynomialReflectionHelpers.LeadingExpVector(a);

            a = Normalform(result, a);
            if (PolynomialReflectionHelpers.Length(a) == 0)
            {
                l--;
                if (l <= 1)
                {
                    return result;
                }

                continue;
            }

            object? leadingAfter = PolynomialReflectionHelpers.LeadingExpVector(a);
            if (leadingAfter is null || PolynomialReflectionHelpers.Signum(leadingAfter) == 0)
            {
                result.Clear();
                result.Add(PolynomialReflectionHelpers.Monic(a));
                return result;
            }

            if (Equals(leadingBefore, leadingAfter))
            {
                irreducibleCount++;
            }
            else
            {
                irreducibleCount = 0;
                a = PolynomialReflectionHelpers.Monic(a);
            }

            result.Add(a);
        }

        return result;
    }

    /// <summary>
    /// Computes the normalform of <paramref name="A"/> with respect to <paramref name="P"/>.
    /// </summary>
    /// <param name="P">Polynomial list.</param>
    /// <param name="A">Polynomial to reduce.</param>
    /// <returns>nf(A) with respect to <paramref name="P"/>.</returns>
    public abstract GenPolynomial<C> Normalform(List<GenPolynomial<C>> P, GenPolynomial<C> A);
}
