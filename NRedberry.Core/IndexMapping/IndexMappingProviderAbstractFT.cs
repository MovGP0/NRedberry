using NRedberry.Concurrent;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderAbstractFT.java
 */

public abstract class IndexMappingProviderAbstractFT<T> : IndexMappingProviderAbstract
    where T : Tensor
{
    protected T From { get; }
    protected T To { get; }

    protected IndexMappingProviderAbstractFT(IOutputPort<IIndexMappingBuffer> outputPort, T fromTensor, T toTensor)
        : base(outputPort)
    {
        ArgumentNullException.ThrowIfNull(fromTensor);
        ArgumentNullException.ThrowIfNull(toTensor);

        From = fromTensor;
        To = toTensor;
    }
}
