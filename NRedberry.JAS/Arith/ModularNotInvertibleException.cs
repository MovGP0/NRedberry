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
public class ModularNotInvertibleException : NotInvertibleException
{
    public readonly object? F;
    public readonly object? F1;
    public readonly object? F2;

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
