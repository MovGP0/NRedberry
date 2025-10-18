using System;
using NRedberry.Core.Concurrent;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/MinusIndexMappingProvider.java
 */

internal sealed class MinusIndexMappingProvider : IndexMappingProviderAbstract
{
    public MinusIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
        : base(outputPort)
    {
        throw new NotImplementedException();
    }

    public override IIndexMappingBuffer? Take()
    {
        throw new NotImplementedException();
    }
}
