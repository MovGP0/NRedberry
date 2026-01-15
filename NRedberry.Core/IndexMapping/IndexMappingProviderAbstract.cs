using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderAbstract.java
 */

public abstract class IndexMappingProviderAbstract : IIndexMappingProvider
{
    protected IIndexMappingBuffer? currentBuffer;
    private readonly IOutputPort<IIndexMappingBuffer> _outputPort;

    protected IndexMappingProviderAbstract(IOutputPort<IIndexMappingBuffer> outputPort)
    {
        ArgumentNullException.ThrowIfNull(outputPort);
        _outputPort = outputPort;
    }

    public virtual bool Tick()
    {
        BeforeTick();
        currentBuffer = _outputPort.Take();
        return currentBuffer is not null;
    }

    public abstract IIndexMappingBuffer? Take();

    protected virtual void BeforeTick()
    {
    }
}
