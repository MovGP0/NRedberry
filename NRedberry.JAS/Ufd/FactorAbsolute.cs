using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Absolute factorization algorithms class. This class contains implementations
/// of methods for factorization over algebraically closed fields. The required
/// field extension is computed along with the factors. The methods have been
/// tested for prime fields of characteristic zero, that is for
/// <code>BigRational</code>. It might eventually also be used for prime
/// fields of non-zero characteristic, that is with <code>ModInteger</code>.
/// The field extension may yet not be minimal.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorAbsolute
/// </remarks>
public abstract class FactorAbsolute<C>(RingFactory<C> cfac)
    : FactorAbstract<C>(cfac)
    where C : GcdRingElem<C>
{
    public override string ToString() => GetType().FullName ?? GetType().Name;
}
