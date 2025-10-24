using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithms with monic polynomial remainder sequence.
/// If C is a field, then the monic PRS (on coefficients) is computed otherwise
/// no simplifications in the reduction are made.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisorSimple
/// </remarks>
public class GreatestCommonDivisorSimple<C> : GreatestCommonDivisorAbstract<C> 
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
