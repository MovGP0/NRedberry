using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProvider.java
 */

public interface IIndexMappingProvider : IOutputPort<IIndexMappingBuffer>
{
    bool Tick();
}

public static class IndexMappingProviderUtil
{
    public static IIndexMappingProvider EmptyProvider => throw new NotImplementedException();

    public static IIndexMappingProvider Singleton(IIndexMappingBuffer buffer)
    {
        throw new NotImplementedException();
    }
}
