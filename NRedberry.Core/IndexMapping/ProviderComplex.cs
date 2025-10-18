using System;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderComplex.java
 */

internal static class ProviderComplex
{
    public static IIndexMappingProviderFactory Factory => throw new NotImplementedException();

    public static IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }
}
