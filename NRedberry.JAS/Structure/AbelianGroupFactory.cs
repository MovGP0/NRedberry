namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Abelian group factory interface. Defines get zero.
/// </summary>
/// <typeparam name="C">abelian group element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.AbelianGroupFactory
/// </remarks>
public interface AbelianGroupFactory<C> : ElemFactory<C> where C : AbelianGroupElem<C>
{
    /// <summary>
    /// Get the constant zero for the AbelianGroupElem.
    /// </summary>
    /// <returns>0.</returns>
    C GetZERO();
}
