namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MinusIndexMappingProviderWrapper.java
 */

internal sealed class MinusIndexMappingProviderWrapper : IIndexMappingProvider
{
    private readonly IIndexMappingProvider provider;

    public MinusIndexMappingProviderWrapper(IIndexMappingProvider provider)
    {
        this.provider = provider;
        throw new NotImplementedException();
    }

    public IIndexMappingBuffer Take()
    {
        throw new NotImplementedException();
    }

    public bool Tick()
    {
        throw new NotImplementedException();
    }
}
