using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Compatibility wrapper around <see cref="EliminateDueSymmetriesTransformation"/>.
/// </summary>
public sealed class EliminateFromSymmetriesITransformation : ITransformation
{
    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return EliminateDueSymmetriesTransformation.Instance.Transform(tensor);
    }
}
