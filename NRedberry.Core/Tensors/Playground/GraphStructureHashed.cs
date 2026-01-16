namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructureHashed.java
 */

public sealed class GraphStructureHashed
{
    public static GraphStructureHashed EmptyInstance { get; } = new([], [], []);

    public short[] StretchIndices { get; }
    public long[] FreeContraction { get; }
    public long[][] Contractions { get; }

    public GraphStructureHashed(short[] stretchIndices, long[] freeContraction, long[][] contractions)
    {
        StretchIndices = stretchIndices;
        FreeContraction = freeContraction;
        Contractions = contractions;
    }
}
