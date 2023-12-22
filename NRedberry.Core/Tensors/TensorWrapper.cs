using System;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Tensors;

public sealed class TensorWrapper(Tensor tensor) : IComparable<TensorWrapper>
{
    private readonly Tensor tensor = tensor;
    private readonly int hashWithIndices = TensorHashCalculator.HashWithIndices(tensor);

    public int CompareTo(TensorWrapper? other)
    {
        if (other == null)
        {
            return -1;
        }

        var i = tensor.CompareTo(other.tensor);

        return i != 0
            ? i
            : hashWithIndices.CompareTo(other.hashWithIndices);
    }
}