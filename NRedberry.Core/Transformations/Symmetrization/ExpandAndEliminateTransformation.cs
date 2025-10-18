using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Expand;

namespace NRedberry.Core.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ExpandAndEliminateTransformation.
/// </summary>
public sealed class ExpandAndEliminateTransformation : TransformationToStringAble
{
    public static ExpandAndEliminateTransformation Instance => throw new NotImplementedException();

    private readonly ITransformation[] transformations = null!;

    private ExpandAndEliminateTransformation()
    {
        throw new NotImplementedException();
    }

    public ExpandAndEliminateTransformation(params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public ExpandAndEliminateTransformation(ExpandOptions options)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandAndEliminate(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandAndEliminate(Tensor tensor, params ITransformation[] transformations)
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
