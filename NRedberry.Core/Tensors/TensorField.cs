using NRedberry.Core.Indices;

namespace NRedberry.Core.Tensors;

public sealed class TensorField : SimpleTensor
{
    private Tensor[] Args { get; }
    private SimpleIndices[] ArgIndices { get; }

    public TensorField(int name, SimpleIndices indices, Tensor[] args, SimpleIndices[] argIndices)
        : base(name, indices)
    {
        Args = args;
        ArgIndices = argIndices;
    }

    public TensorField(TensorField field, Tensor[] args)
        : base(field.Name, field.SimpleIndices)
    {
        Args = args;
        ArgIndices = field.ArgIndices;
    }
}
