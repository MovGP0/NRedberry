using NRedberry.Core.Tensors;

namespace NRedberry.Core.Indices;

public sealed class InconsistentIndicesException: TensorException
{
    public InconsistentIndicesException(int index)
        : base($"Inconsistent index {IndicesUtils.ToString(index)}.")
        => Index = index;

    public InconsistentIndicesException(InconsistentIndicesException cause, Tensor inTensor)
        : this(cause.Index, inTensor) { }

    public InconsistentIndicesException(int? index, Tensor inTensor)
        : base($"Inconsistent index {IndicesUtils.ToString(index ?? -1)}", inTensor)
        => Index = index;

    public InconsistentIndicesException(Tensor inTensor)
        : base("Inconsistent indices", inTensor)
        => Index = null;

    public int? Index { get; }

    public InconsistentIndicesException(string message, params Tensor[] tensors) : base(message, tensors)
    {
    }

    public InconsistentIndicesException(string message) : base(message)
    {
    }

    public InconsistentIndicesException(params Tensor[] tensor) : base(tensor)
    {
    }
}