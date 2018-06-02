using System;
using System.Diagnostics;
using System.Linq;
using NRedberry.Core.Graphs;
using NRedberry.Core.Indices;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Tensors
{
    public sealed class StructureOfContractions
    {
        public static StructureOfContractions EmptyFullContractionsStructure =
            new StructureOfContractions(new long[0], new long[0][], new int[0], 0);

        public long[] freeContractions;
        public long[][] contractions;
        public int[] components;
        public int componentCount;

        private StructureOfContractions(long[] freeContractions, long[][] contractions, int[] components, int componentCount)
        {
            this.freeContractions = freeContractions;
            this.contractions = contractions;
            this.components = components;
            this.componentCount = componentCount;
        }

        public StructureOfContractions(Tensor[] data, int differentIndicesCount, IIndices freeIndices)
        {
            //Names (names with type, see IndicesUtils.getNameWithType() ) of all indices in this multiplication
            //It will be used as index name -> index index [0,1,2,3...] mapping
            int[] upperIndices = new int[differentIndicesCount], lowerIndices = new int[differentIndicesCount];
            //This is sorage for intermediate information about indices, used in the algorithm (see below)
            //Structure:
            //
            ulong[] upperInfo = new ulong[differentIndicesCount], lowerInfo = new ulong[differentIndicesCount];

            //This is for generalization of algorithm
            //indices[0] == lowerIndices
            //indices[1] == lowerIndices
            int[][] indices = { lowerIndices, upperIndices };

            //This is for generalization of algorithm too
            //info[0] == lowerInfo
            //info[1] == lowerInfo
            ulong[][] info = { lowerInfo, upperInfo };

            //Pointers for lower and upper indices, used in algorithm
            //pointer[0] - pointer to lower
            //pointer[1] - pointer to upper
            int[] pointer = new int[2];

            //Allocating array for results, one contraction for each tensor
            contractions = new long[data.Length][];
            //There is one dummy tensor with index -1, it represents fake
            //tensor contracting with whole Product to leave no contracting indices.
            //So, all "conractions" with this dummy "contraction" looks like a scalar
            //product. (sorry for English)
            freeContractions = new long[freeIndices.Size()];

            uint state;
            uint index;
            uint i;

            //Processing free indices = creating contractions for dummy tensor
            for (i = 0; i < freeIndices.Size(); ++i)
            {
                index = freeIndices[i];
                //Inverse state (because it is state of index at (??) dummy tensor,
                //contracted with this free index)
                state = 1 - IndicesUtils.getStateInt(index);

                //Important:
                info[state][pointer[state]] = dummyTensorInfo;
                indices[state][pointer[state]++] = IndicesUtils.GetNameWithType(index);
            }

            int tensorIndex;
            for (tensorIndex = 0; tensorIndex < data.Length; ++tensorIndex)
            {
                //Main algorithm
                IIndices tInds = data[tensorIndex].Indices;
                short[] diffIds = tInds.GetDiffIds();

                //FUTURE move to other place
                if (tInds.Size() >= 0x10000)
                    throw new InvalidOperationException("Too many indices!!! max count = 2^16");

                for (uint j = 0; j < tInds.Size(); ++j)
                {
                    index = tInds[j];
                    state = IndicesUtils.getStateInt(index);
                    info[state][pointer[state]] = packToLong(tensorIndex, diffIds[j], j);
                    indices[state][pointer[state]++] = IndicesUtils.GetNameWithType(index);
                }

                //Result allocation
                contractions[tensorIndex] = new long[tInds.Size()];
            }

            //Here we can use unstable sorting algorithm (all indices are different)
            ArraysUtils.quickSort(indices[0], info[0].Select(u => (int)u).ToArray());
            ArraysUtils.quickSort(indices[1], info[1].Select(u => (int)u).ToArray());

            //Calculating connected components
            var infoTensorIndicesFrom = infoToTensorIndices(lowerInfo);
            var infoTensorIndicesTo = infoToTensorIndices(upperInfo);

            uint shift = 0;
            uint last = 0;
            for (i = 0; i < infoTensorIndicesFrom.Length; ++i)
                if (infoTensorIndicesFrom[i] == -1 || infoTensorIndicesTo[i] == -1)
                {
                    Array.Copy(infoTensorIndicesFrom, last, infoTensorIndicesFrom, last - shift, i - last);
                    Array.Copy(infoTensorIndicesTo, last, infoTensorIndicesTo, last - shift, i - last);
                    last = i + 1;
                    ++shift;
                }

            Array.Copy(infoTensorIndicesFrom, last, infoTensorIndicesFrom, last - shift, i - last);
            Array.Copy(infoTensorIndicesTo, last, infoTensorIndicesTo, last - shift, i - last);
            infoTensorIndicesFrom = Arrays.copyOf(infoTensorIndicesFrom, (int)(infoTensorIndicesFrom.Length - shift));
            infoTensorIndicesTo = Arrays.copyOf(infoTensorIndicesTo, (int)(infoTensorIndicesTo.Length - shift));

            int[] components = GraphUtils.CalculateConnectedComponents(infoTensorIndicesFrom, infoTensorIndicesTo, data.Length);
            componentCount = components[components.Length - 1];
            this.components = Arrays.copyOfRange(components, 0, components.Length - 1);
            //<-- Here we have mature info arrays

            Debug.Assert(indices[0].SequenceEqual(indices[1]));

            int freePointer = 0;
            int indexIndex;
            for (i = 0; i < differentIndicesCount; ++i)
            {
                //Contractions from lower to upper
                tensorIndex = (int)(0xFFFFFFFFL & (info[0][i] >> 16)); //From tensor index
                indexIndex = (int)(0xFFFFL & (info[0][i] >> 48));
                ulong contraction = (0xFFFFFFFFFFFF0000L & (info[1][i] << 16))
                                    | (0xFFFFL & info[0][i]);
                if (tensorIndex == -1)
                    freeContractions[freePointer++] = (long)contraction;
                else
                    contractions[tensorIndex][indexIndex] = (long)contraction;

                //Contractions from upper to lower
                tensorIndex = (int)(0xFFFFFFFFL & (info[1][i] >> 16)); //From tensor index
                indexIndex = (int)(0xFFFFL & (info[1][i] >> 48));
                contraction = (0xFFFFFFFFFFFF0000L & (info[0][i] << 16))
                              | (0xFFFFL & info[1][i]);
                if (tensorIndex == -1)
                    freeContractions[freePointer++] = (long)contraction;
                else
                    contractions[tensorIndex][indexIndex] = (long)contraction;
            }
        }

        public static int getToTensorIndex(long contraction)
        {
            return (int)(contraction >> 32);
        }

        public static short getToIndexId(long contraction)
        {
            return (short)(0xFFFF & (contraction >> 16));
        }

        public static short getFromIndexId(long contraction)
        {
            return (short)(0xFFFF & contraction);
        }

        /**
         * Function to pack data to intermediate 64-bit record.
         *
         * @param tensorIndex index of tensor in the data array (before second
         *                    sorting)
         * @param id          id of index in tensor indices list (could be !=0 only
         *                    for simple tensors)
         * @param indexIndex  index of Index in Indices of tensor ( only 16 bits
         *                    used !!!!!!!!! )
         * @return packed record (long)
         */
        private static ulong packToLong(int tensorIndex, short id, uint indexIndex)
        {
            return (((ulong)tensorIndex) << 16) | (0xFFFFL & (ulong)id) | (((ulong)indexIndex) << 48);
        }

        //0xFFFFFFFF00000000L == packToLong(-1, (short) 0, -1);
        private const ulong dummyTensorInfo = 0xFFFFFFFFFFFF0000L;

        private static int[] infoToTensorIndices(ulong[] info)
        {
            int[] result = new int[info.Length];
            for (int i = 0; i < info.Length; ++i)
                result[i] = ((int)(0xFFFFFFFFL & (info[i] >> 16)));
            return result;
        }
    }
}