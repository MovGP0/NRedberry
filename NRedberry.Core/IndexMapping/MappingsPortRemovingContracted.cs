using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MappingsPortRemovingContracted.java
 */

internal sealed class MappingsPortRemovingContracted : IOutputPort<IIndexMappingBuffer>
{
    private readonly IOutputPort<IIndexMappingBuffer> _provider;

    public MappingsPortRemovingContracted(IOutputPort<IIndexMappingBuffer> provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _provider = provider;
    }

    public IIndexMappingBuffer Take()
    {
        IIndexMappingBuffer buffer = _provider.Take();
        buffer?.RemoveContracted();

        return buffer;
    }
}
