using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.PrimitiveSumSubstitution.
/// </summary>
internal sealed class PrimitiveSumSubstitution : PrimitiveSubstitution
{
    public PrimitiveSumSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
        throw new NotImplementedException();
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }
}
