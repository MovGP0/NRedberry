using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Port of cc.redberry.core.transformations.EliminateDueSymmetriesTransformation.
/// </summary>
public sealed class EliminateDueSymmetriesTransformation : ITransformation
{
    public static EliminateDueSymmetriesTransformation Instance { get; } = new();

    private EliminateDueSymmetriesTransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        TreeTraverseIterator iterator = new(tensor);
        TraverseState? state;
        while ((state = iterator.Next()) is not null)
        {
            if (state != TraverseState.Leaving)
            {
                continue;
            }

            Tensor current = iterator.Current();
            if (TensorUtils.IsZeroDueToSymmetry(current))
            {
                iterator.Set(Complex.Zero);
            }
        }

        return iterator.Result();
    }
}
