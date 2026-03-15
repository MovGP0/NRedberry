using NRedberry.Core.Combinatorics;
using NRedberry.Contexts;
using NRedberry.Groups;
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
        HashSet<string> generatedSummands = [];
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
                        symmetries,
                        generatedSummands);
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
                    symmetries,
                    generatedSummands);
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
                    symmetries,
                    generatedSummands);
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
        IList<Permutation> symmetries,
        ISet<string> generatedSummands)
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

        Tensor summand = ApplyIndexMapping.Apply(tensor, new Mapping(indicesArray, newIndices), []);
        summand = NormalizeSummand(summand);

        string summandKey = GetSummandKey(summand);
        if (!generatedSummands.Add(summandKey))
        {
            return null;
        }

        return summand;
    }

    internal static Tensor[] GetAllPermutations(Tensor tensor)
    {
        return SymmetrizeUpperLowerIndices(tensor).ToArray();
    }

    private static Tensor NormalizeSummand(Tensor tensor)
    {
        if (tensor is SimpleTensor simpleTensor)
        {
            return NormalizeSimpleTensor(simpleTensor);
        }

        if (tensor is Product)
        {
            Tensor[] normalizedFactors = tensor.ToArray();
            for (int i = 0; i < normalizedFactors.Length; ++i)
            {
                if (normalizedFactors[i] is SimpleTensor factor)
                {
                    normalizedFactors[i] = NormalizeSimpleTensor(factor);
                }
            }

            return Tensors.Tensors.Multiply(normalizedFactors);
        }

        return tensor;
    }

    private static SimpleTensor NormalizeSimpleTensor(SimpleTensor tensor)
    {
        if (tensor.SimpleIndices.Size() != 2 || !HasSymmetricSwap(tensor))
        {
            return tensor;
        }

        int first = tensor.SimpleIndices[0];
        int second = tensor.SimpleIndices[1];
        if (first <= second)
        {
            return tensor;
        }

        return Tensor.SimpleTensor(
            tensor.Name,
            IndicesFactory.CreateSimple(null, second, first));
    }

    private static bool HasSymmetricSwap(SimpleTensor tensor)
    {
        foreach (Symmetry basis in tensor.SimpleIndices.Symmetries.Basis)
        {
            if (!basis.IsAntisymmetry
                && basis.NewIndexOf(0) == 1
                && basis.NewIndexOf(1) == 0)
            {
                return true;
            }
        }

        return false;
    }

    private static string GetSummandKey(Tensor tensor)
    {
        if (tensor is Product product)
        {
            string[] factorKeys = new string[product.Size];
            for (int i = 0; i < product.Size; ++i)
            {
                factorKeys[i] = GetSummandKey(product[i]);
            }

            Array.Sort(factorKeys, StringComparer.Ordinal);
            return string.Join("*", factorKeys);
        }

        if (tensor is SimpleTensor simpleTensor)
        {
            return NormalizeSimpleTensor(simpleTensor).ToString(OutputFormat.Redberry);
        }

        return tensor.ToString(OutputFormat.Redberry);
    }
}
