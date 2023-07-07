namespace NRedberry.Core.Indices;

public static class IndicesFactory
{
    public static readonly IIndices EmptyIndices = NRedberry.Core.Indices.EmptyIndices.EmptyIndicesInstance;

    public static readonly ISimpleIndices EmptySimpleIndices = EmptySimpleIndices.EmptySimpleIndicesInstance;

    public static ISimpleIndices CreateSimple(IndicesSymmetries? symmetries, params long[] data)
    {
        if (data.Length == 0)
            return EmptySimpleIndices.EmptySimpleIndicesInstance;
        return new SimpleIndicesIsolated((long[])data.Clone(), symmetries);
    }

    public static ISimpleIndices CreateSimple(IndicesSymmetries? symmetries, IIndices indices)
    {
        if (indices.Size() == 0)
            return EmptySimpleIndices.EmptySimpleIndicesInstance;
        if (indices is SimpleIndicesAbstract simpleIndicesAbstract)
            return new SimpleIndicesIsolated(simpleIndicesAbstract.data, symmetries);
        return new SimpleIndicesIsolated(indices.GetAllIndices().Copy(), symmetries);
    }

    public static IIndices Create(IIndices indices)
    {
        if (indices.Size() == 0)
            return EmptyIndices;
        if (indices is SortedIndices)
            return indices;
        return new SortedIndices(indices.GetAllIndices().Copy());
    }

    public static IIndices Create(params long[] data)
    {
        if (data.Length == 0)
            return EmptyIndices;
        return new SortedIndices((long[])data.Clone());
    }
}