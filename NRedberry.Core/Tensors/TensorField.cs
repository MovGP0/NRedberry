using NRedberry.Core.Indices;

namespace NRedberry.Core.Tensors
{
    public sealed class TensorField : SimpleTensor
    {
        private Tensor[] Args { get; }
        private ISimpleIndices[] ArgIndices { get; }

        public TensorField(int name, ISimpleIndices indices, Tensor[] args, ISimpleIndices[] argIndices)
            : base(name, indices)
        {
            Args = args;
            ArgIndices = argIndices;
        }

        public TensorField(TensorField field, Tensor[] args)
            : base(field.Name, field.Indices)
        {
            Args = args;
            ArgIndices = field.ArgIndices;
        }
    }
}