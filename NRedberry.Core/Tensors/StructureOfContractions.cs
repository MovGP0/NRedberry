using System.Diagnostics;
using System.Text;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Indices;

namespace NRedberry.Tensors;

public sealed class StructureOfContractions
{
    public static readonly StructureOfContractions EmptyFullContractionsStructure = new(Array.Empty<long[]>());

    public readonly long[] freeContractions;
    public readonly long[][] contractions;
    public readonly int[] components;
    public readonly int componentCount;

    public StructureOfContractions(long[][] contractions)
    {
        this.contractions = contractions;
        freeContractions = Array.Empty<long>();
        components = new int[contractions.Length];
        Array.Fill(components, -1);
        int componentCounter = -1;
        for (int i = 0; i < contractions.Length; i++)
        {
            if (components[i] == -1)
            {
                components[i] = ++componentCounter;
                FillComponents(components, componentCounter, i);
            }
        }

        componentCount = componentCounter + 1;
    }

    public StructureOfContractions(long[][] contractions, int[] components, int componentCount)
    {
        this.contractions = contractions;
        this.components = components;
        this.componentCount = componentCount;
        freeContractions = Array.Empty<long>();
    }

    private StructureOfContractions(long[] freeContractions, long[][] contractions, int[] components, int componentCount)
    {
        this.freeContractions = freeContractions;
        this.contractions = contractions;
        this.components = components;
        this.componentCount = componentCount;
    }

    public StructureOfContractions(Tensor[] data, int differentIndicesCount, Indices.Indices freeIndices)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(freeIndices);

        int[] upperIndices = new int[differentIndicesCount];
        int[] lowerIndices = new int[differentIndicesCount];
        long[] upperInfo = new long[differentIndicesCount];
        long[] lowerInfo = new long[differentIndicesCount];

        int[][] indices = [lowerIndices, upperIndices];
        long[][] info = [lowerInfo, upperInfo];

        int[] pointer = new int[2];

        contractions = new long[data.Length][];
        freeContractions = new long[freeIndices.Size()];

        int state;
        int index;

        for (int i = 0; i < freeIndices.Size(); ++i)
        {
            index = freeIndices[i];
            state = 1 - IndicesUtils.GetStateInt(index);
            info[state][pointer[state]] = DummyTensorInfo;
            indices[state][pointer[state]++] = IndicesUtils.GetNameWithType(index);
        }

        for (int tensorIndex = 0; tensorIndex < data.Length; ++tensorIndex)
        {
            Indices.Indices tensorIndices = data[tensorIndex].Indices;
            short[] diffIds = tensorIndices.GetDiffIds();

            if (tensorIndices.Size() >= 0x10000)
            {
                throw new InvalidOperationException("Too many indices!!! max count = 2^16");
            }

            for (int i = 0; i < tensorIndices.Size(); ++i)
            {
                index = tensorIndices[i];
                state = IndicesUtils.GetStateInt(index);
                info[state][pointer[state]] = PackToLong(tensorIndex, diffIds[i], i);
                indices[state][pointer[state]++] = IndicesUtils.GetNameWithType(index);
            }

            contractions[tensorIndex] = new long[tensorIndices.Size()];
        }

        ArraysUtils.QuickSort(indices[0], info[0]);
        ArraysUtils.QuickSort(indices[1], info[1]);

        int[] infoTensorIndicesFrom = InfoToTensorIndices(lowerInfo);
        int[] infoTensorIndicesTo = InfoToTensorIndices(upperInfo);

        int shift = 0;
        int last = 0;
        for (int i = 0; i < infoTensorIndicesFrom.Length; ++i)
        {
            if (infoTensorIndicesFrom[i] == -1 || infoTensorIndicesTo[i] == -1)
            {
                Array.Copy(infoTensorIndicesFrom, last, infoTensorIndicesFrom, last - shift, i - last);
                Array.Copy(infoTensorIndicesTo, last, infoTensorIndicesTo, last - shift, i - last);
                last = i + 1;
                ++shift;
            }
        }

        Array.Copy(infoTensorIndicesFrom, last, infoTensorIndicesFrom, last - shift, infoTensorIndicesFrom.Length - last);
        Array.Copy(infoTensorIndicesTo, last, infoTensorIndicesTo, last - shift, infoTensorIndicesTo.Length - last);
        if (shift != 0)
        {
            Array.Resize(ref infoTensorIndicesFrom, infoTensorIndicesFrom.Length - shift);
            Array.Resize(ref infoTensorIndicesTo, infoTensorIndicesTo.Length - shift);
        }

        int[] componentInfo = GraphUtils.CalculateConnectedComponents(infoTensorIndicesFrom, infoTensorIndicesTo, data.Length);
        componentCount = componentInfo[^1];
        components = componentInfo[..^1];

        Debug.Assert(indices[0].SequenceEqual(indices[1]));

