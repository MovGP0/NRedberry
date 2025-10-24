namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Element interface. Basic functionality of elements, e.g. compareTo, equals, copy.
/// Note: extension of Cloneable removed in 2012-08-18, clone() is renamed to copy().
/// </summary>
/// <typeparam name="C">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.Element
/// See the discussion in Bloch on Cloning: http://www.artima.com/intv/bloch13.html
/// </remarks>
public interface Element<C> : IComparable<C> where C : Element<C>
{
    /// <summary>
    /// Clone this Element.
    /// </summary>
    /// <returns>Creates and returns a copy of this Element.</returns>
    C Copy();

    /// <summary>
    /// Test if this is equal to b.
    /// </summary>
    /// <param name="b">object to compare</param>
    /// <returns>true if this is equal to b, else false.</returns>
    bool Equals(object? b);

    /// <summary>
    /// Hashcode of this Element.
    /// </summary>
    /// <returns>the hashCode.</returns>
    int GetHashCode();

    /// <summary>
    /// Get the corresponding element factory.
    /// </summary>
    /// <returns>factory for this Element.</returns>
    ElemFactory<C> Factory();
}
