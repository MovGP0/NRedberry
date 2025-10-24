using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Monomial class. Represents pairs of exponent vectors and coefficients. Adaptor for Map.Entry.
/// </summary>
/// <typeparam name="TC">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.Monomial
/// </remarks>
public sealed class Monomial<TC> where TC : RingElem<TC>
{
    /// <summary>
    /// Exponent of monomial.
    /// </summary>
    public readonly ExpVector E;

    /// <summary>
    /// Coefficient of monomial.
    /// </summary>
    public readonly TC C;

    /// <summary>
    /// Constructor of monomial.
    /// </summary>
    /// <param name="me">a KeyValuePair</param>
    public Monomial(KeyValuePair<ExpVector, TC> me)
    {
        E = me.Key;
        C = me.Value;
    }

    /// <summary>
    /// Constructor of monomial.
    /// </summary>
    /// <param name="e">exponent</param>
    /// <param name="c">coefficient</param>
    public Monomial(ExpVector e, TC c)
    {
        E = e;
        C = c;
    }

    /// <summary>
    /// Getter for exponent.
    /// </summary>
    /// <returns>exponent.</returns>
    public ExpVector Exponent() => E;

    /// <summary>
    /// Getter for coefficient.
    /// </summary>
    /// <returns>coefficient.</returns>
    public TC Coefficient() => C;

    /// <summary>
    /// String representation of Monomial.
    /// </summary>
    public override string ToString()
    {
        return C.ToString() + " " + E.ToString();
    }
}
