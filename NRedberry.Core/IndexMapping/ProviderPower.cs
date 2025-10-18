using NRedberry.Core.Tensors;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderPower.java
 */

internal sealed class ProviderPower : IIndexMappingProviderFactory
{
    public static ProviderPower Instance { get; } = new();

    private ProviderPower()
    {
    }

    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }
}
