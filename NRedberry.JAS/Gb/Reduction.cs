using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial reduction interface that mirrors the Java counterpart (which also implements Serializable).
/// Defines the S-Polynomial-based normalform, criterion 4, module criterion, and irreducible-set helpers.
/// </summary>
/// <typeparam name="C">Coefficient type that implements <see cref="RingElem{T}"/>.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.Reduction
/// </remarks>
public interface Reduction<C> where C : RingElem<C>
{
    /// <summary>
    /// Computes the normalform of <paramref name="A"/> with respect to the list <paramref name="P"/>.
    /// </summary>
    /// <param name="P">Polynomial list.</param>
    /// <param name="A">Polynomial to reduce.</param>
    /// <returns>nf(A) with respect to <paramref name="P"/>.</returns>
    GenPolynomial<C> Normalform(List<GenPolynomial<C>> P, GenPolynomial<C> A);

    /// <summary>
    /// Produces an irreducible, monic basis P such that <paramref name="Pp"/> and P span the same ideal while every element is in normal form.
    /// </summary>
    /// <param name="Pp">Polynomial list.</param>
    /// <returns>A list <c>P</c> of polynomials that are in normal form with respect to each other and satisfy ideal(Pp) = ideal(P).</returns>
    List<GenPolynomial<C>> IrreducibleSet(List<GenPolynomial<C>> Pp);
}
