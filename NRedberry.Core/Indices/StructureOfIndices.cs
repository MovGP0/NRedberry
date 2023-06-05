namespace NRedberry.Core.Indices;

/// <summary>
/// The unique identification information about indices objects. This class contains
/// information about types of indices(number of indices of each type) and about
/// states of non metric indices(if there are any).
/// </summary>
public sealed class StructureOfIndices
{
    private ISimpleIndices indices;

    public StructureOfIndices(ISimpleIndices indices)
    {
        this.indices = indices;
    }
}