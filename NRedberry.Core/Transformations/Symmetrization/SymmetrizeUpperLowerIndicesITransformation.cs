using NRedberry.Core.Combinatorics;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Port of cc.redberry.core.transformations.symmetrization.SymmetrizeUpperLowerIndicesTransformation.
/// </summary>
public sealed class SymmetrizeUpperLowerIndicesITransformation : ITransformation
{
    public static SymmetrizeUpperLowerIndicesITransformation Instance { get; } = new();

    private SymmetrizeUpperLowerIndicesITransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        return SymmetrizeUpperLowerIndices(tensor);
    }

    public static Tensor SymmetrizeUpperLowerIndices(Tensor tensor)
    {
        return SymmetrizeUpperLowerIndices(tensor, false);
    }

    public static Tensor SymmetrizeUpperLowerIndices(Tensor tensor, bool multiplyOnSymmetryFactor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Indices.Indices indices = IndicesFactory.Create(tensor.Indices.GetFree());
        int[] indicesArray = indices.AllIndices.ToArray();
        IList<Permutation> symmetries = TensorUtils.GetIndicesSymmetriesForIndicesWithSameStates(indicesArray, tensor);
        int lowerCount = indices.LowerIndices.Length;
        int upperCount = indices.UpperIndices.Length;

        SumBuilder sumBuilder = new();
        List<int[]> generatedPermutations = [];
        if (upperCount != 0 && lowerCount != 0)
        {
            IntPermutationsGenerator lowerIndicesPermutationsGenerator = new(lowerCount);
            while (lowerIndicesPermutationsGenerator.Take() is { } lowerPermutationRaw)
            {
                int[] lowerPermutation = (int[])lowerPermutationRaw.Clone();
                for (int i = 0; i < lowerCount; ++i)
                {
                    lowerPermutation[i] += upperCount;
                }

                IntPermutationsGenerator upperIndicesPermutationGenerator = new(upperCount);
                while (upperIndicesPermutationGenerator.Take() is { } upperPermutation)
                {
                    Tensor? summand = Permute(
                        tensor,
                        indicesArray,
                        upperPermutation,
                        lowerPermutation,
                        generatedPermutations,
                        symmetries);
                    if (summand is not null)
                    {
                        sumBuilder.Put(summand);
                    }
                }
            }
        }
        else if (upperCount == 0)
        {
            IntPermutationsGenerator lowerIndicesPermutationsGenerator = new(lowerCount);
            while (lowerIndicesPermutationsGenerator.Take() is { } lowerPermutation)
            {
                Tensor? summand = Permute(
                    tensor,
                    indicesArray,
                    [],
                    lowerPermutation,
                    generatedPermutations,
                    symmetries);
                if (summand is not null)
                {
                    sumBuilder.Put(summand);
                }
            }
        }
        else
        {
            IntPermutationsGenerator upperIndicesPermutationGenerator = new(upperCount);
            while (upperIndicesPermutationGenerator.Take() is { } upperPermutation)
            {
                Tensor? summand = Permute(
                    tensor,
                    indicesArray,
                    upperPermutation,
                    [],
                    generatedPermutations,
                    symmetries);
                if (summand is not null)
                {
                    sumBuilder.Put(summand);
                }
            }
        }

        Tensor result = sumBuilder.Build();
        if (!multiplyOnSymmetryFactor || generatedPermutations.Count == 0)
        {
            return result;
        }

        return Tensors.Tensors.Multiply(
            new Complex(new Rational(1, generatedPermutations.Count)),
            result);
    }

    private static Tensor? Permute(
        Tensor tensor,
        int[] indicesArray,
        int[] upperPermutation,
        int[] lowerPermutation,
        IList<int[]> generatedPermutations,
        IList<Permutation> symmetries)
    {
        int[] permutation = new int[upperPermutation.Length + lowerPermutation.Length];
        Array.Copy(upperPermutation, permutation, upperPermutation.Length);
        Array.Copy(lowerPermutation, 0, permutation, upperPermutation.Length, lowerPermutation.Length);

        foreach (int[] generated in generatedPermutations)
        {
            foreach (Permutation symmetry in symmetries)
            {
                if (permutation.AsSpan().SequenceEqual(symmetry.Permute(generated)))
                {
                    return null;
                }
            }
        }

        generatedPermutations.Add(permutation);

        int[] newIndices = new int[indicesArray.Length];
        for (int i = 0; i < indicesArray.Length; ++i)
        {
            newIndices[i] = indicesArray[permutation[i]];
        }

        return ApplyIndexMapping.Apply(tensor, new Mapping(indicesArray, newIndices), []);
    }

    internal static Tensor[] GetAllPermutations(Tensor tensor)
    {
        return SymmetrizeUpperLowerIndices(tensor).ToArray();
    }
}
