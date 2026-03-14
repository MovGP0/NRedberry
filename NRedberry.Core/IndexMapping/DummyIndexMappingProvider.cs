using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/DummyIndexMappingProvider.java
 */

internal sealed class DummyIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
    : IndexMappingProviderAbstract(outputPort)
{
    public override IIndexMappingBuffer? Take()
    {
        IIndexMappingBuffer? buffer = currentBuffer;
        currentBuffer = null;
        return buffer;
    }
}
