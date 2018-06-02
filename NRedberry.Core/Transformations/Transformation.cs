using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations
{
    public interface ITransformation
    {
        Tensor Transform(Tensor t);
    }
}
