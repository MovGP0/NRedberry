namespace NRedberry.Core.Indices;

public static class IndicesFactory
{
    public static readonly Indices EmptyIndices = NRedberry.Core.Indices.EmptyIndices.EmptyIndicesInstance;

    public static readonly SimpleIndices emptySimpleIndices = EmptySimpleIndices.emptySimpleIndicesInstance;

    public static SimpleIndices CreateSimple(IndicesSymmetries? symmetries, params int[] data)
    {
        if (data.Length == 0)
            return EmptySimpleIndices.emptySimpleIndicesInstance;
        return new SimpleIndicesIsolated((int[])data.Clone(), symmetries);
    }

    public static SimpleIndices CreateSimple(IndicesSymmetries? symmetries, Indices indices)
    {
        if (indices.Size() == 0)
            return EmptySimpleIndices.emptySimpleIndicesInstance;
        if (indices is SimpleIndicesAbstract simpleIndicesAbstract)
            return new SimpleIndicesIsolated(simpleIndicesAbstract.Data, symmetries);
        return new SimpleIndicesIsolated(indices.GetAllIndices().Copy(), symmetries);
    }

    public static Indices Create(Indices indices)
    {
        if (indices.Size() == 0)
            return EmptyIndices;
        if (indices is SortedIndices)
            return indices;
        return new SortedIndices(indices.AllIndices.Copy());
    }

    public static Indices Create(params int[] data)
    {
        if (data.Length == 0)
            return EmptyIndices;
        return new SortedIndices((int[])data.Clone());
    }
}
