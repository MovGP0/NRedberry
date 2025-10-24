using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Interface for functions capable for Taylor series expansion.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.TaylorFunction
/// </remarks>
public interface TaylorFunction<C> where C : RingElem<C>
{
    /// <summary>
    /// Test if this is zero.
    /// </summary>
    /// <returns>true if this is 0, else false.</returns>
    bool IsZERO();

    /// <summary>
    /// Derivative.
    /// </summary>
    /// <returns>derivative of this.</returns>
    TaylorFunction<C> Deriviative();

    /// <summary>
    /// Evaluate.
    /// </summary>
    /// <param name="a">element</param>
    /// <returns>this(a).</returns>
    C Evaluate(C a);
}
