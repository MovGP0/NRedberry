namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Monoid factory interface. Defines get one and tests for associativity and commutativity.
/// </summary>
/// <typeparam name="C">monoid element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.MonoidFactory
/// </remarks>
public interface MonoidFactory<C> : ElemFactory<C> where C : MonoidElem<C>
{
    /// <summary>
    /// Get the constant one for the MonoidElem.
    /// </summary>
    /// <returns>1.</returns>
    static C One { get; }

    /// <summary>
    /// Query if this monoid is commutative.
    /// </summary>
    /// <returns>true if this monoid is commutative, else false.</returns>
    bool IsCommutative();

    /// <summary>
    /// Query if this ring is associative.
    /// </summary>
    /// <returns>true if this monoid is associative, else false.</returns>
    bool IsAssociative();
}
