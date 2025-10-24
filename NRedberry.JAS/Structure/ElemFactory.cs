using System.Numerics;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Element factory interface. Defines embedding of integers, parsing and random element construction.
/// </summary>
/// <typeparam name="C">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.ElemFactory
/// </remarks>
public interface ElemFactory<C> where C : Element<C>
{
    /// <summary>
    /// Get a list of the generating elements.
    /// </summary>
    /// <returns>list of generators for the algebraic structure.</returns>
    List<C> Generators();

    /// <summary>
    /// Is this structure finite or infinite.
    /// </summary>
    /// <returns>true if this structure is finite, else false.</returns>
    bool IsFinite();

    /// <summary>
    /// Get the Element for a.
    /// </summary>
    /// <param name="a">long value</param>
    /// <returns>element corresponding to a.</returns>
    C FromInteger(long a);

    /// <summary>
    /// Get the Element for a.
    /// </summary>
    /// <param name="a">BigInteger value</param>
    /// <returns>element corresponding to a.</returns>
    C FromInteger(BigInteger a);

    /// <summary>
    /// Generate a random Element with size less equal to n.
    /// </summary>
    /// <param name="n">size parameter</param>
    /// <returns>a random element.</returns>
    C Random(int n);

    /// <summary>
    /// Generate a random Element with size less equal to n.
    /// </summary>
    /// <param name="n">size parameter</param>
    /// <param name="random">source for random bits</param>
    /// <returns>a random element.</returns>
    C Random(int n, Random random);

    /// <summary>
    /// Create a copy of Element c.
    /// </summary>
    /// <param name="c">element to copy</param>
    /// <returns>a copy of c.</returns>
    C Copy(C c);
}
