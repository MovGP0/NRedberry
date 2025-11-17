using NRedberry.Contexts;

namespace NRedberry.Core.Tensors;

/// <summary>
/// Redberry current context.
/// This class statically delegates common useful methods from Redberry context of current session.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class CC
{
    private static Context _current;

    public static Context Current => _current ??= new Context(new ContextSettings());

    /**
         * Returns true if metric is defined for specified index type.
         *
         * @param type index type
         * @return true if metric is defined for specified index type
         */
    public static bool IsMetric(byte type)
    {
        return Current.IsMetric(type);
    }

    /**
         * Returns {@code NameDescriptor} corresponding to the specified {@code int} nameId.
         *
         * @param name integer name of tensor
         * @return corresponding  {@code NameDescriptor}
         */
    public static NameDescriptor GetNameDescriptor(int name)
    {
        return Current.GetNameDescriptor(name);
    }

    /**
         * Returns the name manager (namespace) of current session.
         *
         * @return the name manager (namespace) of current session.
         */
    public static NameManager NameManager => Current.NameManager;

    /**
         * Returns index converter manager of current session.
         *
         * @return index converter manager of current session
         */
    public static IndexConverterManager GetIndexConverterManager()
    {
        return Current.ConverterManager;
    }

    /**
         * Returns current default output format.
         *
         * @return current default output format
         */
    public static OutputFormat GetDefaultOutputFormat()
    {
        return Current.DefaultOutputFormat;
    }

    /**
         * Sets the default output format.
         *
         * <p>After this step, all expressions will be printed according to the specified output format.</p>
         *
         * @param defaultOutputFormat output format
         */
    public static void SetDefaultOutputFormat(OutputFormat defaultOutputFormat)
    {
        Current.DefaultOutputFormat = defaultOutputFormat;
    }

    /**
         * This method resets all tensor names in the namespace.
         *
         * <p>Any tensor created before this method call becomes invalid, and
         * must not be used! This method is mainly used in unit tests, so
         * avoid invocations of this method in general computations.</p>
         */
    public static void ResetTensorNames()
    {
        Current.ResetTensorNames();
    }

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
    public static void ResetTensorNames(int seed)
    {
        Current.ResetTensorNames(seed);
    }
}
