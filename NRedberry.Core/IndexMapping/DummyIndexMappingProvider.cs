using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/DummyIndexMappingProvider.java
 */

internal sealed class DummyIndexMappingProvider : IndexMappingProviderAbstract
{
    public DummyIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
        : base(outputPort)
    {
    }

    public override IIndexMappingBuffer? Take()
    {
        IIndexMappingBuffer? buffer = currentBuffer;
        currentBuffer = null;
        return buffer;
    }
}
