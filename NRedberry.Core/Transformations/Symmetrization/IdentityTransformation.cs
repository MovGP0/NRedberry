using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

public sealed class IdentityTransformation : ITransformation
{
    public Tensor Transform(Tensor t)
    {
        return t;
    }
}
