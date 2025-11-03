namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorPair implements pairs of exponent vectors for S-polynomials. Objects of this class are immutable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorPair
/// </remarks>
public class ExpVectorPair
{
    private readonly ExpVector _first;
    private readonly ExpVector _second;

    /// <summary>
    /// Constructors for ExpVectorPair.
    /// </summary>
    /// <param name="e">first part</param>
    /// <param name="f">second part</param>
    public ExpVectorPair(ExpVector e, ExpVector f)
    {
        _first = e;
        _second = f;
    }

    /// <summary>
    /// Get first part.
    /// </summary>
    /// <returns>first part.</returns>
    public ExpVector GetFirst() => _first;

    /// <summary>
    /// Get second part.
    /// </summary>
    /// <returns>second part.</returns>
    public ExpVector GetSecond() => _second;

    /// <summary>
    /// String representation.
    /// </summary>
    public override string ToString()
    {
        return $"ExpVectorPair[{_first},{_second}]";
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
        bool equalFirst = _first.Equals(b.GetFirst());
        if (!equalFirst)
        {
            return false;
        }

        return _second.Equals(b.GetSecond());
    }

    /// <summary>
    /// Hash code.
    /// </summary>
    public override int GetHashCode()
    {
        return (_first.GetHashCode() << 16) + _second.GetHashCode();
    }

    /// <summary>
    /// IsMultiple.
    /// </summary>
    /// <param name="p">other</param>
    /// <returns>true, if this is a multiple of p, else false.</returns>
    public bool IsMultiple(ExpVectorPair p)
    {
        if (!_first.MultipleOf(p.GetFirst()))
        {
            return false;
        }

        if (!_second.MultipleOf(p.GetSecond()))
        {
            return false;
        }

        return true;
    }
}
