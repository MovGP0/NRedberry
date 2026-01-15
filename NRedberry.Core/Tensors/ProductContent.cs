using System.Collections;

namespace NRedberry.Tensors;

public sealed class ProductContent : IEnumerable<Tensor>
{
    public static ProductContent EmptyInstance = new(
        StructureOfContractionsHashed.EmptyInstance,
        StructureOfContractions.EmptyFullContractionsStructure,
        [],
        null,
        [],
        [],
        []);

    public StructureOfContractionsHashed StructureOfContractionsHashed { get; }
    public StructureOfContractions StructureOfContractions { get; }

    private Tensor[] _scalars;

    public Tensor[] Scalars
    {
        get => (Tensor[])_scalars.Clone();
        private set => _scalars = value;
    }

    public Tensor? NonScalar { get; }

    private short[] StretchIndices { get; }
    public short[] StretchIds => (short[])StretchIndices.Clone();

    public short GetStretchId(long id) => StretchIndices[id];

    private Tensor[] Data { get; }

    public Tensor this[long i] => Data[i];

    public int Size => Data.Length;

    public ProductContent(
        StructureOfContractionsHashed structureOfContractionsHashed,
        StructureOfContractions structureOfContractions,
        Tensor[] scalars,
        Tensor nonScalar,
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
        Tensor[] scalars,
        Tensor? nonScalar,
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
        int length = to - from;
        Tensor[] result = new Tensor[length];
        Array.Copy(Data, from, result, 0, length);
        return result;
    }

    public Tensor[] GetDataCopy()
    {
        return (Tensor[])Data.Clone();
    }

    private int[]? StretchHashReflection { get; set; }

    public short GetStretchIndexByHash(int hashCode)
    {
        if (StretchHashReflection == null)
        {
            StretchHashReflection = new int[StretchIndices[StretchIndices.Length - 1] + 1];
            //TODO performance (!!!)
            for (var i = 0; i < StretchIndices.Length; ++i)
            {
                StretchHashReflection[StretchIndices[i]] = Data[i].GetHashCode();
            }
        }

        int index = Array.BinarySearch(StretchHashReflection, hashCode);
        if (index < 0)
        {
            return -1;
        }

        return (short)index;
    }

    public IEnumerator<Tensor> GetEnumerator()
    {
        return ((IEnumerable<Tensor>)Data).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
