using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandTensorsTransformation.
/// </summary>
public sealed class ExpandTensorsTransformation : ITransformation
{
    public static ExpandTensorsTransformation Instance => throw new NotImplementedException();

    private readonly bool leaveScalars;
    private readonly ITransformation[] transformations = [];
    private readonly TraverseGuide traverseGuide = null!;

    private ExpandTensorsTransformation()
    {
        throw new NotImplementedException();
    }

    public ExpandTensorsTransformation(params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public ExpandTensorsTransformation(bool leaveScalars, params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public ExpandTensorsTransformation(bool leaveScalars, ITransformation[] transformations, TraverseGuide traverseGuide)
    {
        throw new NotImplementedException();
    }

    public ExpandTensorsTransformation(ExpandTensorsOptions options)
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
