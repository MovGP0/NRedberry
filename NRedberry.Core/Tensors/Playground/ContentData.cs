namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/ContentData.java
 */

public sealed class ContentData(
    GraphStructureHashed structureOfContractionsHashed,
    GraphStructure structureOfContractions,
    Tensor[] data,
    short[] stretchIndices,
    int[] hashCodes)
{
    public static ContentData EmptyInstance { get; } = new(
        GraphStructureHashed.EmptyInstance,
        GraphStructure.EmptyFullContractionsStructure,
        [],
        [],
        []);

    public GraphStructureHashed StructureOfContractionsHashed { get; } = structureOfContractionsHashed;
    public GraphStructure StructureOfContractions { get; } = structureOfContractions;
    public Tensor[] Data { get; } = data;
    public short[] StretchIndices { get; } = stretchIndices;
    public int[] HashCodes { get; } = hashCodes;
}
