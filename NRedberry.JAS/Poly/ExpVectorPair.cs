namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorPair implements pairs of exponent vectors for S-polynomials. Objects of this class are immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorPair
/// </remarks>
public class ExpVectorPair
{
    private readonly ExpVector e1;
    private readonly ExpVector e2;

    /// <summary>
    /// Constructors for ExpVectorPair.
    /// </summary>
    /// <param name="e">first part</param>
    /// <param name="f">second part</param>
    public ExpVectorPair(ExpVector e, ExpVector f)
    {
        e1 = e;
        e2 = f;
    }

    /// <summary>
    /// Get first part.
    /// </summary>
    /// <returns>first part.</returns>
    public ExpVector GetFirst() => e1;

    /// <summary>
    /// Get second part.
    /// </summary>
    /// <returns>second part.</returns>
    public ExpVector GetSecond() => e2;

    /// <summary>
    /// String representation.
    /// </summary>
    public override string ToString()
    {
        return $"ExpVectorPair[{e1},{e2}]";
    }

    /// <summary>
    /// Equals.
    /// </summary>
    /// <param name="B">other</param>
    /// <returns>true, if this == B, else false.</returns>
    public override bool Equals(object? B)
    {
        if (B is not ExpVectorPair b) return false;
        return Equals(b);
    }

    /// <summary>
    /// Equals.
    /// </summary>
    /// <param name="b">other</param>
    /// <returns>true, if this == b, else false.</returns>
    public bool Equals(ExpVectorPair b)
    {
        bool t = e1.Equals(b.GetFirst());
        t = t && e2.Equals(b.GetSecond());
        return t;
    }

    /// <summary>
    /// Hash code.
    /// </summary>
    public override int GetHashCode()
    {
        return (e1.GetHashCode() << 16) + e2.GetHashCode();
    }

    /// <summary>
    /// IsMultiple.
    /// </summary>
    /// <param name="p">other</param>
    /// <returns>true, if this is a multiple of p, else false.</returns>
    public bool IsMultiple(ExpVectorPair p)
    {
        throw new NotImplementedException();
    }
}
