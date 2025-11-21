using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

public class TransformationException : TensorException
{
    public TransformationException(string message, params Tensor[] tensors)
        : base(message, tensors)
    {
    }

    public TransformationException(string message)
        : base(message)
    {
    }

    public TransformationException(params Tensor[] tensor)
        : base(tensor)
    {
    }
}
