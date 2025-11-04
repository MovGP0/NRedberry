namespace NRedberry.Contexts.Defaults;

/// <summary>
/// Context factory, which creates context with default settings <see cref="DefaultContextSettings"/>.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/defaults/DefaultContextFactory.java</remarks>
public sealed class DefaultContextFactory : IContextFactory
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static DefaultContextFactory Instance { get; } = new();

    private DefaultContextFactory()
    {
    }

    /// <summary>
    /// Creating context defaults
    /// </summary>
    /// <returns></returns>
    public Context CreateContext() => new(DefaultContextSettings.Create());
}
