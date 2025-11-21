using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Substitutions;

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
