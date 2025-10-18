using NRedberry.Core.Combinatorics;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.symmetrization.SymmetrizeUpperLowerIndicesTransformation.
/// </summary>
public sealed class SymmetrizeUpperLowerIndicesITransformation : ITransformation
{
    public static SymmetrizeUpperLowerIndicesITransformation Instance => throw new NotImplementedException();

    private SymmetrizeUpperLowerIndicesITransformation()
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor SymmetrizeUpperLowerIndices(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor SymmetrizeUpperLowerIndices(Tensor tensor, bool multiplyOnSymmetryFactor)
    {
        throw new NotImplementedException();
    }

    private static Tensor Permute(Tensor tensor, int[] indicesArray, int[] upperPermutation, int[] lowerPermutation, IList<int[]> generatedPermutations, IList<Permutation> symmetries)
    {
        throw new NotImplementedException();
    }

    internal static Tensor[] GetAllPermutations(Tensor tensor)
    {
        throw new NotImplementedException();
    }
}
