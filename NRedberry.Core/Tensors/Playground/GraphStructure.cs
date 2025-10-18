namespace NRedberry.Core.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructure.java
 */

public sealed class GraphStructure
{
    public static GraphStructure EmptyFullContractionsStructure { get; } =
        new GraphStructure([], [], [], 0);

    public GraphStructure(long[] freeContractions, long[][] contractions, int[] components, int componentCount)
    {
        throw new NotImplementedException();
    }

    public GraphStructure(Tensor[] data, int differentIndicesCount, Indices.Indices freeIndices)
    {
        throw new NotImplementedException();
    }
}
