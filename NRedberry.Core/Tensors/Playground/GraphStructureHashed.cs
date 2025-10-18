namespace NRedberry.Core.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/GraphStructureHashed.java
 */

public sealed class GraphStructureHashed
{
    public static GraphStructureHashed EmptyInstance { get; } =
        new GraphStructureHashed([], [], []);

    public GraphStructureHashed(short[] stretchIndices, long[] freeContraction, long[][] contractions)
    {
        throw new NotImplementedException();
    }
}
