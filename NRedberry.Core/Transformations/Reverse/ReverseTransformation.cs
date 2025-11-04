using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Reverse;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.reverse.ReverseTransformation.
/// </summary>
public sealed class ReverseTransformation : TransformationToStringAble
{
    private readonly SingleReverse[] reversers = [];

    public ReverseTransformation(params IndexType[] types)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
