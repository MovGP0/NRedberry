using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandOptions.
/// </summary>
public class ExpandOptions
{
    public ITransformation? Simplifications { get; set; }
    public TraverseGuide? TraverseGuide { get; set; }

    public ExpandOptions()
    {
        throw new NotImplementedException();
    }
}
