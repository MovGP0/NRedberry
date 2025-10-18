using System;
using NRedberry.Core.Concurrent;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MappingsPortRemovingContracted.java
 */

internal sealed class MappingsPortRemovingContracted : IOutputPort<IIndexMappingBuffer>
{
    private readonly IOutputPort<IIndexMappingBuffer> provider;

    public MappingsPortRemovingContracted(IOutputPort<IIndexMappingBuffer> provider)
    {
        this.provider = provider;
        throw new NotImplementedException();
    }

    public IIndexMappingBuffer Take()
    {
        throw new NotImplementedException();
    }
}
