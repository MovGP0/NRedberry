using NRedberry.Core.Parsers;

namespace NRedberry.Contexts;

/// <summary>
/// This class represents Redberry context. It stores all Redberry session data (in some sense it stores static data).
/// <p>Management of current Redberry context is made through {@link ContextManager} class.
/// Context of Redberry is attached to the current thread, so that any thread created from the outside of Redebrry
/// will hold a unique instance of {@link Context} object. In such a way tensors created in one thread can not
/// be used in the other thread because they are in some sense "attached" to the initial thread. However, if thread is
/// created through the executor service which is obtained from {@link ContextManager#getExecutorService()}, it will
/// share the same context as initial thread (such threads could hold concurrent computations regarding single context,
/// the appropriate synchronization is assumed). In order to create a new session of Redberry with a particular context,
/// an instance of this class should be set as a current context via {@link ContextManager#setCurrentContext(Context)}.</p>
/// </summary>
public sealed class Context
{
    ///<summary>
    /// NameManager has a sense of namespace
    /// </summary>
    public NameManager nameManager;

    /// <summary>
    ///  Defaults output format can be changed during the session
    /// </summary>
    private OutputFormat defaultOutputFormat;

    private IndexConverterManager converterManager;
    private ParseManager parseManager;

    /// <summary>
    /// Holds information about metric types.
    /// This is a "map" from (byte) type to (bit) isMetric
    /// </summary>
    public LongBackedBitArray metricTypes = new(128);

    /**
     * Creates context from the settings
     *
     * @param contextSettings settings
     * @see ContextSettings
     */
    public Context(ContextSettings contextSettings)
    {
        parseManager = new ParseManager(contextSettings.Parser());
        converterManager = contextSettings.ConverterManager;
        nameManager = new NameManager(contextSettings.NameManagerSeed, contextSettings.Kronecker, contextSettings.MetricName);

        defaultOutputFormat = contextSettings.DefaultOutputFormat;

        foreach (IndexType type in contextSettings.MetricTypes)
        {
            metricTypes.Set(type.GetType().GetHashCode());
        }
    }

    private readonly object _resetTensorNamesLock = new();
    /**
     * This method resets all tensor names in the namespace.
     *
     * <p>Any tensor created before this method call becomes invalid, and
     * must not be used! This method is mainly used in unit tests, so
     * avoid invocations of this method in general computations.</p>
     */
    public  void ResetTensorNames()
    {
        lock (_resetTensorNamesLock)
        {
            nameManager.Reset();
        }
    }

    private readonly object _resetTensorNames = new();
    /**
     * This method resets all tensor names in the namespace and sets a
     * specified seed to the {@link NameManager}. If this method is invoked
     * with constant seed before any interactions with Redberry, further
     * behaviour of Redberry will be fully deterministic from run to run
     * (order of summands and multipliers will be fixed, computation time
     * will be pretty constant, hash codes will be the same).
     *
     * <p>Any tensor created before this method call becomes invalid, and
     * must not be used! This method is mainly used in unit tests, so
     * avoid invocations of this method in general computations.</p>
     */
    public void ResetTensorNames(int seed)
    {
        lock (_resetTensorNames)
        {
            nameManager.Reset(seed);
        }
    }

    /**
     * Sets the default output format. After this step, all expressions
     * will be printed according to the specified output format.
     *
     * @param defaultOutputFormat output format
     */
    public void SetDefaultOutputFormat(OutputFormat defaultOutputFormat) {
        this.defaultOutputFormat = defaultOutputFormat;
    }

    /**
     * Returns index converter manager of current session.
     *
     * @return index converter manager of current session
     */
    public IndexConverterManager GetIndexConverterManager() {
        return converterManager;
    }

    /**
     * Returns the name manager (namespace) of current session.
     *
     * @return the name manager (namespace) of current session.
     */
    public NameManager GetNameManager() {
        return nameManager;
    }

    /**
     * Returns {@code NameDescriptor} corresponding to the specified {@code int} nameId.
     *
     * @param nameId integer name of tensor
     * @return corresponding  {@code NameDescriptor}
     */
    public NameDescriptor GetNameDescriptor(int nameId) {
        return nameManager.GetNameDescriptor(nameId);
    }

    /**
     * Returns string representation of Kronecker delta name
     *
     * @return string representation of Kronecker delta name
     */
    public string GetKroneckerName() {
        return nameManager.GetKroneckerName();
    }

    /**
     * Returns string representation of metric tensor name
     *
     * @return string representation of metric tensor name
     */
    public string GetMetricName() {
        return nameManager.GetMetricName();
    }

    /**
     * Sets the default metric tensor name. After this step, metric tensor
     * will be printed with the specified string name.
     *
     * @param name string representation of metric tensor name
     */
    public void SetMetricName(string name) {
        nameManager.SetMetricName(name);
    }

    /**
     * Sets the default Kronecker tensor name. After this step, Kronecker tensor
     * will be printed with the specified string name.
     *
     * @param name string representation of Kronecker tensor name
     */
    public void SetKroneckerName(string name) {
        nameManager.SetKroneckerName(name);
    }

    /**
     * Returns parse manager of current session
     *
     * @return parse manager of current session
     */
    public ParseManager GetParseManager() {
        return parseManager;
    }

    /**
     * Returns true if metric is defined for the specified index type.
     *
     * @param type index type
     * @return true if metric is defined for the specified index type
     */
    public bool IsMetric(byte type) {
        return metricTypes[type];
    }

    /**
     * Returns the current context of Redberry session.
     *
     * @return the current context of Redberry session.
     */
    public static Context Get()
    {
        return ContextManager.GetCurrentContext();
    }

    public OutputFormat GetDefaultOutputFormat() => defaultOutputFormat;
    public OutputFormat DefaultOutputFormat => defaultOutputFormat;
}