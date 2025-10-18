using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

public sealed class IdentityTransformation : ITransformation
{
    public Tensor Transform(Tensor t)
    {
        return t;
    }
}
