using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ToNumericTransformation.
/// </summary>
public sealed class ToNumericTransformation : ITransformation
{
    public static ToNumericTransformation Instance { get; } = new();

    private ToNumericTransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        return ToNumeric(tensor);
    }

    /*
     * Original Java behavior:
     * - singleton TO_NUMERIC
     * - transform delegates to ToNumeric
     * - ToNumeric walks FromChildToParentIterator and replaces Complex with numeric value
     */
    public static Tensor ToNumeric(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        FromChildToParentIterator iterator = new(tensor);
        Tensor current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Complex complex)
            {
                iterator.Set(complex.GetNumericValue());
            }
        }

        return iterator.Result();
    }
}
