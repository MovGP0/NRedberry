using System;

namespace NRedberry.Core.Indices;

public sealed class SimpleIndicesOfTensor : SimpleIndicesAbstract
{
    public SimpleIndicesOfTensor(int[] data, IndicesSymmetries symmetries)
        : base(data, symmetries)
    {
    }

    public SimpleIndicesOfTensor(bool notResort, int[] data, IndicesSymmetries symmetries)
        : base(notResort, data, symmetries)
    {
    }

    public SimpleIndices Create(int[] data, IndicesSymmetries symmetries)
    {
        return new SimpleIndicesOfTensor(true, data, symmetries);
    }
}