using NRedberry.Core.Concurrent;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/DummyIndexMappingProvider.java
 */

internal sealed class DummyIndexMappingProvider : IndexMappingProviderAbstract
{
    public DummyIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
        : base(outputPort)
    {
        throw new NotImplementedException();
    }

    public override IIndexMappingBuffer? Take()
    {
        throw new NotImplementedException();
    }
}
