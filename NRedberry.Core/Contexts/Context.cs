using NRedberry.Core.Parsers;

namespace NRedberry.Contexts;

/// <summary>
/// Represents a Redberry context that stores all session state.
/// </summary>
/// <remarks>
/// Management of the current context is performed via <see cref="ContextManager"/>, and a context is
/// bound to the current thread. Threads created externally obtain their own <see cref="Context"/>
/// instance, so tensors from one thread must not be reused in another. Threads created via the executor
/// service from <see cref="ContextManager.GetExecutorService"/> share the same context and must be
/// synchronized appropriately. To start a new session with a specific context, set it via
/// <see cref="ContextManager.SetCurrentContext(Context)"/>.
/// </remarks>
public sealed class Context
{
    private IndexConverterManager ConverterManager { get; }
    private ParseManager ParseManager { get; }

    /// <summary>
    /// Holds information about metric types.
    /// This is a "map" from (byte) type to (bit) isMetric
    /// </summary>
    public LongBackedBitArray metricTypes { get; set; } = new(128);

    /// <summary>
    /// Creates a context using supplied settings.
    /// </summary>
    /// <param name="contextSettings">Configuration for parsers, converters, names, and defaults.</param>
    public Context(ContextSettings contextSettings)
    {
        ParseManager = new ParseManager(contextSettings.Parser());
        ConverterManager = contextSettings.ConverterManager;
        NameManager = new NameManager(
            contextSettings.NameManagerSeed,
            contextSettings.Kronecker,
            contextSettings.MetricName);

        DefaultOutputFormat = contextSettings.DefaultOutputFormat;

        foreach (IndexType type in contextSettings.MetricTypes)
        {
            metricTypes.Set(type.GetType().GetHashCode());
        }
    }

    private readonly Lock _resetTensorNamesLock = new();

    /// <summary>
    /// Resets all tensor names in the namespace.
    /// </summary>
    /// <remarks>
    /// Any tensor created before this call becomes invalid. Primarily intended for unit tests; avoid
    /// using it during normal computations.
    /// </remarks>
    public void ResetTensorNames()
    {
        lock (_resetTensorNamesLock)
        {
            NameManager.Reset();
        }
    }

    /// <summary>
    /// Resets all tensor names and seeds the <see cref="NameManager"/> deterministically.
    /// </summary>
    /// <param name="seed">Seed applied to <see cref="NameManager"/> for deterministic runs.</param>
    /// <remarks>
    /// Any tensor created before this call becomes invalid. Primarily intended for unit tests; avoid
    /// using it during normal computations.
    /// </remarks>
    public void ResetTensorNames(int seed)
    {
        lock (_resetTensorNamesLock)
        {
            NameManager.Reset(seed);
        }
    }

    /// <summary>
    /// Sets the default output format for expression printing.
    /// </summary>
    /// <param name="defaultOutputFormat">Output format.</param>
    public void SetDefaultOutputFormat(OutputFormat defaultOutputFormat)
    {
        this.DefaultOutputFormat = defaultOutputFormat;
    }

    /// <summary>
    /// Gets the index converter manager of the current session.
    /// </summary>
    public IndexConverterManager GetIndexConverterManager()
    {
        return ConverterManager;
    }

    /// <summary>
    /// Gets the name manager (namespace) of the current session.
    /// </summary>
    public NameManager NameManager { get; set; }

    /// <summary>
    /// Gets the <see cref="NameDescriptor"/> for the specified tensor name id.
    /// </summary>
    /// <param name="nameId">Integer tensor name.</param>
    public NameDescriptor GetNameDescriptor(int nameId)
    {
        return NameManager.GetNameDescriptor(nameId);
    }

    /// <summary>
    /// Gets the string representation of the Kronecker delta name.
    /// </summary>
    public string GetKroneckerName()
    {
        return NameManager.GetKroneckerName();
    }

    /// <summary>
    /// Gets the string representation of the metric tensor name.
    /// </summary>
    public string GetMetricName()
    {
        return NameManager.GetMetricName();
    }

    /// <summary>
    /// Sets the default metric tensor name used for printing.
    /// </summary>
    /// <param name="name">String representation of the metric tensor name.</param>
    public void SetMetricName(string name)
    {
        NameManager.SetMetricName(name);
    }

    /// <summary>
    /// Sets the default Kronecker tensor name used for printing.
    /// </summary>
    /// <param name="name">String representation of the Kronecker tensor name.</param>
    public void SetKroneckerName(string name)
    {
        NameManager.SetKroneckerName(name);
    }

    /// <summary>
    /// Gets the parse manager for the current session.
    /// </summary>
    public ParseManager GetParseManager()
    {
        return ParseManager;
    }

    /// <summary>
    /// Returns <c>true</c> if a metric is defined for the specified index type.
    /// </summary>
    /// <param name="type">Index type.</param>
    public bool IsMetric(byte type)
    {
        return metricTypes[type];
    }

    /// <summary>
    /// Gets the current context of the Redberry session.
    /// </summary>
    public static Context Get()
    {
        return ContextManager.GetCurrentContext();
    }

    public OutputFormat GetDefaultOutputFormat() => DefaultOutputFormat;

    /// <summary>
    /// Default output format; can be updated during the session.
    /// </summary>
    public OutputFormat DefaultOutputFormat { get; private set; }
}
