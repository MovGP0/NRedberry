using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MinusIndexMappingProvider.java
 */

internal sealed class MinusIndexMappingProvider : IndexMappingProviderAbstract
{
    public MinusIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
        : base(outputPort)
    {
    }

    public override IIndexMappingBuffer? Take()
    {
        if (currentBuffer is null)
        {
            return null;
        }

        IIndexMappingBuffer buffer = currentBuffer;
        currentBuffer = null;
        buffer.AddSign(true);
        return buffer;
    }
}
