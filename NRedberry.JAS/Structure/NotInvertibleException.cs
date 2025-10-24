namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// NotInvertibleException class. Runtime Exception to be thrown for not invertible monoid elements.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.NotInvertibleException
/// </remarks>
public class NotInvertibleException : Exception
{
    public NotInvertibleException() 
        : base("NotInvertibleException")
    {
    }

    public NotInvertibleException(string message) 
        : base(message)
    {
    }

    public NotInvertibleException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
