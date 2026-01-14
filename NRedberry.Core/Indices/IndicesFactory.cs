namespace NRedberry.Indices;

public static class IndicesFactory
{
    public static readonly Indices EmptyIndices = NRedberry.Indices.EmptyIndices.EmptyIndicesInstance;

    public static readonly SimpleIndices EmptySimpleIndices = NRedberry.Indices.EmptySimpleIndices.emptySimpleIndicesInstance;

    public static SimpleIndices CreateSimple(IndicesSymmetries? symmetries, params int[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        if (data.Length == 0)
        {
            return NRedberry.Indices.EmptySimpleIndices.emptySimpleIndicesInstance;
        }

        return new SimpleIndicesIsolated((int[])data.Clone(), symmetries);
    }

    public static SimpleIndices CreateSimple(IndicesSymmetries? symmetries, Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        if (indices.Size() == 0)
        {
            return NRedberry.Indices.EmptySimpleIndices.emptySimpleIndicesInstance;
        }

        if (indices is SimpleIndicesAbstract simpleIndicesAbstract)
        {
            return new SimpleIndicesIsolated(simpleIndicesAbstract.Data, symmetries);
        }

        return new SimpleIndicesIsolated(indices.AllIndices.ToArray(), symmetries);
    }

    public static Indices Create(Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        if (indices.Size() == 0)
        {
            return EmptyIndices;
        }

        if (indices is SortedIndices)
        {
            return indices;
        }

        return new SortedIndices(indices.AllIndices.ToArray());
    }

    public static Indices Create(params int[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        if (data.Length == 0)
        {
            return EmptyIndices;
        }

        return new SortedIndices((int[])data.Clone());
    }
}
