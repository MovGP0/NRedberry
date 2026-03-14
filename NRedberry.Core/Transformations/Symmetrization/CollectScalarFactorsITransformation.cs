using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.CollectScalarFactorsTransformation.
/// </summary>
public sealed class CollectScalarFactorsITransformation : ITransformation
{
    public static CollectScalarFactorsITransformation Instance { get; } = new();

    private readonly TraverseGuide traverseGuide;

    private CollectScalarFactorsITransformation()
    {
        traverseGuide = TraverseGuide.All;
    }

    public CollectScalarFactorsITransformation(TraverseGuide traverseGuide)
    {
        this.traverseGuide = traverseGuide ?? throw new ArgumentNullException(nameof(traverseGuide));
    }

    public Tensor Transform(Tensor tensor)
    {
        return CollectScalarFactors(tensor, traverseGuide);
    }

    public static Tensor CollectScalarFactors(Tensor tensor)
    {
        return CollectScalarFactors(tensor, TraverseGuide.All);
    }

    public static Tensor CollectScalarFactors(Tensor tensor, TraverseGuide traverseGuide)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(traverseGuide);

        FromChildToParentIterator iterator = new(tensor, traverseGuide);
        Tensor current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Product product)
            {
                iterator.Set(CollectScalarFactorsInProduct(product));
            }
        }

        return iterator.Result();
    }

    public static Tensor CollectScalarFactorsInProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (TensorUtils.IsSymbolic(product))
        {
            return product;
        }

        ScalarsBackedProductBuilder builder = new(product.Size, 1, product.Indices.GetFree().Size());
        builder.Put(product);
        return builder.Build();
    }
}
