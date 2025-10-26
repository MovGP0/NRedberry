namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Monoid element interface. Defines the multiplicative methods.
/// </summary>
/// <typeparam name="C">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.MonoidElem
/// </remarks>
public interface MonoidElem<C> : Element<C> where C : MonoidElem<C>
{
    /// <summary>
    /// Test if this is one.
    /// </summary>
    /// <returns>true if this is 1, else false.</returns>
    bool IsOne();

    /// <summary>
    /// Test if this is a unit. I.e. there exists x with this.Multiply(x).IsONE() == true.
    /// </summary>
    /// <returns>true if this is a unit, else false.</returns>
    bool IsUnit();

    /// <summary>
    /// Multiply this with S.
    /// </summary>
    /// <param name="S">element to multiply with</param>
    /// <returns>this * S.</returns>
    C Multiply(C S);

    /// <summary>
    /// Divide this by S.
    /// </summary>
    /// <param name="S">divisor</param>
    /// <returns>this / S.</returns>
    C Divide(C S);

    /// <summary>
    /// Remainder after division of this by S.
    /// </summary>
    /// <param name="S">divisor</param>
    /// <returns>this - (this / S) * S.</returns>
    C Remainder(C S);

    /// <summary>
    /// Inverse of this. Some implementing classes will throw NotInvertibleException if the element is not invertible.
    /// </summary>
    /// <returns>x with this * x = 1, if it exists.</returns>
    C Inverse();
}
