using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// List utilities. For example map functor on list elements.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.ListUtil
/// </remarks>
public class ListUtil
{
    /// <summary>
    /// Map a unary function to the list.
    /// </summary>
    /// <typeparam name="C">input element type</typeparam>
    /// <typeparam name="D">output element type</typeparam>
    /// <param name="list">input list</param>
    /// <param name="f">evaluation functor</param>
    /// <returns>new list elements f(list(i)).</returns>
    public static List<D> Map<C, D>(List<C> list, UnaryFunctor<C, D> f) 
        where C : Element<C> 
        where D : Element<D>
    {
        throw new NotImplementedException();
    }
}
