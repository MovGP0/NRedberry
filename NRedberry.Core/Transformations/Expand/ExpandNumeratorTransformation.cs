using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandNumeratorTransformation.
/// </summary>
public sealed class ExpandNumeratorTransformation : AbstractExpandNumeratorDenominatorTransformation
{
    public static ExpandNumeratorTransformation Instance => throw new NotImplementedException();

    private ExpandNumeratorTransformation()
    {
        throw new NotImplementedException();
    }

    public ExpandNumeratorTransformation(ITransformation[] transformations)
        : base(transformations)
    {
        throw new NotImplementedException();
    }

    public ExpandNumeratorTransformation(ExpandOptions options)
        : base(options)
    {
        throw new NotImplementedException();
    }

    public static Tensor Expand(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    protected override Tensor ExpandProduct(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
