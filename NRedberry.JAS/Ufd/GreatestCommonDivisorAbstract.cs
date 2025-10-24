using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms abstract base class.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorAbstract
/// </remarks>
public abstract class GreatestCommonDivisorAbstract<C> : GreatestCommonDivisor<C> where C : GcdRingElem<C>
{
    public override string ToString()
    {
        return GetType().Name;
    }

    public abstract GenPolynomial<C> BaseGcd(GenPolynomial<C> P, GenPolynomial<C> S);

    public abstract GenPolynomial<C> RecursiveUnivariateGcd(GenPolynomial<C> P, GenPolynomial<C> S);

    public C BaseContent(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> BasePrimitivePart(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Content(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> PrimitivePart(GenPolynomial<C> P)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Gcd(GenPolynomial<C> P, GenPolynomial<C> S)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Lcm(GenPolynomial<C> P, GenPolynomial<C> S)
    {
        throw new NotImplementedException();
    }

    public GenPolynomial<C> Resultant(GenPolynomial<C> P, GenPolynomial<C> S)
    {
        throw new NotImplementedException();
    }

    public List<GenPolynomial<C>> CoPrime(List<GenPolynomial<C>> A)
    {
        throw new NotImplementedException();
    }
}
