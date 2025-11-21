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
        throw new NotImplementedException();
    }

    public static bool TestMapping(Mapping mapping, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public static bool AnyMappingExists(Tensor a, Tensor b)
    {
        throw new NotImplementedException();
    }

    public static bool MappingExists(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public static bool PositiveMappingExists(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public static bool Equals(Tensor u, Tensor v)
    {
        throw new NotImplementedException();
    }

    public static bool? Compare1(Tensor u, Tensor v)
    {
        throw new NotImplementedException();
    }

    public static bool IsZeroDueToSymmetry(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static ISet<Mapping> GetAllMappings(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    private static ISet<Mapping> GetAllMappings(IOutputPort<Mapping> outputPort)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    internal static IIndexMappingProvider CreatePort(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }
}
