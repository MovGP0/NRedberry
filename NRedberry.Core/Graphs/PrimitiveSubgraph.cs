namespace NRedberry.Graphs;

public class PrimitiveSubgraph
{
    public PrimitiveSubgraph(GraphType graphType, int[] partition)
    {
        GraphType = graphType;
        Partition = (int[])partition.Clone();
    }

    public GraphType GraphType { get; }

    private int[] partition;

    public int[] Partition
    {
        get => (int[])partition.Clone();
        private set => partition = value;
    }

    public int GetPosition(int i) => Partition[i];

    public int Size => Partition.Length;
    public int Length => Partition.Length;

    public override string ToString() => $"{GraphType}: [{string.Join(", ", Partition)}]";
}
