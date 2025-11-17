using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// Modular integer NotInvertibleException class. Runtime Exception to be thrown for not invertible modular integers.
/// Container for the non-trivial factors found by the inversion algorithm.
/// Note: cannot be generic because of Throwable.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.ModularNotInvertibleException
/// </remarks>
public sealed class ModularNotInvertibleException : NotInvertibleException
{
    public object? F { get; }
    public object? F1 { get; }
    public object? F2 { get; }

    public ModularNotInvertibleException()
        : base("ModularNotInvertibleException")
    {
    }

    public ModularNotInvertibleException(string message)
        : base(message)
    {
    }

    public ModularNotInvertibleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates a new exception that remembers the non-trivial factors uncovered during inversion.
    /// </summary>
    /// <param name="message">Description of the failure.</param>
    /// <param name="f">GcdRingElem that equals <paramref name="f1"/> * <paramref name="f2"/>.</param>
    /// <param name="f1">First factor.</param>
    /// <param name="f2">Second factor.</param>
    public ModularNotInvertibleException(
        string message,
        object? f = null,
        object? f1 = null,
        object? f2 = null)
        : base(message)
    {
        F = f;
        F1 = f1;
        F2 = f2;
    }

    /// <summary>
    /// Creates a new exception that wraps another failure while keeping the factor candidates.
    /// </summary>
    /// <param name="innerException">The original failure that triggered this exception.</param>
    /// <param name="f">GcdRingElem that equals <paramref name="f1"/> * <paramref name="f2"/>.</param>
    /// <param name="f1">First factor.</param>
    /// <param name="f2">Second factor.</param>
    public ModularNotInvertibleException(
        Exception innerException,
        object? f = null,
        object? f1 = null,
        object? f2 = null)
        : base("ModularNotInvertibleException", innerException)
    {
        F = f;
        F1 = f1;
        F2 = f2;
    }

    public ModularNotInvertibleException(Exception innerException)
        : base(innerException)
    {
    }

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
