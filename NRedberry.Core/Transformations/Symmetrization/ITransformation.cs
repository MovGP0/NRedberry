using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

public interface ITransformation
{
    Tensor Transform(Tensor t);
}
