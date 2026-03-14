using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Removes terms from tensor that are zero because of their symmetries.
/// </summary>
public sealed class EliminateFromSymmetriesITransformation : ITransformation
{
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
