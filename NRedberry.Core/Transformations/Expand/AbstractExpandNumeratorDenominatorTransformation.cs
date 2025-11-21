using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.AbstractExpandNumeratorDenominatorTransformation.
/// </summary>
public abstract class AbstractExpandNumeratorDenominatorTransformation : ITransformation
{
    protected readonly ITransformation[] transformations = [];

    protected AbstractExpandNumeratorDenominatorTransformation()
    {
        throw new NotImplementedException();
    }

    protected AbstractExpandNumeratorDenominatorTransformation(ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    protected AbstractExpandNumeratorDenominatorTransformation(ExpandOptions options)
    {
        throw new NotImplementedException();
    }

    public virtual Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    protected abstract Tensor ExpandProduct(Tensor tensor);

    public virtual string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
