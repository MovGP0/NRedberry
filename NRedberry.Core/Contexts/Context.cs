using System;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts;

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
    private NameManager nameManager;

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
    private LongBackedBitArray metricTypes = new(128);

    /**
     * Creates context from the settings
     *
     * @param contextSettings settings
     * @see ContextSettings
     */
    public Context(ContextSettings contextSettings) {
        parseManager = new ParseManager(contextSettings.Parser);
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
    public void SetMetricName(String name) {
        nameManager.SetMetricName(name);
    }

    /**
     * Sets the default Kronecker tensor name. After this step, Kronecker tensor
     * will be printed with the specified string name.
     *
     * @param name string representation of Kronecker tensor name
     */
    public void SetKroneckerName(String name) {
        nameManager.SetKroneckerName(name);
    }

    /**
     * Returns {@code true} if specified tensor is a Kronecker tensor
     *
     * @param t tensor
     * @return {@code true} if specified tensor is a Kronecker tensor
     */
    public bool IsKronecker(SimpleTensor t) {
        return nameManager.IsKroneckerOrMetric(t.Name)
               && !IndicesUtils.HaveEqualStates(t.Indices[0], t.Indices[1]);
    }

    /**
     * Returns {@code true} if specified tensor is a metric tensor
     *
     * @param t tensor
     * @return {@code true} if specified tensor is a metric tensor
     */
    public bool IsMetric(SimpleTensor t) {
        return nameManager.IsKroneckerOrMetric(t.Name)
               && IndicesUtils.HaveEqualStates(t.Indices[0], t.Indices[1]);
    }

    /**
     * Returns {@code true} if specified tensor is a metric or a Kronecker tensor
     *
     * @param t tensor
     * @return {@code true} if specified tensor is a metric or a Kronecker tensor
     */
    public bool IsKroneckerOrMetric(SimpleTensor t) {
        return nameManager.IsKroneckerOrMetric(t.Name);
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
     * Returns Kronecker tensor with specified upper and lower indices.
     *
     * @param index1 first index
     * @param index2 second index
     * @return Kronecker tensor with specified upper and lower indices
     * @throws IllegalArgumentException if indices have same states
     * @throws IllegalArgumentException if indices have different types
     */
    public SimpleTensor CreateKronecker(uint index1, uint index2) {
        byte type;
        if ((type = IndicesUtils.GetType_(index1)) != IndicesUtils.GetType_(index2) || IndicesUtils.GetRawStateInt((uint)index1) == IndicesUtils.GetRawStateInt((uint)index2))
            throw new ArgumentException("This is not kronecker indices!");

        if (!IsMetric(type) && IndicesUtils.GetState(index2))
        {
            var t = index1;
            index1 = index2;
            index2 = t;
        }

        ISimpleIndices indices = IndicesFactory.CreateSimple(null, index1, index2);
        var nd = nameManager.mapNameDescriptor(nameManager.getKroneckerName(), new StructureOfIndices(indices));
        var name = nd.Id;
        return Tensor.SimpleTensor(name, indices);
    }

    /**
     * Returns metric tensor with specified indices.
     *
     * @param index1 first index
     * @param index2 second index
     * @return metric tensor with specified indices
     * @throws IllegalArgumentException if indices have different states
     * @throws IllegalArgumentException if indices have different types
     * @throws IllegalArgumentException if indices have non metric types
     */
    public SimpleTensor CreateMetric(uint index1, uint index2) {
        byte type;
        if ((type = IndicesUtils.GetType_(index1)) != IndicesUtils.GetType_(index2)
            || !IndicesUtils.HaveEqualStates(index1, index2)
            || !metricTypes.Get(type))
            throw new ArgumentException("Not metric indices.");
        var indices = IndicesFactory.CreateSimple(null, index1, index2);
        var nd = nameManager.mapNameDescriptor(nameManager.GetMetricName(), new StructureOfIndices(indices));
        var name = nd.Id;
        return Tensor.SimpleTensor(name, indices);
    }

    /**
     * Returns metric tensor if specified indices have same states and
     * Kronecker tensor if specified indices have different states.
     *
     * @param index1 first index
     * @param index2 second index
     * @return metric tensor if specified indices have same states and
     *         Kronecker tensor if specified indices have different states
     * @throws IllegalArgumentException if indices have different types
     * @throws IllegalArgumentException if indices have same states and non metric types
     */
    public SimpleTensor CreateMetricOrKronecker(uint index1, uint index2) {
        if (IndicesUtils.GetRawStateInt(index1) == IndicesUtils.GetRawStateInt(index2))
            return createMetric(index1, index2);
        return createKronecker(index1, index2);
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

    public SimpleTensor createMetric(uint index1, uint index2)
    {
        throw new NotImplementedException();
    }

    public SimpleTensor createKronecker(uint index1, uint index2)
    {
        throw new NotImplementedException();
    }

    public OutputFormat GetDefaultOutputFormat() => defaultOutputFormat;
}