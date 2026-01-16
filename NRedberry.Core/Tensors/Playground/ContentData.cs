namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/ContentData.java
 */

public sealed class ContentData
{
    public static ContentData EmptyInstance { get; } = new(
        GraphStructureHashed.EmptyInstance,
        GraphStructure.EmptyFullContractionsStructure,
        [],
        [],
        []);

    public GraphStructureHashed StructureOfContractionsHashed { get; }
    public GraphStructure StructureOfContractions { get; }
    public Tensor[] Data { get; }
    public short[] StretchIndices { get; }
    public int[] HashCodes { get; }

    public ContentData(
        GraphStructureHashed structureOfContractionsHashed,
        GraphStructure structureOfContractions,
        Tensor[] data,
        short[] stretchIndices,
        int[] hashCodes)
    {
        StructureOfContractionsHashed = structureOfContractionsHashed;
        StructureOfContractions = structureOfContractions;
        Data = data;
        StretchIndices = stretchIndices;
        HashCodes = hashCodes;
    }
}
