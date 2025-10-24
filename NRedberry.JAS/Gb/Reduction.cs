using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial Reduction interface. Defines S-Polynomial, normalform, criterion 4, module criterion and irreducible set.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.Reduction
/// </remarks>
public interface Reduction<C> where C : RingElem<C>
{
    /// <summary>
    /// Normalform.
    /// </summary>
    /// <param name="P">polynomial list</param>
    /// <param name="A">polynomial</param>
    /// <returns>nf(A) with respect to P.</returns>
    GenPolynomial<C> Normalform(List<GenPolynomial<C>> P, GenPolynomial<C> A);

    /// <summary>
    /// Irreducible set.
    /// </summary>
    /// <param name="Pp">polynomial list</param>
    /// <returns>a list P of polynomials which are in normalform wrt. P and with ideal(Pp) = ideal(P).</returns>
    List<GenPolynomial<C>> IrreducibleSet(List<GenPolynomial<C>> Pp);
}
