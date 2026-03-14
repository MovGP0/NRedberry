namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructureHashed.java
 */

public sealed class GraphStructureHashed(short[] stretchIndices, long[] freeContraction, long[][] contractions)
{
    public static GraphStructureHashed EmptyInstance { get; } = new([], [], []);

    public short[] StretchIndices { get; } = stretchIndices;
    public long[] FreeContraction { get; } = freeContraction;
    public long[][] Contractions { get; } = contractions;
}
