using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;

/// <summary>
/// Polynomial Reduction abstract class. Implements common S-Polynomial, normalform, criterion 4 module criterion and irreducible set.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.gb.ReductionAbstract
/// </remarks>
public abstract class ReductionAbstract<C> : Reduction<C> where C : RingElem<C>
{
    public ReductionAbstract()
    {
    }

    /// <summary>
    /// Irreducible set.
    /// </summary>
    /// <param name="Pp">polynomial list</param>
    /// <returns>a list P of monic polynomials which are in normalform wrt. P and with ideal(Pp) = ideal(P).</returns>
    public virtual List<GenPolynomial<C>> IrreducibleSet(List<GenPolynomial<C>> Pp)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Normalform.
    /// </summary>
    /// <param name="P">polynomial list</param>
    /// <param name="A">polynomial</param>
    /// <returns>nf(A) with respect to P.</returns>
    public abstract GenPolynomial<C> Normalform(List<GenPolynomial<C>> P, GenPolynomial<C> A);
}
