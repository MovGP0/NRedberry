using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandOptions.
/// </summary>
public class ExpandOptions
{
    public ITransformation? Simplifications { get; set; } = Transformation.Identity;
    public TraverseGuide? TraverseGuide { get; set; } = new DefaultExpandTraverseGuide();
}
