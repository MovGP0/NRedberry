using NRedberry.Graphs;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Reverse;

public sealed class SingleReverse : ITransformation
{
    public IndexType Type { get; }

    public SingleReverse(IndexType type)
    {
        if (CC.IsMetric(type))
        {
            throw new ArgumentException("Type should be non-metric.", nameof(type));
        }

        Type = type;
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return InverseOrderOfMatrices(tensor, Type);
    }

    public static Tensor InverseOrderOfMatrices(Tensor tensor, IndexType type)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (CC.IsMetric(type))
        {
            throw new ArgumentException("Type should be non-metric.", nameof(type));
        }

        return InverseOrderOfMatricesInternal(tensor, type);
    }

    private static Tensor InverseOrderOfMatricesInternal(Tensor tensor, IndexType type)
    {
        FromChildToParentIterator iterator = new(tensor);
        Tensor current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Product product)
            {
                iterator.Set(InverseOrderInProduct(product, type));
            }
        }

        return iterator.Result();
    }

    private static bool IsMatrix(Tensor tensor, IndexType type)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        Indices.Indices subIndices = tensor.Indices.GetOfType(type);
        return subIndices.UpperIndices.Length != 0
            && subIndices.UpperIndices.Length == subIndices.LowerIndices.Length;
    }

    private static Tensor InverseOrderInProduct(Product product, IndexType type)
    {
        ProductContent productContent = product.Content;
        PrimitiveSubgraph[] subgraphs = PrimitiveSubgraphPartition.CalculatePartition(productContent, type);
        Tensor[] data = productContent.GetDataCopy();
        bool somethingDone = false;

        foreach (PrimitiveSubgraph subgraph in subgraphs)
        {
            if (subgraph.GraphType == GraphType.Graph)
            {
                throw new ArgumentException("Not a product of matrices.", nameof(product));
            }

            if (subgraph.GraphType != GraphType.Line)
            {
                continue;
            }

            int[] partition = subgraph.Partition;
            Tensor left = null!;
            Tensor right = null!;
            bool leftSkip = false;
            bool rightSkip = false;
            bool leftMatrix = false;
            bool rightMatrix = false;
            Indices.Indices leftSubIndices = null!;
            Indices.Indices rightSubIndices = null!;
            int matrixIndexCount = -1;

            for (int leftPointer = 0, rightPointer = partition.Length - 1;
                 leftPointer < rightPointer;
                 ++leftPointer, --rightPointer)
            {
                if (!leftSkip)
                {
                    left = data[partition[leftPointer]];
                    leftSubIndices = left.Indices.GetOfType(type);
                    leftMatrix = TryClassifyMatrix(leftSubIndices, ref matrixIndexCount);
                }

                if (!rightSkip)
                {
                    right = data[partition[rightPointer]];
                    rightSubIndices = right.Indices.GetOfType(type);
                    rightMatrix = TryClassifyMatrix(rightSubIndices, ref matrixIndexCount);
                }

                leftSkip = false;
                rightSkip = false;

                if (!leftMatrix && !rightMatrix)
                {
                    continue;
                }

                if (leftMatrix && !rightMatrix)
                {
                    leftSkip = true;
                    --leftPointer;
                    continue;
                }

                if (!leftMatrix && rightMatrix)
                {
                    rightSkip = true;
                    ++rightPointer;
                    continue;
                }

                somethingDone = true;
                Tensor renamedLeft = SetIndices(left, leftSubIndices, rightSubIndices);
                Tensor renamedRight = SetIndices(right, rightSubIndices, leftSubIndices);
                data[partition[leftPointer]] = renamedRight;
                data[partition[rightPointer]] = renamedLeft;
            }
        }

        if (!somethingDone)
        {
            return product;
        }

        return TensorApi.Multiply(GetIndexlessSubProduct(product), TensorApi.Multiply(data));
    }

    private static bool TryClassifyMatrix(Indices.Indices subIndices, ref int matrixIndexCount)
    {
        int upperCount = subIndices.UpperIndices.Length;
        int lowerCount = subIndices.LowerIndices.Length;

        if (upperCount != lowerCount)
        {
            if (upperCount != 0 && lowerCount != 0)
            {
                throw new ArgumentException("Not a product of matrices.");
            }

            return false;
        }

        if (matrixIndexCount == -1)
        {
            matrixIndexCount = upperCount;
        }
        else if (matrixIndexCount != upperCount)
        {
            throw new ArgumentException("Not a product of matrices.");
        }

        return true;
    }

    private static Tensor SetIndices(Tensor tensor, Indices.Indices fromSubIndices, Indices.Indices toSubIndices)
    {
        ExactIndexMapping indexMapping = new(fromSubIndices, toSubIndices);
        Indices.Indices freeIndices = tensor.Indices.GetFree();
        Indices.Indices newFreeIndices = freeIndices.ApplyIndexMapping(indexMapping);
        if (freeIndices.Equals(newFreeIndices))
        {
            return tensor;
        }

        return ApplyIndexMapping.Apply(
            tensor,
            new Mapping(freeIndices.AllIndices.ToArray(), newFreeIndices.AllIndices.ToArray()));
    }

    private static Tensor GetIndexlessSubProduct(Product product)
    {
        if (product.IndexlessData.Length == 0)
        {
            return product.Factor;
        }

        if (product.Factor == NRedberry.Numbers.Complex.One && product.IndexlessData.Length == 1)
        {
            return product.IndexlessData[0];
        }

        if (product.Factor == NRedberry.Numbers.Complex.One)
        {
            return TensorApi.Multiply(product.IndexlessData);
        }

        Tensor[] factorsWithScalar = new Tensor[product.IndexlessData.Length + 1];
        factorsWithScalar[0] = product.Factor;
        Array.Copy(product.IndexlessData, 0, factorsWithScalar, 1, product.IndexlessData.Length);
        return TensorApi.Multiply(factorsWithScalar);
    }

    private sealed class ExactIndexMapping : IIndexMapping
    {
        private readonly int[] _from;
        private readonly int[] _to;

        public ExactIndexMapping(Indices.Indices fromSubIndices, Indices.Indices toSubIndices)
        {
            ArgumentNullException.ThrowIfNull(fromSubIndices);
            ArgumentNullException.ThrowIfNull(toSubIndices);

            _from = fromSubIndices.AllIndices.ToArray();
            _to = toSubIndices.AllIndices.ToArray();
            Array.Sort(_from, _to);
        }

        public int Map(int from)
        {
            int position = Array.BinarySearch(_from, from);
            return position >= 0 ? _to[position] : from;
        }
    }
}
