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
        if (Pp != null)
        {
            foreach (GenPolynomial<C>? poly in Pp)
            {
                if (poly.Length() == 0)
                {
                    continue;
                }

                GenPolynomial<C> monic = poly.Monic();
                if (monic.IsOne())
                {
                    result.Clear();
                    result.Add(monic);
                    return result;
                }

                result.Add(monic);
            }
        }

        int count = result.Count;
        if (count <= 1)
        {
            return result;
        }

        int irreducibleCount = 0;
        while (irreducibleCount != count)
        {
            GenPolynomial<C> polynomial = result[0];
            result.RemoveAt(0);
            ExpVector? leadingBefore = polynomial.LeadingExpVector();

            polynomial = Normalform(result, polynomial);
            if (polynomial.Length() == 0)
            {
                count--;
                if (count <= 1)
                {
                    return result;
                }

                continue;
            }

            ExpVector? leadingAfter = polynomial.LeadingExpVector();
            if (leadingAfter == null || leadingAfter.Signum() == 0)
            {
                result.Clear();
                result.Add(polynomial.Monic());
                return result;
            }

            if (leadingBefore?.Equals(leadingAfter) == true)
            {
                irreducibleCount++;
            }
            else
            {
                irreducibleCount = 0;
                polynomial = polynomial.Monic();
            }

            result.Add(polynomial);
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
