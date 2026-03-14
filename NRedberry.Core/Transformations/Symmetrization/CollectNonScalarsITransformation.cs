using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Port of cc.redberry.core.transformations.CollectNonScalarsTransformation.
/// </summary>
public sealed class CollectNonScalarsITransformation : ITransformation
{
    public static CollectNonScalarsITransformation Instance { get; } = new();

    private CollectNonScalarsITransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return CollectNonScalars(tensor);
    }

    public static Tensor CollectNonScalars(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        FromChildToParentIterator iterator = new(tensor);
        Tensor current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Sum)
            {
                SumBuilderSplitingScalars builder = new(current.Size);
                foreach (Tensor item in current)
                {
                    builder.Put(item);
                }

                iterator.Set(builder.Build());
            }
        }

        return iterator.Result();
    }
}
