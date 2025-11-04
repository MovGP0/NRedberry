namespace NRedberry.Core.Tensors;

public class TensorException : Exception
{
    public TensorException(string message, params Tensor[] tensors)
        : base($"\"{message}\" in tensors {ArrayToString(tensors)}")
    {
    }

    public TensorException(string message, long nmseed, params Tensor[] tensors)
        : base($"\"{message}\" in tensors {ArrayToString(tensors)}\n nmseed: {nmseed}")
    {
    }

    public TensorException(string message)
        : base(message)
    {
    }

    public TensorException(string message, long nmseed)
        : base($"{message}\n nmseed: {nmseed}")
    {
    }

    public TensorException(params Tensor[] tensor)
        : this("Exception", tensor)
    {
    }

    private static string ArrayToString(Tensor[] tensors) => string.Join<Tensor>(", ", tensors);
}
