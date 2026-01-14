namespace NRedberry.Graphs;

internal sealed class BreadthFirstPointer
{
    public BreadthFirstPointer(int node, int edgePointer)
    {
        Vertex = node;
        EdgePointer = edgePointer;
    }

    public int Vertex { get; }
    public int EdgePointer { get; set; }
}
