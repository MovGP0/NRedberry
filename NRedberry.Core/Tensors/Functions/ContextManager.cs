using NRedberry.Contexts;
using NRedberry.Contexts.Defaults;

namespace NRedberry.Tensors.Functions;

/// <summary>
/// Implements context management logic for the current Redberry session.
/// </summary>
/// <remarks>
/// This class holds the current context of the Redberry session.
/// </remarks>
public static class ContextManager
{
    private sealed class ContextContainer
    {
        public Context Context { get; set; } = DefaultContextFactory.Instance.CreateContext();
    }

    private static readonly ThreadLocal<ContextContainer> s_threadLocalContainer = new(() => new ContextContainer());

    /// <summary>
    /// Gets or sets the current context of the Redberry session.
    /// </summary>
    public static Context CurrentContext
    {
        get => GetCurrentContainer().Context;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            GetCurrentContainer().Context = value;
        }
    }

    /// <summary>
    /// Returns the current context of the Redberry session.
    /// </summary>
    public static Context GetCurrentContext()
    {
        return GetCurrentContainer().Context;
    }

    /// <summary>
    /// Initializes and sets the current session context using the default factory.
    /// After this step, all tensors created earlier become invalid.
    /// </summary>
    public static Context InitializeNew()
    {
        var context = DefaultContextFactory.Instance.CreateContext();
        GetCurrentContainer().Context = context;
        return context;
    }

    /// <summary>
    /// Initializes and sets the current session context using the specified settings.
    /// After this step, all tensors created earlier become invalid.
    /// </summary>
    /// <param name="contextSettings">Settings for the new context.</param>
    public static Context InitializeNew(ContextSettings contextSettings)
    {
        ArgumentNullException.ThrowIfNull(contextSettings);
        var context = new Context(contextSettings);
        GetCurrentContainer().Context = context;
        return context;
    }

    /// <summary>
    /// Sets the current context to the specified one. After this step, all tensors created earlier become invalid.
    /// </summary>
    /// <param name="context">The new context.</param>
    public static void SetCurrentContext(Context context)
    {
        CurrentContext = context;
    }

    private static ContextContainer GetCurrentContainer()
    {
        return s_threadLocalContainer.Value
            ?? throw new InvalidOperationException("Current thread context container is not initialized.");
    }
}
