using System.Collections;
using System.Diagnostics;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;
using static NRedberry.Core.Indices.IndicesUtils;

namespace NRedberry.Core.Graphs;

public sealed class PrimitiveSubgraphPartition
{
    private readonly ProductContent pc;
    private readonly StructureOfContractions fcs;
    private readonly int size;
    private readonly IndexType type;
    private readonly PrimitiveSubgraph[] partition;
    private readonly BitArray used;

    public PrimitiveSubgraphPartition(ProductContent productContent, IndexType type)
    {
        pc = productContent;
        fcs = pc.StructureOfContractions;
        size = pc.Size;
        this.type = type;
        used = new BitArray(size);
        partition = CalculatePartition();
    }

    public PrimitiveSubgraph[] Partition => (PrimitiveSubgraph[])partition.Clone();
    public static PrimitiveSubgraph[] CalculatePartition(Product p, IndexType type) => new PrimitiveSubgraphPartition(p.Content, type).Partition;
    public static PrimitiveSubgraph[] CalculatePartition(ProductContent p, IndexType type) => new PrimitiveSubgraphPartition(p, type).Partition;

    private PrimitiveSubgraph[] CalculatePartition()
    {
        List<PrimitiveSubgraph> subgraphs = [];
        for (int pivot = 0; pivot < size; ++pivot)
        {
            if (pc[pivot].Indices.Size(type) != 0 && !used.Get(pivot))
            {
                subgraphs.Add(CalculateComponent(pivot));
            }
        }
        return subgraphs.ToArray();
    }

    private PrimitiveSubgraph CalculateComponent(int pivot)
    {
        LinkedList<int> positions = new LinkedList<int>();
        positions.AddLast(pivot);

        int[] left, right;
        left = right = GetLinks(pivot);

        Debug.Assert(left[0] != NO_LINKS || left[1] != NO_LINKS);

        if (left[0] == BRANCHING || left[1] == BRANCHING)
            return ProcessGraph(pivot);

        if (left[0] == left[1] && left[0] == pivot) {
            used.Set(pivot, true);
            return new PrimitiveSubgraph(GraphType.Cycle, [pivot]);
        }

        long leftPivot, rightPivot, lastLeftPivot = NOT_INITIALIZED, lastRightPivot = NOT_INITIALIZED;

        while (left != DUMMY || right != DUMMY)
        {
            if (left[0] == BRANCHING || left[1] == BRANCHING || right[0] == BRANCHING || right[1] == BRANCHING)
                return ProcessGraph(pivot);

            leftPivot = left[0];
            rightPivot = right[1];

            Debug.Assert(leftPivot < 0 || !used.Get((int)leftPivot));
            Debug.Assert(rightPivot < 0 || !used.Get((int)rightPivot));

            //Left end detection
            if (leftPivot == NO_LINKS || leftPivot == -1)
                leftPivot = DUMMY_PIVOT;

            //Right end detection
            if (rightPivot == NO_LINKS || rightPivot == -1)
                rightPivot = DUMMY_PIVOT;

            //Odd cycle detection
            if (leftPivot >= 0 && leftPivot == lastRightPivot) {
                //Closing odd nodes number cycle
                Debug.Assert(rightPivot == lastLeftPivot);
                return new PrimitiveSubgraph(GraphType.Cycle, DequeToArray(positions));
            }

            //Adding left pivot before cycle detection (if cycle, not to add closing node twice)
            if (leftPivot >= 0)
                positions.AddFirst((int)leftPivot);

            //Even cycle detection
            if (leftPivot >= 0 && leftPivot == rightPivot) {
                left = GetLinks((int)leftPivot);

                // Checking next (cycle closing) node
                if (left[0] == BRANCHING || left[1] == BRANCHING)
                    return ProcessGraph(pivot);

                return new PrimitiveSubgraph(GraphType.Cycle, DequeToArray(positions));
            }

            //Adding right pivot
            if (rightPivot >= 0)
                positions.AddLast((int)rightPivot);

            //Needed in odd cycle detection
            lastLeftPivot = leftPivot;
            //Redundant (needed for assertion)
            lastRightPivot = rightPivot;

            //Next layer (breadth-first traversal)
            left = GetLinks((int)leftPivot);
            right = GetLinks((int)rightPivot);
        }

        return new PrimitiveSubgraph(GraphType.Line, DequeToArray(positions));
    }

    private int[] DequeToArray(LinkedList<int> deque)
    {
        int[] arr = new int[deque.Count];
        int i = -1;
        foreach (int ii in deque)
        {
            arr[++i] = ii;
            used.Set(ii, true);
        }
        return arr;
    }

    private const int BRANCHING = -3, NO_LINKS = -2, NOT_INITIALIZED = -4, DUMMY_PIVOT = -5;
    private static readonly int[] DUMMY = [DUMMY_PIVOT, DUMMY_PIVOT];

    private int[] GetLinks(int pivot)
    {
        if (pivot == DUMMY_PIVOT)
            return DUMMY;

        Debug.Assert(pivot >= 0);

        int[] links = [NOT_INITIALIZED, NOT_INITIALIZED];
        int[] contractions = fcs.contractions[pivot];
        Indices.Indices indices = pc[pivot].Indices;
        int index, toTensorIndex;
        for (int i = contractions.Length - 1; i >= 0; --i)
        {
            index = indices[i];

            if (GetType_(index) != type.GetType_())
                continue;

            toTensorIndex = StructureOfContractions.ToPosition(contractions[i]);
            long state = 1 - GetStateInt(index);

            if (links[state] >= -1 && links[state] != toTensorIndex)
                links[state] = BRANCHING;
            if (links[state] == NOT_INITIALIZED)
                links[state] = toTensorIndex;
        }

        if (links[0] == NOT_INITIALIZED)
            links[0] = NO_LINKS;

        if (links[1] == NOT_INITIALIZED)
            links[1] = NO_LINKS;

        return links;
    }

    private PrimitiveSubgraph ProcessGraph(int pivot)
    {
        List<int> positions =
        [
            pivot
        ];

        Stack<int> stack = new Stack<int>();
        stack.Push(pivot);
        used.Set(pivot, true);

        int[] contractions;
        Indices.Indices indices;

        int currentPivot, index, toTensorIndex;
        while (stack.Count > 0)
        {
            currentPivot = stack.Pop();

            indices = pc[currentPivot].Indices;
            contractions = fcs.contractions[currentPivot];
            for (int i = contractions.Length - 1; i >= 0; --i)
            {
                index = indices[i];
                if (GetType_(index) != type.GetType_())
                    continue;

                toTensorIndex = StructureOfContractions.ToPosition(contractions[i]);
                if (toTensorIndex == -1 || used.Get(toTensorIndex))
                    continue;
                used.Set(toTensorIndex, true);
                positions.Add(toTensorIndex);
                stack.Push(toTensorIndex);
            }
        }
        return new PrimitiveSubgraph(GraphType.Graph, positions.ToArray());
    }
}