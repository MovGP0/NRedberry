using NRedberry.Indices;

namespace NRedberry.Tensors;

public sealed class TensorField(int name, SimpleIndices indices, Tensor[] args, SimpleIndices[] argIndices)
    : SimpleTensor(name, indices)
{
    private Tensor[] Args { get; } = args;
    private SimpleIndices[] ArgIndices { get; } = argIndices;

    internal Tensor[] Arguments => Args;
    internal SimpleIndices[] ArgumentIndices => ArgIndices;

    public TensorField(TensorField field, Tensor[] args)
        : this(field.Name, field.SimpleIndices, args, field.ArgIndices)
    {
    }
}
