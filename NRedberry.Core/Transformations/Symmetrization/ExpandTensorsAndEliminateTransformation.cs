using NRedberry.Tensors;
using NRedberry.Transformations.Expand;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ExpandTensorsAndEliminateTransformation.
/// </summary>
public sealed class ExpandTensorsAndEliminateTransformation : TransformationToStringAble
{
    public static ExpandTensorsAndEliminateTransformation Instance => throw new NotImplementedException();

    private readonly ITransformation[] transformations = null!;

    private ExpandTensorsAndEliminateTransformation()
    {
        throw new NotImplementedException();
    }

    public ExpandTensorsAndEliminateTransformation(params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public ExpandTensorsAndEliminateTransformation(ExpandOptions options)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandTensorsAndEliminate(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandTensorsAndEliminate(Tensor tensor, params ITransformation[] transformations)
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
