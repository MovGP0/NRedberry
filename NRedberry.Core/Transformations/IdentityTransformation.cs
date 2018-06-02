using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations
{
    public sealed class IdentityTransformation : ITransformation
    {
        public Tensor Transform(Tensor t)
        {
            return t;
        }
    }
}