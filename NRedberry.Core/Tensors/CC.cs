using NRedberry.Concurrent;
using NRedberry.Contexts;
using NRedberry.Indices;

namespace NRedberry.Tensors;

/// <summary>
/// Redberry current context.
/// This class statically delegates common useful methods from the Redberry context of the current session.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class CC
{
    /// <summary>
    /// Returns the current context of the Redberry session.
    /// </summary>
    public static Context Current => Context.Get();

    /// <summary>
    /// Returns true if a metric is defined for the specified index type.
    /// </summary>
    /// <param name="type">Index type.</param>
    public static bool IsMetric(byte type)
    {
        return Current.IsMetric(type);
    }

    /// <summary>
    /// Returns true if a metric is defined for the specified index type.
    /// </summary>
    /// <param name="type">Index type.</param>
    public static bool IsMetric(IndexType type)
    {
        return Current.IsMetric(type.GetType_());
    }

    /// <summary>
    /// Returns the <see cref="NameDescriptor"/> corresponding to the specified tensor name id.
    /// </summary>
    /// <param name="name">Integer name of the tensor.</param>
    public static NameDescriptor GetNameDescriptor(int name)
    {
        return Current.GetNameDescriptor(name);
    }

    /// <summary>
    /// Returns the name manager (namespace) of the current session.
    /// </summary>
    public static NameManager NameManager => Current.NameManager;

    /// <summary>
    /// Returns the index converter manager of the current session.
    /// </summary>
    public static IndexConverterManager IndexConverterManager => Current.ConverterManager;

    /// <summary>
    /// Returns the current default output format.
    /// </summary>
    public static OutputFormat DefaultOutputFormat
    {
        get => Current.DefaultOutputFormat;
        set => Current.DefaultOutputFormat = value;
    }

    /// <summary>
    /// Returns all metric index types.
    /// </summary>
    public static IReadOnlySet<IndexType> GetMetricTypes()
    {
        var result = new HashSet<IndexType>();
        foreach (var type in IndexTypeMethods.Values)
        {
            if (IsMetric(type))
            {
                result.Add(type);
            }
        }

        return result;
    }

    /// <summary>
    /// Returns all matrix index types.
    /// </summary>
    public static IReadOnlySet<IndexType> GetMatrixTypes()
    {
        var result = new HashSet<IndexType>();
        foreach (var type in IndexTypeMethods.Values)
        {
            if (!IsMetric(type))
            {
                result.Add(type);
            }
        }

        return result;
    }

    /// <summary>
    /// Returns current default output format.
    /// </summary>
    public static OutputFormat GetDefaultOutputFormat()
    {
        return DefaultOutputFormat;
    }

    /// <summary>
    /// Sets the default output format.
    /// </summary>
    /// <param name="defaultOutputFormat">Output format.</param>
    public static void SetDefaultOutputFormat(OutputFormat defaultOutputFormat)
    {
        DefaultOutputFormat = defaultOutputFormat;
    }

    /// <summary>
    /// This method resets all tensor names in the namespace.
    /// Any tensor created before this method call becomes invalid, and must not be used.
    /// This method is mainly used in unit tests, so avoid invocations of this method in general computations.
    /// </summary>
    public static void ResetTensorNames()
    {
        Current.ResetTensorNames();
    }

    /// <summary>
    /// Resets all definitions.
    /// </summary>
    public static void Reset()
    {
        Current.ResetTensorNames();
        Current.ParseManager.Reset();
    }

    /// <summary>
    /// Resets all tensor names in the namespace and seeds the <see cref="NameManager"/> deterministically.
    /// Any tensor created before this method call becomes invalid, and must not be used.
    /// This method is mainly used in unit tests, so avoid invocations of this method in general computations.
    /// </summary>
    /// <param name="seed">Seed applied to <see cref="NameManager"/>.</param>
    public static void ResetTensorNames(int seed)
    {
        Current.ResetTensorNames(seed);
    }

    /// <summary>
    /// Generates a new symbol which never used before during current session.
    /// </summary>
    public static SimpleTensor GenerateNewSymbol()
    {
        var nameDescriptor = Current.NameManager.GenerateNewSymbolDescriptor();
        return new SimpleTensor(nameDescriptor.Id, IndicesFactory.EmptySimpleIndices);
    }

    /// <summary>
    /// Return an output port which generates a new symbol at each <see cref="IOutputPort{T}.Take"/> invocation.
    /// </summary>
    public static IOutputPort<SimpleTensor> GetParametersGenerator()
    {
        return new GeneratedParameters();
    }

    /// <summary>
    /// Returns random generator used by Redberry in current session.
    /// </summary>
    public static Random GetRandomGenerator()
    {
        return Random.Shared;
    }

    /// <summary>
    /// Allows to parse expressions with repeated indices of the same variance.
    /// </summary>
    /// <param name="allowSameVariance">Allow or not to parse repeated indices with same variance.</param>
    public static void SetParserAllowsSameVariance(bool allowSameVariance)
    {
        Current.ParseManager.Parser.AllowSameVariance = allowSameVariance;
    }

    /// <summary>
    /// Returns whether repeated indices of the same variance are allowed to be parsed.
    /// </summary>
    public static bool GetParserAllowsSameVariance()
    {
        return Current.ParseManager.Parser.AllowSameVariance;
    }

    private sealed record class GeneratedParameters : IOutputPort<SimpleTensor>
    {
        public SimpleTensor Take()
        {
            return GenerateNewSymbol();
        }
    }
}
