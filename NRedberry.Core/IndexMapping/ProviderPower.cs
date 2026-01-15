using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

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
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IIndexMappingBuffer? exponentMapping = IndexMappings.GetFirstBuffer(from[1], to[1]);
        if (exponentMapping?.GetSign() != false)
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        IIndexMappingBuffer? baseMapping = IndexMappings.GetFirstBuffer(from[0], to[0]);
        if (baseMapping is null)
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        if (!baseMapping.GetSign())
        {
            return new DummyIndexMappingProvider(provider);
        }

        if (from[1] is not Complex exponent)
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        if (NumberUtils.IsIntegerEven(exponent))
        {
            return new DummyIndexMappingProvider(provider);
        }

        if (NumberUtils.IsIntegerOdd(exponent))
        {
            return new MinusIndexMappingProvider(provider);
        }

        return IndexMappingProviderUtil.EmptyProvider;
    }
}
