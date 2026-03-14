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
        Tensor[] data = productContent.GetDataCopy();
        PrimitiveSubgraph[] subgraphs = CalculatePartition(data, type);
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
                Tensor renamedLeft = SetIndices(left, left.Indices, RenameOfType(left.Indices, leftSubIndices, rightSubIndices));
                Tensor renamedRight = SetIndices(right, right.Indices, RenameOfType(right.Indices, rightSubIndices, leftSubIndices));
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

    private static PrimitiveSubgraph[] CalculatePartition(Tensor[] data, IndexType type)
    {
        bool[] used = new bool[data.Length];
        HashSet<int>[] adjacency = CreateAdjacency(data.Length);
        int[][] links = BuildLinks(data, type, adjacency);
        List<PrimitiveSubgraph> subgraphs = [];

        for (int pivot = 0; pivot < data.Length; ++pivot)
        {
            if (data[pivot].Indices.Size(type) != 0 && !used[pivot])
            {
                subgraphs.Add(CalculateComponent(pivot, links, adjacency, used));
            }
        }

        return subgraphs.ToArray();
    }

    private static HashSet<int>[] CreateAdjacency(int size)
    {
        HashSet<int>[] adjacency = new HashSet<int>[size];
        for (int i = 0; i < size; ++i)
        {
            adjacency[i] = [];
        }

        return adjacency;
    }

    private static int[][] BuildLinks(Tensor[] data, IndexType type, HashSet<int>[] adjacency)
    {
        Dictionary<int, HashSet<int>> upperOwners = [];
        Dictionary<int, HashSet<int>> lowerOwners = [];

        for (int i = 0; i < data.Length; ++i)
        {
            if (data[i].Indices.Size(type) == 0)
            {
                continue;
            }

            Indices.Indices subIndices = data[i].Indices.GetOfType(type);
            for (int j = 0; j < subIndices.Size(); ++j)
            {
                int index = subIndices[j];
                int name = IndicesUtils.GetNameWithType(index);
                Dictionary<int, HashSet<int>> owners = IndicesUtils.GetState(index) ? upperOwners : lowerOwners;
                RegisterOwner(owners, name, i);
            }
        }

        BuildAdjacency(upperOwners, lowerOwners, adjacency);

        int[][] links = new int[data.Length][];
        for (int i = 0; i < data.Length; ++i)
        {
            links[i] = data[i].Indices.Size(type) == 0
                ? [NO_LINKS, NO_LINKS]
                : BuildLinksForTensor(data[i].Indices.GetOfType(type), upperOwners, lowerOwners);
        }

        return links;
    }

    private static void RegisterOwner(Dictionary<int, HashSet<int>> owners, int name, int owner)
    {
        if (!owners.TryGetValue(name, out HashSet<int>? positions))
        {
            positions = [];
            owners[name] = positions;
        }

        positions.Add(owner);
    }

    private static void BuildAdjacency(
        Dictionary<int, HashSet<int>> upperOwners,
        Dictionary<int, HashSet<int>> lowerOwners,
        HashSet<int>[] adjacency)
    {
        foreach ((int name, HashSet<int> uppers) in upperOwners)
        {
            if (!lowerOwners.TryGetValue(name, out HashSet<int>? lowers))
            {
                continue;
            }

            foreach (int upper in uppers)
            {
                foreach (int lower in lowers)
                {
                    if (upper == lower)
                    {
                        continue;
                    }

                    adjacency[upper].Add(lower);
                    adjacency[lower].Add(upper);
                }
            }
        }
    }

    private static int[] BuildLinksForTensor(
        Indices.Indices subIndices,
        Dictionary<int, HashSet<int>> upperOwners,
        Dictionary<int, HashSet<int>> lowerOwners)
    {
        int[] links = [NOT_INITIALIZED, NOT_INITIALIZED];

        for (int i = 0; i < subIndices.Size(); ++i)
        {
            int index = subIndices[i];
            int slot = 1 - IndicesUtils.GetStateInt(index);
            int name = IndicesUtils.GetNameWithType(index);
            int toTensorIndex = ResolveOwner(
                name,
                IndicesUtils.GetState(index) ? lowerOwners : upperOwners);

            if (links[slot] >= -1 && links[slot] != toTensorIndex)
            {
                links[slot] = BRANCHING;
            }
            else if (links[slot] == NOT_INITIALIZED)
            {
                links[slot] = toTensorIndex;
            }
        }

        if (links[0] == NOT_INITIALIZED)
        {
            links[0] = NO_LINKS;
        }

        if (links[1] == NOT_INITIALIZED)
        {
            links[1] = NO_LINKS;
        }

        return links;
    }

    private static int ResolveOwner(int name, Dictionary<int, HashSet<int>> owners)
    {
        if (!owners.TryGetValue(name, out HashSet<int>? positions) || positions.Count == 0)
        {
            return -1;
        }

        if (positions.Count == 1)
        {
            foreach (int position in positions)
            {
                return position;
            }
        }

        return BRANCHING;
    }

    private static PrimitiveSubgraph CalculateComponent(
        int pivot,
        int[][] links,
        HashSet<int>[] adjacency,
        bool[] used)
    {
        LinkedList<int> positions = new();
        positions.AddLast(pivot);

        int[] left = GetLinks(pivot, links);
        int[] right = left;

        if (left[0] == BRANCHING || left[1] == BRANCHING)
        {
            return ProcessGraph(pivot, adjacency, used);
        }

        if (left[0] == left[1] && left[0] == pivot)
        {
            used[pivot] = true;
            return new PrimitiveSubgraph(GraphType.Cycle, [pivot]);
        }

        int leftPivot;
        int rightPivot;
        int lastLeftPivot = NOT_INITIALIZED;
        int lastRightPivot = NOT_INITIALIZED;

        while (!ReferenceEquals(left, DUMMY) || !ReferenceEquals(right, DUMMY))
        {
            if (left[0] == BRANCHING || left[1] == BRANCHING || right[0] == BRANCHING || right[1] == BRANCHING)
            {
                return ProcessGraph(pivot, adjacency, used);
            }

            leftPivot = left[0];
            rightPivot = right[1];

            if (leftPivot == NO_LINKS || leftPivot == -1)
            {
                leftPivot = DUMMY_PIVOT;
            }

            if (rightPivot == NO_LINKS || rightPivot == -1)
            {
                rightPivot = DUMMY_PIVOT;
            }

            if (leftPivot >= 0 && leftPivot == lastRightPivot)
            {
                return new PrimitiveSubgraph(GraphType.Cycle, ToArrayAndMarkUsed(positions, used));
            }

            if (leftPivot >= 0)
            {
                positions.AddFirst(leftPivot);
            }

            if (leftPivot >= 0 && leftPivot == rightPivot)
            {
                left = GetLinks(leftPivot, links);
                if (left[0] == BRANCHING || left[1] == BRANCHING)
                {
                    return ProcessGraph(pivot, adjacency, used);
                }

                return new PrimitiveSubgraph(GraphType.Cycle, ToArrayAndMarkUsed(positions, used));
            }

            if (rightPivot >= 0)
            {
                positions.AddLast(rightPivot);
            }

            lastLeftPivot = leftPivot;
            lastRightPivot = rightPivot;

            left = GetLinks(leftPivot, links);
            right = GetLinks(rightPivot, links);
        }

        return new PrimitiveSubgraph(GraphType.Line, ToArrayAndMarkUsed(positions, used));
    }

    private static int[] GetLinks(int pivot, int[][] links)
    {
        return pivot == DUMMY_PIVOT ? DUMMY : links[pivot];
    }

    private static PrimitiveSubgraph ProcessGraph(int pivot, HashSet<int>[] adjacency, bool[] used)
    {
        List<int> positions =
        [
            pivot
        ];
        Stack<int> stack = new();
        stack.Push(pivot);
        used[pivot] = true;

        while (stack.Count > 0)
        {
            int current = stack.Pop();
            foreach (int neighbour in adjacency[current])
            {
                if (used[neighbour])
                {
                    continue;
                }

                used[neighbour] = true;
                positions.Add(neighbour);
                stack.Push(neighbour);
            }
        }

        return new PrimitiveSubgraph(GraphType.Graph, positions.ToArray());
    }

    private static int[] ToArrayAndMarkUsed(LinkedList<int> positions, bool[] used)
    {
        int[] result = new int[positions.Count];
        int pointer = 0;
        foreach (int position in positions)
        {
            result[pointer++] = position;
            used[position] = true;
        }

        return result;
    }

    private const int BRANCHING = -3;
    private const int NO_LINKS = -2;
    private const int NOT_INITIALIZED = -4;
    private const int DUMMY_PIVOT = -5;

    private static readonly int[] DUMMY = [DUMMY_PIVOT, DUMMY_PIVOT];

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

    private static Tensor SetIndices(Tensor tensor, Indices.Indices from, Indices.Indices to)
    {
        if (from.Equals(to))
        {
            return tensor;
        }

        return ApplyIndexMapping.Apply(
            tensor,
            new Mapping(from.AllIndices.ToArray(), to.AllIndices.ToArray()),
            []);
    }

    private static Indices.Indices RenameOfType(
        Indices.Indices indices,
        Indices.Indices fromSubIndices,
        Indices.Indices toSubIndices)
    {
        return indices.ApplyIndexMapping(new ExactIndexMapping(fromSubIndices, toSubIndices));
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
