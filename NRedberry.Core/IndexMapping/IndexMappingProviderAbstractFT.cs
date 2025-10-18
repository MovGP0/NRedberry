using NRedberry.Core.Concurrent;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderAbstractFT.java
 */

public abstract class IndexMappingProviderAbstractFT<T> : IndexMappingProviderAbstract
    where T : Tensor
{
    protected readonly T from;
    protected readonly T to;

    protected IndexMappingProviderAbstractFT(IOutputPort<IIndexMappingBuffer> outputPort, T fromTensor, T toTensor)
        : base(outputPort)
    {
        throw new NotImplementedException();
    }
}
