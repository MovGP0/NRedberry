using NRedberry.Core.Tensors;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProviderFactory.java
 */

public interface IIndexMappingProviderFactory
{
    IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to);
}
