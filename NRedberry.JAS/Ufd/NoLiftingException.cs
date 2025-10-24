namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Non existing Hensel lifting. Exception to be thrown when a valid Hensel lifting cannot be constructed.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.NoLiftingException
/// </remarks>
public class NoLiftingException : Exception
{
    public NoLiftingException(string message)
        : base(message)
    {
    }
}
