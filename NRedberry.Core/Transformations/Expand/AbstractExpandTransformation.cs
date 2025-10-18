using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Iterators;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.AbstractExpandTransformation.
/// </summary>
public abstract class AbstractExpandTransformation : ITransformation
{
    protected readonly ITransformation[] transformations = [];
    protected readonly TraverseGuide traverseGuide = null!;

    protected static TraverseGuide DefaultExpandTraverseGuide => throw new NotImplementedException();

    protected AbstractExpandTransformation()
    {
        throw new NotImplementedException();
    }

    protected AbstractExpandTransformation(ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    protected AbstractExpandTransformation(ITransformation[] transformations, TraverseGuide traverseGuide)
    {
        throw new NotImplementedException();
    }

    protected AbstractExpandTransformation(ExpandOptions options)
    {
        throw new NotImplementedException();
    }

    public virtual Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    protected abstract Tensor ExpandProduct(Product product, ITransformation[] transformations);

    public virtual string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
