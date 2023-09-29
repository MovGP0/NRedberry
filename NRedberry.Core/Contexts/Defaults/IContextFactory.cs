namespace NRedberry.Contexts.Defaults;

/// <summary>
/// A factory interface for <see cref="Context"/> creation.
/// </summary>
/// <remarks>
/// https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/ContextFactory.java
/// </remarks>
public interface IContextFactory
{
    /// <summary>
    /// Creates a context object.
    /// </summary>
    /// <returns>context</returns>
    Context CreateContext();
}