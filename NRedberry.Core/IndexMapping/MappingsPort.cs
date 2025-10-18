using NRedberry.Core.Concurrent;
using NRedberry.Core.Indices;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MappingsPort.java
 */

public sealed class MappingsPort : IOutputPort<IIndexMapping>
{
    private readonly IOutputPort<IIndexMappingBuffer> innerPort;

    public MappingsPort(IOutputPort<IIndexMappingBuffer> innerPort)
    {
        this.innerPort = innerPort;
        throw new NotImplementedException();
    }

    public IIndexMapping Take()
    {
        throw new NotImplementedException();
    }
}
