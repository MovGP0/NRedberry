using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.CollectScalarFactorsTransformation.
/// </summary>
public sealed class CollectScalarFactorsITransformation : ITransformation
{
    public static CollectScalarFactorsITransformation Instance => throw new NotImplementedException();

    private readonly TraverseGuide traverseGuide;

    private CollectScalarFactorsITransformation()
    {
        throw new NotImplementedException();
    }

    public CollectScalarFactorsITransformation(TraverseGuide traverseGuide)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor CollectScalarFactors(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor CollectScalarFactors(Tensor tensor, TraverseGuide traverseGuide)
    {
        throw new NotImplementedException();
    }

    public static Tensor CollectScalarFactorsInProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
