namespace NRedberry.Graphs;

internal sealed class BreadthFirstPointer(int node, int edgePointer)
{
    public int Vertex { get; } = node;
    public int EdgePointer { get; set; } = edgePointer;
}
