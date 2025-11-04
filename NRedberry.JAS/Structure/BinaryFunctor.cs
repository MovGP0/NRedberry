namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Binary functor interface.
/// </summary>
/// <typeparam name="C1">element type</typeparam>
/// <typeparam name="C2">element type</typeparam>
/// <typeparam name="D">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.BinaryFunctor
/// </remarks>
public interface BinaryFunctor<C1, C2, D>
    where C1 : Element<C1>
    where C2 : Element<C2>
    where D : Element<D>
{
    /// <summary>
    /// Evaluate.
    /// </summary>
    /// <param name="c1">first input element</param>
    /// <param name="c2">second input element</param>
    /// <returns>evaluated element.</returns>
    D Eval(C1 c1, C2 c2);
}
