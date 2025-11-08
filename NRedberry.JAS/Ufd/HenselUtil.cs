using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Hensel utilities for unique factorization domains.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselUtil
/// </remarks>
public static partial class HenselUtil
{
    private const bool Debug = false;

    internal static GenPolynomialRing<BigInteger> CreateIntegerPolynomialRing<C>(GenPolynomialRing<C> template)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(template);
        return new GenPolynomialRing<BigInteger>(
            new BigInteger(),
            template.Nvar,
            template.Tord,
            template.GetVars());
    }

    internal static GenPolynomialRing<T> CreatePolynomialRingFromTemplate<T, C>(
        RingFactory<T> coefficientFactory,
        GenPolynomialRing<C> template)
        where T : RingElem<T>
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(coefficientFactory);
        ArgumentNullException.ThrowIfNull(template);
        return new GenPolynomialRing<T>(
            coefficientFactory,
            template.Nvar,
            template.Tord,
            template.GetVars());
    }
}
