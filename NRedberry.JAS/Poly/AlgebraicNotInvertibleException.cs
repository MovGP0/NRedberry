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
    public object? F { get; }
    public object? F1 { get; }
    public object? F2 { get; }

    public AlgebraicNotInvertibleException(
        string message,
        object? f,
        object? f1,
        object? f2)
        : base(message)
    {
        F = f;
        F1 = f1;
        F2 = f2;
    }

    public AlgebraicNotInvertibleException(
        string message,
        Exception? innerException,
        object? f = null,
        object? f1 = null,
        object? f2 = null)
        : base(message, innerException!)
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
