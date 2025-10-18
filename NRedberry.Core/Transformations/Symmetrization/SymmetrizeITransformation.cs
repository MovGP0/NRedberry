using NRedberry.Core.Groups;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.symmetrization.SymmetrizeTransformation.
/// </summary>
public sealed class SymmetrizeITransformation : ITransformation
{
    public SymmetrizeITransformation(SimpleIndices indices, bool multiplyBySymmetryFactor)
    {
        throw new NotImplementedException();
    }

    public SymmetrizeITransformation(SimpleIndices indices, SymmetrizeOptions options)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private static bool ContainsSubIndices(Indices.Indices indices, Indices.Indices subIndices)
    {
        throw new NotImplementedException();
    }

    private PermutationGroup ConjugatedSymmetriesOfSubIndices(SimpleIndices allIndices)
    {
        throw new NotImplementedException();
    }

    public sealed class SymmetrizeOptions
    {
        public bool MultiplyBySymmetryFactor
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public SymmetrizeOptions()
        {
            throw new NotImplementedException();
        }
    }
}
