using NRedberry.Tensors;

namespace NRedberry.Indices;

public sealed class InconsistentIndicesException : TensorException
{
    public InconsistentIndicesException()
        : base("Inconsistent indices")
    {
    }

    public InconsistentIndicesException(int index)
        : base($"Inconsistent index {IndicesUtils.ToString(index)}.")
    {
        Index = index;
    }

    public InconsistentIndicesException(InconsistentIndicesException cause, Tensor inTensor)
        : this(GetIndexOrThrow(cause), GetTensorOrThrow(inTensor))
    {
    }

    public InconsistentIndicesException(int? index, Tensor inTensor)
        : base($"Inconsistent index {IndicesUtils.ToString(index ?? -1)}", GetTensorOrThrow(inTensor))
    {
        Index = index;
    }

    public InconsistentIndicesException(Tensor inTensor)
        : base("Inconsistent indices", GetTensorOrThrow(inTensor))
    {
        Index = null;
    }

    public int? Index { get; }

    public InconsistentIndicesException(string message, params Tensor[] tensors)
        : base(message, tensors)
    {
    }

    public InconsistentIndicesException(string message)
        : base(message)
    {
    }

    public InconsistentIndicesException(params Tensor[] tensor)
        : base(tensor)
    {
    }

    private static int? GetIndexOrThrow(InconsistentIndicesException cause)
    {
        ArgumentNullException.ThrowIfNull(cause);
        return cause.Index;
    }

    private static Tensor GetTensorOrThrow(Tensor inTensor)
    {
        ArgumentNullException.ThrowIfNull(inTensor);
        return inTensor;
    }
}
