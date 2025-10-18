using System;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/ContentData.java
 */

public sealed class ContentData
{
    public static ContentData EmptyInstance { get; } =
        new ContentData(GraphStructureHashed.EmptyInstance, null, Array.Empty<Tensor>(), Array.Empty<short>(), Array.Empty<int>());

    public ContentData(GraphStructureHashed structureOfContractionsHashed,
                       GraphStructure? structureOfContractions,
                       Tensor[] data,
                       short[] stretchIndices,
                       int[] hashCodes)
    {
        throw new NotImplementedException();
    }
}
