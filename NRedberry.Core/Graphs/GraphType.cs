namespace NRedberry.Core.Graphs;

/// <summary>
/// Some basic types of graphs that are used to interpret matrices products.
/// </summary>
public enum GraphType
{
    /// <summary>
    /// The graph of form A^{i<sub>1</sub>}_{i<sub>2</sub>}*B^{i<sub>2</sub>}_{i<sub>3</sub>}*...*C^{i<sub>N</sub>}_{i<sub>1</sub>},
    /// where {i<sub>j</sub>} denotes the whole set of tensor indices. Tensors of such structure have no free indices of considered
    /// index type.
    /// </summary>
    Cycle,
    /// <summary>
    /// The graph of form A^{i<sub>1</sub>}_{i<sub>2</sub>}*B^{i<sub>2</sub>}_{i<sub>3</sub>}*...*C^{i<sub>N-1</sub>}_{i<sub>N</sub>},
    /// where {i<sub>j</sub>} denotes the whole set of tensor indices.
    /// </summary>
    Line,
    /// <summary>
    /// Not cycle or line
    /// </summary>
    Graph
}
