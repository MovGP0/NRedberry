using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Abstract factorization algorithms class. This class contains implementations of all methods of the
/// Factorization interface, except the method for factorization of a squarefree polynomial.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.FactorAbstract
/// </remarks>
public abstract class FactorAbstract<C> : Factorization<C> where C : GcdRingElem<C>
{
    protected readonly GreatestCommonDivisorAbstract<C> engine;
    protected readonly SquarefreeAbstract<C> sengine;

    protected FactorAbstract()
    {
        throw new ArgumentException("don't use this constructor");
    }

    public FactorAbstract(RingFactory<C> cfac)
    {
        engine = GCDFactory.GetProxy(cfac);
        sengine = SquarefreeFactory.GetImplementation(cfac);
    }

    public override string ToString()
    {
        return GetType().Name;
    }

    public bool IsIrreducible(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public bool IsSquarefree(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public virtual List<GenPolynomial<C>> FactorsSquarefree(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public List<GenPolynomial<C>> BaseFactorsRadical(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public bool IsFactorization(GenPolynomial<C> P, SortedDictionary<GenPolynomial<C>, long> F)
    {
        throw new NotImplementedException();
    }

    public SortedDictionary<GenPolynomial<C>, long> BaseFactors(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public abstract List<GenPolynomial<C>> BaseFactorsSquarefree(GenPolynomial<C> P);
}
