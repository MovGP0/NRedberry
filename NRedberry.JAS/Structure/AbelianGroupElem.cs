namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Abelian group element interface. Defines the additive methods.
/// </summary>
/// <typeparam name="C">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.AbelianGroupElem
/// </remarks>
public interface AbelianGroupElem<C> : Element<C> where C : AbelianGroupElem<C>
{
    /// <summary>
    /// Test if this is zero.
    /// </summary>
    /// <returns>true if this is 0, else false.</returns>
    bool IsZero();

    /// <summary>
    /// Signum.
    /// </summary>
    /// <returns>the sign of this.</returns>
    int Signum();

    /// <summary>
    /// Sum of this and S.
    /// </summary>
    /// <param name="S">element to add</param>
    /// <returns>this + S.</returns>
    C Sum(C S);

    /// <summary>
    /// Subtract S from this.
    /// </summary>
    /// <param name="S">element to subtract</param>
    /// <returns>this - S.</returns>
    C Subtract(C S);

    /// <summary>
    /// Negate this.
    /// </summary>
    /// <returns>- this.</returns>
    C Negate();

    /// <summary>
    /// Absolute value of this.
    /// </summary>
    /// <returns>|this|.</returns>
    C Abs();
}
