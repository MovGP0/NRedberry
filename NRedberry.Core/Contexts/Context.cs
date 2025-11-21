using NRedberry.Parsers;
using NRedberry.Tensors.Functions;

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
    /// <summary>
    /// Gets the index converter manager of the current session.
    /// </summary>
    public IndexConverterManager ConverterManager { get; }

    public ParseManager ParseManager { get; }

    /// <summary>
    /// Holds information about metric types.
    /// This is a "map" from (byte) type to (bit) isMetric
    /// </summary>
    public LongBackedBitArray metricTypes { get; set; } = new(128);

    /// <summary>
    /// Gets the name manager (namespace) of the current session.
    /// </summary>
    public NameManager NameManager { get; }

    /// <summary>
    /// Default output format; can be updated during the session.
    /// </summary>
    public OutputFormat DefaultOutputFormat { get; set; }

    private readonly Lock _resetTensorNamesLock = new();

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
    /// Gets the <see cref="NameDescriptor"/> for the specified tensor name id.
    /// </summary>
    /// <param name="nameId">Integer tensor name.</param>
    public NameDescriptor GetNameDescriptor(int nameId) => NameManager.GetNameDescriptor(nameId);

    /// <summary>
    /// The string representation of the Kronecker delta name.
    /// </summary>
    public string KroneckerName
    {
        get => NameManager.KroneckerName;
        set => NameManager.KroneckerName = value;
    }

    /// <summary>
    /// The string representation of the metric tensor name.
    /// </summary>
    public string MetricName
    {
        get => NameManager.MetricName;
        set => NameManager.MetricName = value;
    }

    public string DiracDeltaName
    {
        get => NameManager.DiracDeltaName;
        set => NameManager.DiracDeltaName = value;
    }

    /// <summary>
    /// Returns <c>true</c> if a metric is defined for the specified index type.
    /// </summary>
    /// <param name="type">Index type.</param>
    public bool IsMetric(byte type) => metricTypes[type];

    /// <summary>
    /// Gets the current context of the Redberry session.
    /// </summary>
    public static Context Get() => ContextManager.GetCurrentContext();
}
