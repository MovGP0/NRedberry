using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Greatest common divisor algorithm interface.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Usage: To create classes that implement this interface use the
/// GreatestCommonDivisorFactory. It will select an appropriate
/// implementation based on the types of polynomial coefficients CT.
///
/// Example:
/// <code>
/// // C#-style example (transcribed from Java docs)
/// var cofac = /* coefficient example, e.g. BigInteger instance type or factory */;
/// GreatestCommonDivisor&lt;BigInteger&gt; engine = GCDFactory.GetImplementation(cofac);
/// var c = engine.Gcd(a, b);
/// </code>
///
/// For details see GCDFactory.GetImplementation.
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.GreatestCommonDivisor
/// </remarks>
public interface GreatestCommonDivisor<C> where C : GcdRingElem<C>
{
    /// <summary>
    /// GenPolynomial content.
    /// </summary>
    /// <param name="polynomial">GenPolynomial.</param>
    /// <returns>cont(P).</returns>
    GenPolynomial<C> Content(GenPolynomial<C> polynomial);

    /// <summary>
    /// GenPolynomial greatest common divisor.
    /// </summary>
    /// <param name="first">GenPolynomial P.</param>
    /// <param name="second">GenPolynomial S.</param>
    /// <returns>gcd(P, S).</returns>
    GenPolynomial<C> Gcd(GenPolynomial<C> first, GenPolynomial<C> second);

    /// <summary>
    /// GenPolynomial least common multiple.
    /// </summary>
    /// <param name="first">GenPolynomial P.</param>
    /// <param name="second">GenPolynomial S.</param>
    /// <returns>lcm(P, S).</returns>
    GenPolynomial<C> Lcm(GenPolynomial<C> first, GenPolynomial<C> second);

    /// <summary>
    /// GenPolynomial resultant.
    /// The input polynomials are considered as univariate polynomials in the main variable.
    /// </summary>
    /// <param name="first">GenPolynomial P.</param>
    /// <param name="second">GenPolynomial S.</param>
    /// <returns>res(P, S).</returns>
    /// <remarks>
    /// Implementations may throw NotSupportedException if resultant is not implemented.
    /// </remarks>
    GenPolynomial<C> Resultant(GenPolynomial<C> first, GenPolynomial<C> second);

    /// <summary>
    /// GenPolynomial co-prime list.
    /// </summary>
    /// <param name="polynomials">List of GenPolynomials A.</param>
    /// <returns>
    /// B with gcd(b,c) = 1 for all b != c in B and for all non-constant
    /// a in A there exists b in B with b|a. B does not contain zero or
    /// constant polynomials.
    /// </returns>
    List<GenPolynomial<C>> CoPrime(List<GenPolynomial<C>> polynomials);
}
