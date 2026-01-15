using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/PlusMinusIndexMappingProvider.java
 */

internal sealed class PlusMinusIndexMappingProvider : IndexMappingProviderAbstract
{
    private bool _state;

    public PlusMinusIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
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
