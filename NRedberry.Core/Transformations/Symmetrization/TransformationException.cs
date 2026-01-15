using System.Runtime.Serialization;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

public class TransformationException : TensorException
{
    public TransformationException()
    {
    }

    public TransformationException(string message, params Tensor[] tensors)
        : base(message, tensors)
    {
    }

    public TransformationException(string message)
        : base(message)
    {
    }

    public TransformationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected TransformationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public TransformationException(params Tensor[] tensor)
        : base(tensor)
    {
    }
}
