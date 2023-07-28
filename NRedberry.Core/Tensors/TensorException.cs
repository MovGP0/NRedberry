using System;

namespace NRedberry.Core.Tensors;

public class TensorException : Exception
{
    public TensorException(string message, params Tensor[] tensors)
        : base($"\"{message}\" in tensors {ArrayToString(tensors)}\n nmseed: {CC.GetNameManager().Seed}")
    {
    }

    public TensorException(string message)
        : base($"{message}\n nmseed: {CC.GetNameManager().Seed}")
    {
    }

    public TensorException(params Tensor[] tensor)
        : this("Exception", tensor)
    {
    }

    private static string ArrayToString(Tensor[] tensors) => string.Join<Tensor>(", ", tensors);
}