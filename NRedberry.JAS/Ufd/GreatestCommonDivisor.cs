using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithm interface.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisor
/// </remarks>
public interface GreatestCommonDivisor<C> where C : GcdRingElem<C>
{
    GenPolynomial<C> Content(GenPolynomial<C> P);

    GenPolynomial<C> Gcd(GenPolynomial<C> P, GenPolynomial<C> S);

    GenPolynomial<C> Lcm(GenPolynomial<C> P, GenPolynomial<C> S);

    GenPolynomial<C> Resultant(GenPolynomial<C> P, GenPolynomial<C> S);

    List<GenPolynomial<C>> CoPrime(List<GenPolynomial<C>> A);
}
