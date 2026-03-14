using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ToNumericTransformation.
/// </summary>
public sealed class ToNumericITransformation : ITransformation
{
    public static ToNumericITransformation Instance { get; } = new();

    private ToNumericITransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        return ToNumericTransformation.Instance.Transform(tensor);
    }

    public static Tensor ToNumeric(Tensor tensor)
    {
        return ToNumericTransformation.ToNumeric(tensor);
    }
}
