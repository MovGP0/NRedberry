using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.CollectNonScalarsTransformation.
/// </summary>
public sealed class CollectNonScalarsITransformation : ITransformation
{
    public static CollectNonScalarsITransformation Instance => throw new NotImplementedException();

    private CollectNonScalarsITransformation()
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor CollectNonScalars(Tensor tensor)
    {
        throw new NotImplementedException();
    }
}
