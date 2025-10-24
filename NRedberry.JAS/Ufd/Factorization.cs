using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Factorization algorithms interface.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.Factorization
/// </remarks>
public interface Factorization<C> where C : GcdRingElem<C>
{
    /// <summary>
    /// GenPolynomial test if is irreducible.
    /// </summary>
    /// <param name="P">GenPolynomial</param>
    /// <returns>true if P is irreducible, else false.</returns>
    bool IsIrreducible(GenPolynomial<C> P);
}
