using System;
using static NRedberry.Core.Indices.EmptyIndices;

namespace NRedberry.Core.Indices;

public static class IndicesFactory
{
    public static IIndices EmptyIndices = EmptyIndicesInstance;

    public static ISimpleIndices createSimple(IndicesSymmetries symmetries, params uint[] data)
    {
        throw new NotImplementedException();
    }

    public static ISimpleIndices createSimple(IndicesSymmetries symmetries, IIndices indices)
    {
        throw new NotImplementedException();
    }

    public static IIndices create(IIndices indices)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates unordered indices from specified integer array of indices.
    /// </summary>
    /// <param name="data"></param>
    /// <returns>unordered indices from specified integer array of indices</returns>
    public static IIndices create(params uint[] data)
    {
        throw new NotImplementedException();
    }
}