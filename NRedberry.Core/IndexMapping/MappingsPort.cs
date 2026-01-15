using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MappingsPort.java
 */

public sealed class MappingsPort : IOutputPort<Mapping>
{
    private readonly IOutputPort<IIndexMappingBuffer> _innerPort;

    public MappingsPort(IOutputPort<IIndexMappingBuffer> innerPort)
    {
        ArgumentNullException.ThrowIfNull(innerPort);

        _innerPort = innerPort;
    }

    public Mapping Take()
    {
        IIndexMappingBuffer? buffer = _innerPort.Take();
        if (buffer is null)
        {
            return null!;
        }

        return new Mapping(buffer);
    }
}
