namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Ring element interface. Combines additive and multiplicative methods. Adds also gcd because of polynomials.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.RingElem
/// </remarks>
public interface RingElem<C> : AbelianGroupElem<C>, MonoidElem<C> where C : RingElem<C>
{
    /// <summary>
    /// Greatest common divisor.
    /// </summary>
    /// <param name="b">other element</param>
    /// <returns>gcd(this, b).</returns>
    C Gcd(C b);

    /// <summary>
    /// Extended greatest common divisor.
    /// </summary>
    /// <param name="b">other element</param>
    /// <returns>[ gcd(this,b), c1, c2 ] with c1*this + c2*b = gcd(this,b).</returns>
    C[] Egcd(C b);
}
