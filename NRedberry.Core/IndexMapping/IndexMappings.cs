using NRedberry.Concurrent;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/// <summary>
/// Skeleton port of cc.redberry.core.indexmapping.IndexMappings.
/// </summary>
public static class IndexMappings
{
    private static IReadOnlyDictionary<Type, IIndexMappingProviderFactory> ProviderFactories => throw new NotImplementedException();

    public static MappingsPort CreatePort(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public static MappingsPort SimpleTensorsPort(SimpleTensor from, SimpleTensor to)
    {
        throw new NotImplementedException();
    }

    public static MappingsPort CreateBijectiveProductPort(Tensor[] from, Tensor[] to)
    {
        throw new NotImplementedException();
    }

    public static Mapping? GetFirst(Tensor from, Tensor to)
    {
        bool? comparison = Compare1(from, to);
        if (comparison is null)
        {
            return null;
        }

        return Mapping.IdentityMapping.AddSign(comparison.Value);
    }

    public static bool TestMapping(Mapping mapping, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return mapping.Equals(GetFirst(from, to));
    }

    public static bool AnyMappingExists(Tensor a, Tensor b)
    {
        return MappingExists(a, b) || MappingExists(b, a);
    }

    public static bool MappingExists(Tensor from, Tensor to)
    {
        return Compare1(from, to) is not null;
    }

    public static bool PositiveMappingExists(Tensor from, Tensor to)
    {
        return Compare1(from, to) == false;
    }

    public static bool Equals(Tensor u, Tensor v)
    {
        return Compare1(u, v) == false;
    }

    public static bool? Compare1(Tensor u, Tensor v)
    {
        ArgumentNullException.ThrowIfNull(u);
        ArgumentNullException.ThrowIfNull(v);

        if (!u.Indices.GetFree().EqualsRegardlessOrder(v.Indices.GetFree()))
        {
            return null;
        }

        if (TensorUtils.EqualsExactly(u, v))
        {
            return false;
        }

        return TensorUtils.EqualsExactly(NRedberry.Tensors.Tensors.Negate(u), v)
            || TensorUtils.EqualsExactly(u, NRedberry.Tensors.Tensors.Negate(v))
            ? true
            : null;
    }

    public static bool IsZeroDueToSymmetry(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static ISet<Mapping> GetAllMappings(Tensor from, Tensor to)
    {
        Mapping? mapping = GetFirst(from, to);
        return mapping is null ? new HashSet<Mapping>() : new HashSet<Mapping> { mapping };
    }

    private static ISet<Mapping> GetAllMappings(IOutputPort<Mapping> outputPort)
    {
        ArgumentNullException.ThrowIfNull(outputPort);

        HashSet<Mapping> mappings = [];
        Mapping? mapping;
        while ((mapping = outputPort.Take()) is not null)
        {
            mappings.Add(mapping);
        }

        return mappings;
    }

    internal static IOutputPort<IIndexMappingBuffer> CreatePortOfBuffers(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    internal static IOutputPort<IIndexMappingBuffer> CreatePortOfBuffers(IIndexMappingBuffer buffer, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    internal static IIndexMappingBuffer? GetFirstBuffer(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    private static Tensor? ExtractNonComplexFactor(Tensor tensor)
    {
        if (tensor is not Product product)
        {
            return null;
        }

        return product.Factor.IsMinusOne() ? product[1] : null;
    }

    internal static IIndexMappingProvider CreatePort(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }
}
