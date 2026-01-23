using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Container for optimization results that keeps the associated permutation vector.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.OptimizedPolynomialList
/// </remarks>
public class OptimizedPolynomialList<C> : PolynomialList<C> where C : RingElem<C>
{
    /// <summary>
    /// Permutation vector used to adjust the underlying polynomial ordering.
    /// </summary>
    public readonly List<int> Perm;

    /// <summary>
    /// Constructs an optimized polynomial list with the provided permutation and base list.
    /// </summary>
    /// <param name="P">Permutation vector.</param>
    /// <param name="R">Underlying polynomial ring.</param>
    /// <param name="L">Polynomial list to wrap.</param>
    public OptimizedPolynomialList(List<int> P, GenPolynomialRing<C> R, List<GenPolynomial<C>> L)
        : base(R, L)
    {
        Perm = P ?? throw new ArgumentNullException(nameof(P));
    }

    /// <summary>
    /// Includes the permutation data before delegating to the base representation.
    /// </summary>
    public override string ToString()
    {
        return "permutation = " + string.Join(", ", Perm) + Environment.NewLine + base.ToString();
    }
}
