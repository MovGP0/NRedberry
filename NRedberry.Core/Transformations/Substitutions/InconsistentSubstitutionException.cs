using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.InconsistentSubstitutionException.
/// </summary>
public class InconsistentSubstitutionException : TransformationException
{
    public InconsistentSubstitutionException(Tensor from, Tensor to, Tensor current)
    {
        throw new NotImplementedException();
    }
}
