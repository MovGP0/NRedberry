using System;
using System.Linq;

namespace NRedberry.Core.Tensors;

public sealed class ProductContent
{
    public static ProductContent EmptyInstance = new(
        StructureOfContractionsHashed.EmptyInstance,
        StructureOfContractions.EmptyFullContractionsStructure,
        new Tensor[0], null, new short[0], new Tensor[0], new int[0]);

    public StructureOfContractionsHashed StructureOfContractionsHashed { get; }
    public StructureOfContractions StructureOfContractions { get; }

    private Tensor[] _scalars;
    public Tensor[] Scalars
    {
        get => (Tensor[])_scalars.Clone();
        private set => _scalars = value;
    }

    public Tensor NonScalar { get; }

    private short[] StretchIndices { get; }
    public short[] StretchIds => (short[])StretchIndices.Clone();
    public short GetStretchId(long id) => StretchIndices[id];

    private Tensor[] Data { get; }
    public Tensor this[long i] => Data[i];
    public int Size => Data.Length;

    public ProductContent(
        StructureOfContractionsHashed structureOfContractionsHashed,
        StructureOfContractions structureOfContractions,
        Tensor[] scalars, Tensor nonScalar,
        short[] stretchIndices,
        Tensor[] data)
    {
        StructureOfContractionsHashed = structureOfContractionsHashed;
        StructureOfContractions = structureOfContractions;
        Scalars = scalars;
        NonScalar = nonScalar;
        StretchIndices = stretchIndices;
        Data = data;
    }

    private ProductContent(
        StructureOfContractionsHashed structureOfContractionsHashed,
        StructureOfContractions structureOfContractions,
        Tensor[] scalars, Tensor nonScalar,
        short[] stretchIndices,
        Tensor[] data,
        int[] stretchHashReflection)
    {
        StructureOfContractionsHashed = structureOfContractionsHashed;
        StructureOfContractions = structureOfContractions;
        Scalars = scalars;
        NonScalar = nonScalar;
        StretchIndices = stretchIndices;
        Data = data;
        StretchHashReflection = stretchHashReflection;
    }

    public Tensor[] GetRange(int from, int to)
    {
        return Data.Skip(from).Take(to-from).ToArray();
    }

    public Tensor[] GetDataCopy()
    {
        return (Tensor[])Data.Clone();
    }

    private int[] StretchHashReflection { get; set; }

    public short GetStretchIndexByHash(int hashCode)
    {
        if (StretchHashReflection == null)
        {
            StretchHashReflection = new int[StretchIndices[StretchIndices.Length - 1] + 1];
            //TODO performance (!!!)
            for (var i = 0; i < StretchIndices.Length; ++i)
                StretchHashReflection[StretchIndices[i]] = Data[i].GetHashCode();
        }

        return (short)Array.IndexOf(StretchHashReflection, hashCode);
    }
}