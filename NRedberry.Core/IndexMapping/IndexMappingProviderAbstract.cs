using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderAbstract.java
 */

public abstract class IndexMappingProviderAbstract : IIndexMappingProvider
{
    protected IIndexMappingBuffer? currentBuffer;
    private readonly IOutputPort<IIndexMappingBuffer> outputPort;

    protected IndexMappingProviderAbstract(IOutputPort<IIndexMappingBuffer> outputPort)
    {
        throw new NotImplementedException();
    }

    public virtual bool Tick()
    {
        throw new NotImplementedException();
    }

    public virtual IIndexMappingBuffer? Take()
    {
        throw new NotImplementedException();
    }

    protected virtual void BeforeTick()
    {
        throw new NotImplementedException();
    }
}
