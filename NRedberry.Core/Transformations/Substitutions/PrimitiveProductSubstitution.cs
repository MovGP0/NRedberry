using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.PrimitiveProductSubstitution.
/// </summary>
internal sealed class PrimitiveProductSubstitution : PrimitiveSubstitution
{
    public PrimitiveProductSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
        throw new NotImplementedException();
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }

    private SubsResult? AtomicSubstitute(PContent content, ForbiddenContainer forbidden, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }

    private static Tensor[] Extract(Tensor[] source, int[] positions)
    {
        throw new NotImplementedException();
    }

    private sealed class ForbiddenContainer
    {
        public HashSet<int>? Forbidden
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }

    private sealed class SubsResult
    {
        public Tensor NewTo => throw new NotImplementedException();

        public PContent Remainder => throw new NotImplementedException();

        public SubsResult(Tensor newTo, PContent remainder)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class PContent
    {
        public PContent(Tensor[] indexless, Tensor data)
        {
            throw new NotImplementedException();
        }

        public Tensor[] Indexless => throw new NotImplementedException();

        public Tensor Data => throw new NotImplementedException();
    }
}
