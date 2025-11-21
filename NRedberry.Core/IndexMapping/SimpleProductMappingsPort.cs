using NRedberry.Concurrent;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/SimpleProductMappingsPort.java
 */

internal sealed class SimpleProductMappingsPort : IOutputPort<IIndexMappingBuffer>
{
    public SimpleProductMappingsPort(IIndexMappingProvider[] providers)
    {
        throw new NotImplementedException();
    }

    public SimpleProductMappingsPort(IIndexMappingProvider provider, Tensor[] from, Tensor[] to)
    {
        throw new NotImplementedException();
    }

    public IIndexMappingBuffer Take()
    {
        throw new NotImplementedException();
    }
}
