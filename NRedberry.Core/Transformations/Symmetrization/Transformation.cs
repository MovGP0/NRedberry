using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

public interface Transformation
{
    Tensor Transform(Tensor t);
    public static readonly Transformation Identity = new IdentityTransformation();
}

public sealed class IdentityTransformation : Transformation
{
    public Tensor Transform(Tensor t) => t;
}
