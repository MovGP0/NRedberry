using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Absolute factorization algorithms abstract class.
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
