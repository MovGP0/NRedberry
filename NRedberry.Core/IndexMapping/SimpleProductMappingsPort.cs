using System;
using NRedberry.Core.Concurrent;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.IndexMapping;

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
