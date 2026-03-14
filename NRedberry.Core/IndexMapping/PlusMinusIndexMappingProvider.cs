using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/PlusMinusIndexMappingProvider.java
 */

internal sealed class PlusMinusIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
    : IndexMappingProviderAbstract(outputPort)
{
    private bool _state;

    public override IIndexMappingBuffer? Take()
    {
        if (currentBuffer is null)
        {
            return null;
        }

        IIndexMappingBuffer buffer = currentBuffer;
        if (_state)
        {
            currentBuffer = null;
            buffer.AddSign(true);
        }
        else
        {
            buffer = (IIndexMappingBuffer)buffer.Clone();
            _state = true;
        }

        return buffer;
    }
}
