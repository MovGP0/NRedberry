using System;
using System.Linq;
using System.Text;

namespace NRedberry.Core.Tensors
{
    public sealed class TensorContraction : IComparable<TensorContraction>
    {
        public short TensorId { get; }
        public long[] IndexContractions { get; private set; }
        private int _hash = -1;

        public TensorContraction(short tensorId, long[] indexContractions)
        {
            TensorId = tensorId;
            IndexContractions = indexContractions;
        }

        public void SortContractions()
        {
            IndexContractions = IndexContractions.OrderBy(c => c).ToArray();
        }

        public bool ContainsFreeIndex()
        {
            return IndexContractions.Any(contradiction => GetToTensorId(contradiction) == -1);
        }

        public int CompareTo(TensorContraction other)
        {
            var idDifference = TensorId.CompareTo(other.TensorId);
            if (idDifference != 0)
            {
                return idDifference;
            }

            var lengthDifference = IndexContractions.Length.CompareTo(other.IndexContractions.Length);
            if (lengthDifference != 0)
            {
                return lengthDifference;
            }

            return IndexContractions.Select((t, i) => t.CompareTo(other.IndexContractions[i])).FirstOrDefault(v => v != 0);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (IndexContractions.Length == 0)
                return builder.Append(TensorId).Append("x").ToString();

            builder.Append(TensorId).Append("x{");
            foreach (var l in IndexContractions)
            {
                builder.Append("^").Append(GetFromIndexId(l)).Append("->").Append(GetToTensorId(l)).Append("^").Append(GetToIndexId(l));
                builder.Append(":");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append("}");
            return builder.ToString();
        }

        public static short GetFromIndexId(long contraction)
        {
            return (short)((contraction >> 32) & 0xFFFF);
        }

        public static short GetToIndexId(long contraction)
        {
            return (short)(contraction & 0xFFFF);
        }

        public static short GetToTensorId(long contraction)
        {
            return (short)((contraction >> 16) & 0xFFFF);
        }
    }
}