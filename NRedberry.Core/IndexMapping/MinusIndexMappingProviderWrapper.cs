namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MinusIndexMappingProviderWrapper.java
 */

internal sealed class MinusIndexMappingProviderWrapper : IIndexMappingProvider
{
    private readonly IIndexMappingProvider _provider;

    public MinusIndexMappingProviderWrapper(IIndexMappingProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        _provider = provider;
    }

    public IIndexMappingBuffer Take()
    {
        IIndexMappingBuffer buffer = _provider.Take();
        if (buffer is null)
        {
            return null!;
        }

        buffer.AddSign(true);
        return buffer;
    }

    public bool Tick()
    {
        return _provider.Tick();
    }
}
