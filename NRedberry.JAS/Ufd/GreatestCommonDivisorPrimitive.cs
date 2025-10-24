using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with primitive polynomial remainder sequence.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorPrimitive
/// </remarks>
public class GreatestCommonDivisorPrimitive<C> : GreatestCommonDivisorAbstract<C> 
    where C : GcdRingElem<C>
{
    public override GenPolynomial<C> BaseGcd(GenPolynomial<C> P, GenPolynomial<C> S)
    {
        throw new NotImplementedException();
    }

    public override GenPolynomial<C> RecursiveUnivariateGcd(GenPolynomial<C> P, GenPolynomial<C> S)
    {
        throw new NotImplementedException();
    }
}
