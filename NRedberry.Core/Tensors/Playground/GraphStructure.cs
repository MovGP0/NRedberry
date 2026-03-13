namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructure.java
 */

public sealed class GraphStructure
{
    public static GraphStructure EmptyFullContractionsStructure { get; } = new([], [], [], 0);

    public long[] FreeContractions { get; }
    public long[][] Contractions { get; }
    public int[] Components { get; }
    public int ComponentCount { get; }

    public GraphStructure(long[] freeContractions, long[][] contractions, int[] components, int componentCount)
    {
        FreeContractions = freeContractions;
        Contractions = contractions;
        Components = components;
        ComponentCount = componentCount;
    }

    public GraphStructure(Tensor[] data, int differentIndicesCount, Indices.Indices freeIndices)
    {
        throw new NotImplementedException();
    }
}
