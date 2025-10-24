namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Unary functor interface.
/// </summary>
/// <typeparam name="C">element type</typeparam>
/// <typeparam name="D">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.UnaryFunctor
/// </remarks>
public interface UnaryFunctor<C, D> 
    where C : Element<C> 
    where D : Element<D>
{
    /// <summary>
    /// Evaluate.
    /// </summary>
    /// <param name="c">input element</param>
    /// <returns>evaluated element.</returns>
    D Eval(C c);
}
