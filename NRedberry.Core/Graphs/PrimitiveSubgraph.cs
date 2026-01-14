namespace NRedberry.Graphs;

public sealed class PrimitiveSubgraph
{
    private readonly int[] _partition;

    public PrimitiveSubgraph(GraphType graphType, int[] partition)
    {
        ArgumentNullException.ThrowIfNull(partition);
        GraphType = graphType;
        _partition = partition;
    }

    public GraphType GraphType { get; }

    public int[] Partition => (int[])_partition.Clone();

    public int GetPosition(int i) => _partition[i];

    public int Size => _partition.Length;

    public override string ToString() => $"{GraphType}: [{string.Join(", ", _partition)}]";
}
