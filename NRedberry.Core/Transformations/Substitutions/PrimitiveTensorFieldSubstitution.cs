using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.PrimitiveTensorFieldSubstitution.
/// </summary>
internal sealed class PrimitiveTensorFieldSubstitution : PrimitiveSubstitution
{
    public PrimitiveTensorFieldSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
        throw new NotImplementedException();
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }

    private Tensor NewToInternal(DFromTo fromTo, TensorField currentField, Tensor currentNode, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }

    private sealed class DFromTo
    {
        public DFromTo(TensorField from, Tensor to)
        {
            throw new NotImplementedException();
        }

        public TensorField From => throw new NotImplementedException();

        public Tensor To => throw new NotImplementedException();
    }
}
