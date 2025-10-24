namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Gcd ring element interface. Empty interface since gcd and egcd is now in RingElem.
/// Adds greatest common divisor and extended greatest common divisor.
/// </summary>
/// <typeparam name="C">gcd element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.GcdRingElem
/// </remarks>
public interface GcdRingElem<C> : RingElem<C> where C : GcdRingElem<C>
{
}
