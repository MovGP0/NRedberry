using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

public interface ITransformation
{
    Tensor Transform(Tensor t);
}
