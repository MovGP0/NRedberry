using NRedberry.Core.Tensors;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderFunctions.java
 */

internal static class ProviderFunctions
{
    public static IIndexMappingProviderFactory OddFactory => throw new NotImplementedException();

    public static IIndexMappingProviderFactory EvenFactory => throw new NotImplementedException();

    public static IIndexMappingProviderFactory Factory => throw new NotImplementedException();

    public static IIndexMappingProvider CreateOdd(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public static IIndexMappingProvider CreateEven(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public static IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }
}
