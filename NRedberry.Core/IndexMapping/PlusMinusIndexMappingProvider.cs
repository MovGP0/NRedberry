using System;
using NRedberry.Core.Concurrent;

namespace NRedberry.Core.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/PlusMinusIndexMappingProvider.java
 */

internal sealed class PlusMinusIndexMappingProvider : IndexMappingProviderAbstract
{
    public PlusMinusIndexMappingProvider(IOutputPort<IIndexMappingBuffer> outputPort)
        : base(outputPort)
    {
        throw new NotImplementedException();
    }

    public override IIndexMappingBuffer? Take()
    {
        throw new NotImplementedException();
    }
}
