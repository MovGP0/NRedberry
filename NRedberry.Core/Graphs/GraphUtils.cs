using System.Diagnostics;
using NRedberry.Core.Utils;

namespace NRedberry.Graphs;

public static class GraphUtils
{
    public static int[] CalculateConnectedComponents(int[] _from, int[] _to, int vertices)
    {
        //Test for parameters consistence
        if (_from.Length != _to.Length)
        {
            throw new ArgumentException();
        }

        //No edges case
        if (_from.Length == 0)
        {
            var result = new int[vertices + 1];
            for (var i = 0; i < vertices + 1; ++i)
            {
                result[i] = i;
            }

            return result;
        }

        //Creating sorted union
        var from = new int[_from.Length << 1];
        var to = new int[_from.Length << 1];
        Array.Copy(_from, 0, from, 0, _from.Length);
        Array.Copy(_to, 0, to, 0, _from.Length);
        Array.Copy(_from, 0, to, _from.Length, _from.Length);
        Array.Copy(_to, 0, from, _from.Length, _from.Length);

        //Sorting to easy indexing by from
        ArraysUtils.QuickSort(from, to);

        //Test for parameters consistence
        if (from[0] < 0 || from[from.Length - 1] > vertices)
        {
            throw new ArgumentException();
        }

        //Creating index for fast search in from array
        var fromIndex = new int[vertices];
        Arrays.fill(fromIndex, -1); //-1 in fromIndex means absence of edges for certain vertex
        var lastVertex = -1;
        for (var i = 0; i < from.Length; ++i)
        {
            if (lastVertex != from[i])
            {
                fromIndex[lastVertex = from[i]] = i;
            }
        }

        //Allocation resulting array
        var components = new int[vertices + 1];
        Arrays.fill(components, -1); //There will be no -1 at the end

        var currentComponent = -1;
        var m1 = 0;
        var stack = new Stack<BreadthFirstPointer>();
        do
        {
            ++currentComponent;
            components[m1] = currentComponent;

            if (fromIndex[m1] == -1)
            {
                //There is no edges for curreent vertex,
                //so it is connected component by it self
                continue;
            }

            //Pushing seed vertex to stack
            stack.Push(new BreadthFirstPointer(m1, fromIndex[m1]));

            //Main algorithm (simple depth-first search)
            while (stack.Count == 0)
            {
                var pointer = stack.Peek();

                if (pointer.EdgePointer >= from.Length || from[pointer.EdgePointer++] != pointer.Vertex)
                {
                    //There are no more edges from this vertex => delete it from stack and continue
                    stack.Pop();
                    continue;
                }

                // -1 because pointer.edgePointer++ was invoked
                var pointsTo = to[pointer.EdgePointer - 1];

                if (components[pointsTo] == currentComponent)
                {
                    //We've been here earlier, continue
                    continue;
                }

                Debug.Assert(components[pointsTo] == -1);

                //Marking current vertex by current connected component index
                components[pointsTo] = currentComponent;

                if (fromIndex[pointsTo] != -1)
                {
                    //No edges from this vertex
                    stack.Push(new BreadthFirstPointer(pointsTo, fromIndex[pointsTo]));
                }
            }
        } while ((m1 = FirstM1(components)) != vertices);

        //writing components count
        components[vertices] = currentComponent + 1;
        return components;
    }

    private static int FirstM1(IReadOnlyList<int> arr)
    {
        for (var i = 0; i < arr.Count; ++i)
        {
            if (arr[i] == -1)
            {
                return i;
            }
        }

        return -1;
    }

    private sealed class BreadthFirstPointer(int node, int edgePointer)
    {
        public int Vertex { get; } = node;
        public int EdgePointer { get; set; } = edgePointer;
    }

    public static int ComponentSize(int vertex, int[] components)
    {
        if (vertex > components.Length - 1)
        {
            throw new IndexOutOfRangeException();
        }

        var componentCount = components[vertex];

        return components.Count(t => t == componentCount);
    }
}
