using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// Modular ring factory interface. Defines chinese remainder method and get modul method.
/// </summary>
/// <typeparam name="C">ring element type that is both RingElem and Modular</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModularRingFactory
/// </remarks>
public interface ModularRingFactory<C> : RingFactory<C> where C : RingElem<C>, Modular
{
    /// <summary>
    /// Return the BigInteger modul for the factory.
    /// </summary>
    /// <returns>a BigInteger of this.modul.</returns>
    BigInteger GetIntegerModul();

    /// <summary>
    /// Chinese remainder algorithm. Assert c.modul >= a.modul and c.modul * a.modul = this.modul.
    /// </summary>
    /// <param name="c">modular</param>
    /// <param name="ci">inverse of c.modul in ring of a</param>
    /// <param name="a">other ModLong</param>
    /// <returns>S, with S mod c.modul == c and S mod a.modul == a.</returns>
    C ChineseRemainder(C c, C ci, C a);
}
