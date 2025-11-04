using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Container for the factors of absolute factorization.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.Factors
/// </remarks>
public class Factors<C> where C : GcdRingElem<C>
{
    /// <summary>
    /// Original (irreducible) polynomial to be factored with coefficients from C.
    /// </summary>
    public readonly GenPolynomial<C> Poly;

    /// <summary>
    /// Algebraic field extension over C. Should be null, if p is absolutely irreducible.
    /// </summary>
    public readonly AlgebraicNumberRing<C>? Afac;

    /// <summary>
    /// Original polynomial to be factored with coefficients from AlgebraicNumberRing&lt;C&gt;.
    /// Should be null, if p is absolutely irreducible.
    /// </summary>
    public readonly GenPolynomial<AlgebraicNumber<C>>? Apoly;

    /// <summary>
    /// List of factors with coefficients from AlgebraicNumberRing&lt;C&gt;.
    /// Should be null, if p is absolutely irreducible.
    /// </summary>
    public readonly List<GenPolynomial<AlgebraicNumber<C>>>? Afactors;

    /// <summary>
    /// List of factors with coefficients from AlgebraicNumberRing&lt;AlgebraicNumber&lt;C&gt;&gt;.
    /// Should be null, if p is absolutely irreducible.
    /// </summary>
    public readonly List<Factors<AlgebraicNumber<C>>>? Arfactors;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="p">irreducible GenPolynomial over C</param>
    /// <param name="af">algebraic extension field of C where p has factors from afact</param>
    /// <param name="ap">GenPolynomial p represented with coefficients from af</param>
    /// <param name="afact">absolute irreducible factors of p with coefficients from af</param>
    /// <param name="arfact">further absolute irreducible factors of p with coefficients from extensions of af</param>
    public Factors(
        GenPolynomial<C> p,
        AlgebraicNumberRing<C>? af,
        GenPolynomial<AlgebraicNumber<C>>? ap,
        List<GenPolynomial<AlgebraicNumber<C>>>? afact,
        List<Factors<AlgebraicNumber<C>>>? arfact)
    {
        Poly = p;
        Afac = af;
        Apoly = ap;
        Afactors = afact;
        Arfactors = arfact;
    }

    public override int GetHashCode()
    {
        int h = Poly.GetHashCode();
        if (Afac == null)
        {
            return h;
        }

        h <<= 27;
        h += Afac.GetHashCode();
        if (Afactors != null)
        {
            h <<= 27;
            h += Afactors.GetHashCode();
        }

        if (Arfactors != null)
        {
            h <<= 27;
            h += Arfactors.GetHashCode();
        }

        return h;
    }
}
