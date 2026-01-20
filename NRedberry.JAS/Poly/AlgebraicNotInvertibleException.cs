using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Algebraic number NotInvertibleException class. Runtime Exception to be thrown for not invertible algebraic numbers.
/// Container for the non-trivial factors found by the inversion algorithm.
/// Note: cannot be generic because of Throwable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.AlgebraicNotInvertibleException
/// </remarks>
public class AlgebraicNotInvertibleException : NotInvertibleException
{
    private const string DefaultMessage = "AlgebraicNotInvertibleException";

    /// <summary>
    /// Polynomial factor satisfying <c>F = F1 * F2</c>.
    /// </summary>
    public object? F { get; }

    /// <summary>
    /// Left factor contributing to <c>F</c>.
    /// </summary>
    public object? F1 { get; }

    /// <summary>
    /// Right factor contributing to <c>F</c>.
    /// </summary>
    public object? F2 { get; }

    /// <summary>
    /// Initializes the exception with a message and optional factors found during inversion.
    /// </summary>
    /// <param name="message">Explanation for the failure to invert.</param>
    /// <param name="f">Polynomial satisfying <c>F = F1 * F2</c>.</param>
    /// <param name="f1">Left factor of the non-trivial decomposition.</param>
    /// <param name="f2">Right factor of the non-trivial decomposition.</param>
    public AlgebraicNotInvertibleException(string message, object? f, object? f1, object? f2)
        : base(message)
    {
        F = f;
        F1 = f1;
        F2 = f2;
    }

    public AlgebraicNotInvertibleException()
        : base(DefaultMessage)
    {
    }

    public AlgebraicNotInvertibleException(string message)
        : base(message)
    {
    }

    public AlgebraicNotInvertibleException(Exception innerException)
        : base(DefaultMessage, innerException)
    {
    }

    public AlgebraicNotInvertibleException(string message, Exception innerException)
        : this(message, innerException, null, null, null)
    {
    }

    /// <summary>
    /// Initializes the exception with a message, inner <see cref="Exception"/>, and optional factors.
    /// </summary>
    /// <param name="message">Explanation for the failure to invert.</param>
    /// <param name="innerException">Wrapped exception from the lower-level inversion attempt.</param>
    /// <param name="f">Polynomial satisfying <c>F = F1 * F2</c>.</param>
    /// <param name="f1">Left factor of the non-trivial decomposition.</param>
    /// <param name="f2">Right factor of the non-trivial decomposition.</param>
    public AlgebraicNotInvertibleException(string message, Exception innerException, object? f, object? f1, object? f2)
        : base(message, innerException)
    {
        F = f;
        F1 = f1;
        F2 = f2;
    }

    /// <summary>
    /// Returns the base string plus any non-null factors that were captured.
    /// </summary>
    public override string ToString()
    {
        string s = base.ToString();
        if (F != null || F1 != null || F2 != null)
        {
            s += $", f = {F}, f1 = {F1}, f2 = {F2}";
        }

        return s;
    }
}
