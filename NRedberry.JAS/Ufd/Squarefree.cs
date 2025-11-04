using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Squarefree decomposition interface.
/// </summary>
/// <typeparam name="C">Coefficient type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.Squarefree
/// </remarks>
public interface Squarefree<C>
    where C : GcdRingElem<C>
{
    /// <summary>
    /// Computes the greatest squarefree divisor of the polynomial.
    /// </summary>
    /// <param name="P">Input polynomial.</param>
    /// <returns>Squarefree normalization of <paramref name="P"/>.</returns>
    GenPolynomial<C> SquarefreePart(GenPolynomial<C> P);

    /// <summary>
    /// Checks whether the polynomial is squarefree.
    /// </summary>
    /// <param name="P">Input polynomial.</param>
    /// <returns><see langword="true"/> if <paramref name="P"/> is squarefree; otherwise <see langword="false"/>.</returns>
    bool IsSquarefree(GenPolynomial<C> P);

    /// <summary>
    /// Computes the squarefree factorization.
    /// </summary>
    /// <param name="P">Input polynomial.</param>
    /// <returns>Mapping of squarefree factors to their multiplicities.</returns>
    SortedDictionary<GenPolynomial<C>, long> SquarefreeFactors(GenPolynomial<C> P);
}
