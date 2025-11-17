namespace NRedberry.Contexts;

/// <summary>
/// Redberry current context.
/// This class statically delegates common useful methods from Redberry <see cref="Context"/> of current session.
/// </summary>
/// <remarks>https://github.com/redberry-cas/core/blob/master/src/main/java/cc/redberry/core/context/CC.java</remarks>
// ReSharper disable once InconsistentNaming
public static class CC
{
    /// <summary>
    /// Returns the current context of the Redberry session.
    /// </summary>
    /// <returns>The current context of the Redberry session.</returns>
    public static Context Current => Context.Get();

    /// <summary>
    /// Returns true if the metric is defined for the specified index type.
    /// </summary>
    /// <param name="type">The index type.</param>
    /// <returns>True if the metric is defined for the specified index type.</returns>
    public static bool IsMetric(byte type) => Current.IsMetric(type);

    /// <summary>
    /// Returns the <see cref="NameDescriptor"/> corresponding to the specified <paramref name="nameId"/>.
    /// </summary>
    /// <param name="name">The integer name of the tensor.</param>
    /// <returns>The corresponding <see cref="NameDescriptor"/>.</returns>
    public static NameDescriptor GetNameDescriptor(int name) => Current.GetNameDescriptor(name);

    /// <summary>
    /// Returns the name manager (namespace) of the current session.
    /// </summary>
    /// <returns>The name manager (namespace) of the current session.</returns>
    public static NameManager NameManager => Current.NameManager;

    /// <summary>
    /// Returns the index converter manager of the current session.
    /// </summary>
    /// <returns>The index converter manager of the current session.</returns>
    public static IndexConverterManager GetIndexConverterManager() => Current.GetIndexConverterManager();

    /// <summary>
    /// Returns the current default output format.
    /// </summary>
    /// <returns>The current default output format.</returns>
    public static OutputFormat GetDefaultOutputFormat() => Current.GetDefaultOutputFormat();

    /// <summary>
    /// Sets the default output format.
    /// After this step, all expressions will be printed according to the specified output format.
    /// </summary>
    /// <param name="defaultOutputFormat">The output format.</param>
    public static void SetDefaultOutputFormat(OutputFormat defaultOutputFormat) => Current.SetDefaultOutputFormat(defaultOutputFormat);

    /// <summary>
    /// Resets all tensor names in the namespace.
    /// Any tensor created before this method call becomes invalid and must not be used.
    /// This method is mainly used in unit tests, so avoid invocations of this method in general computations.
    /// </summary>
    public static void ResetTensorNames() => Current.ResetTensorNames();

    /// <summary>
    /// Resets all tensor names in the namespace and sets a specified seed to the <see cref="NameManager"/>.
    /// If this method is invoked with a constant seed before any interactions with Redberry,
    /// further behavior of Redberry will be fully deterministic from run to run
    /// (order of summands and multipliers will be fixed, computation time will be pretty constant, hash codes will be the same).
    /// Any tensor created before this method call becomes invalid and must not be used.
    /// This method is mainly used in unit tests, so avoid invocations of this method in general computations.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    public static void ResetTensorNames(int seed) => Current.ResetTensorNames(seed);
}