        int freePointer = 0;
        for (int i = 0; i < differentIndicesCount; ++i)
        {
            int tensorIndex = unchecked((int)(0xFFFFFFFFL & (info[0][i] >> 16)));
            int indexIndex = (int)(0xFFFFL & (info[0][i] >> 48));
            long contraction = (UpperInfoMask & (info[1][i] << 16))
                | (0xFFFFL & info[0][i]);
            if (tensorIndex == -1)
            {
                freeContractions[freePointer++] = contraction;
            }
            else
            {
                contractions[tensorIndex][indexIndex] = contraction;
            }

            tensorIndex = unchecked((int)(0xFFFFFFFFL & (info[1][i] >> 16)));
            indexIndex = (int)(0xFFFFL & (info[1][i] >> 48));
            contraction = (UpperInfoMask & (info[0][i] << 16))
                | (0xFFFFL & info[1][i]);
            if (tensorIndex == -1)
            {
                freeContractions[freePointer++] = contraction;
            }
            else
            {
                contractions[tensorIndex][indexIndex] = contraction;
            }
        }
    }

    public Contraction[] GetContractedWith(int position)
    {
        long[] indicesContractions = contractions[position];
        int[] involvedTensors = new int[contractions.Length];
        Array.Fill(involvedTensors, -1);
        List<int>?[] indicesFrom = new List<int>[contractions.Length];
        List<int>?[] indicesTo = new List<int>[contractions.Length];

        List<int> freeIndices = [];
        int tensorsCount = 0;
        for (int i = 0; i < indicesContractions.Length; ++i)
        {
            int tensorIndex = GetToTensorIndex(indicesContractions[i]);
            if (tensorIndex == -1)
            {
                freeIndices.Add(i);
                continue;
            }

            if (involvedTensors[tensorIndex] == -1)
            {
                involvedTensors[tensorIndex] = tensorIndex;
                indicesFrom[tensorIndex] = [];
                indicesTo[tensorIndex] = [];
                ++tensorsCount;
            }

            indicesFrom[tensorIndex]!.Add(i);
            indicesTo[tensorIndex]!.Add(GetToIndexId(indicesContractions[i]));
        }

        int f = freeIndices.Count == 0 ? 0 : 1;
        Contraction[] result = new Contraction[tensorsCount + f];
        if (f == 1)
        {
            result[0] = new Contraction(-1, freeIndices.ToArray(), null);
        }

        tensorsCount = f;
        for (int i = 0; i < contractions.Length; ++i)
        {
            if (involvedTensors[i] == -1)
            {
                continue;
            }

            result[tensorsCount++] = new Contraction(involvedTensors[i], indicesFrom[i]!.ToArray(), indicesTo[i]!.ToArray());
        }

        return result;
    }

    public sealed class Contraction
    {
        public Contraction(int tensor, int[] indicesFrom, int[]? indicesTo)
        {
            Tensor = tensor;
            IndicesFrom = indicesFrom;
            IndicesTo = indicesTo;
        }

        public int Tensor { get; }

        public int[] IndicesFrom { get; }

        public int[]? IndicesTo { get; }

        public override string ToString()
        {
            return $"tensor: {Tensor}, indices from: [{string.Join(", ", IndicesFrom)}], indices to: [{string.Join(", ", IndicesTo ?? Array.Empty<int>())}]";
        }
    }

    private void FillComponents(int[] components, int component, int position)
    {
        foreach (long l in contractions[position])
        {
            int to = ToPosition(l);
            if (to != -1 && components[to] == -1)
            {
                components[to] = component;
                FillComponents(components, component, to);
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < contractions.Length; i++)
        {
            foreach (long l in contractions[i])
            {
                sb.Append(i).Append('_').Append(FromIPosition(l)).Append(" -> ").Append(ToPosition(l)).Append('_').Append(ToIDiffId(l));
                sb.Append('\n');
            }
        }

        return sb.ToString();
    }

    public static int GetToTensorIndex(long contraction)
    {
        return (int)(contraction >> 32);
    }

    public static int GetToIndexId(long contraction)
    {
        return (short)(0xFFFF & (contraction >> 16));
    }

    public static int GetFromIndexId(long contraction)
    {
        return (short)(0xFFFF & contraction);
    }

    public static int ToPosition(long contraction)
    {
        return GetToTensorIndex(contraction);
    }

    public static short ToIDiffId(long contraction)
    {
        return (short)GetToIndexId(contraction);
    }

    public static int FromIPosition(long contraction)
    {
        return GetFromIndexId(contraction);
    }

    private static long PackToLong(int tensorIndex, short id, int indexIndex)
    {
        return (((long)tensorIndex) << 16) | (0xFFFFL & id) | (((long)indexIndex) << 48);
    }

    private const long UpperInfoMask = unchecked((long)0xFFFFFFFFFFFF0000UL);
    private const long DummyTensorInfo = UpperInfoMask;

    private static int[] InfoToTensorIndices(long[] info)
    {
        int[] result = new int[info.Length];
        for (int i = 0; i < info.Length; ++i)
        {
            result[i] = unchecked((int)(0xFFFFFFFFL & (info[i] >> 16)));
        }

        return result;
    }
}
