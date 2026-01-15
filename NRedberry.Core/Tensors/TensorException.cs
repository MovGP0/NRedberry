using System.Runtime.Serialization;

namespace NRedberry.Tensors;

public class TensorException : Exception
{
    public TensorException()
        : this("Exception")
    {
    }

    public TensorException(string message, params Tensor[] tensors)
        : base(BuildMessage(message, CC.NameManager.Seed, tensors))
    {
        Tensors = tensors ?? Array.Empty<Tensor>();
    }

    public TensorException(string message, long nmseed, params Tensor[] tensors)
        : base(BuildMessage(message, nmseed, tensors))
    {
        Tensors = tensors ?? Array.Empty<Tensor>();
    }

    public TensorException(string message)
        : base(BuildMessage(message, CC.NameManager.Seed))
    {
        Tensors = Array.Empty<Tensor>();
    }

    public TensorException(string message, long nmseed)
        : base(BuildMessage(message, nmseed))
    {
        Tensors = Array.Empty<Tensor>();
    }

    public TensorException(string message, Exception innerException)
        : base(BuildMessage(message, CC.NameManager.Seed), innerException)
    {
        Tensors = Array.Empty<Tensor>();
    }

    protected TensorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Tensors = Array.Empty<Tensor>();
    }

    public TensorException(params Tensor[] tensor)
        : this("Exception", tensor)
    {
    }

    public Tensor[] Tensors { get; }

    private static string BuildMessage(string message, long nmseed, params Tensor[]? tensors)
    {
        if (tensors is null || tensors.Length == 0)
        {
            return $"{message}\n nmseed: {nmseed}";
        }

        return $"\"{message}\" in tensors {ArrayToString(tensors)}\n nmseed: {nmseed}";
    }

    private static string ArrayToString(Tensor[] tensors) => string.Join<Tensor>(", ", tensors);
}
