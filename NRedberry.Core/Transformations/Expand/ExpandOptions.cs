using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
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
        Simplifications = Transformation.Identity;
        TraverseGuide = new DefaultExpandTraverseGuide();
    }
}
