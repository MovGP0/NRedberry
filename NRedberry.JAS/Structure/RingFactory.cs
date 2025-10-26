using System.Numerics;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Ring factory interface. Defines test for field and access of characteristic.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.RingFactory
/// </remarks>
public interface RingFactory<C> : AbelianGroupFactory<C>, MonoidFactory<C> where C : RingElem<C>
{
    /// <summary>
    /// Query if this ring is a field. May return false if it is too hard to determine if this ring is a field.
    /// </summary>
    /// <returns>true if it is known that this ring is a field, else false.</returns>
    bool IsField();

    /// <summary>
    /// Characteristic of this ring.
    /// </summary>
    /// <returns>characteristic of this ring.</returns>
    BigInteger Characteristic();
}
