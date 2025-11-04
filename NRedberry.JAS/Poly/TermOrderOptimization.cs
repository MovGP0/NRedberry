using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Vector;
using BigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Term order optimization. For example computation of optimal permutations of variables in polynomials.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.TermOrderOptimization
/// </remarks>
public static class TermOrderOptimization
{
    public static List<GenPolynomial<BigInteger>>? DegreeMatrix<C>(GenPolynomial<C>? polynomial)
        where C : RingElem<C>
    {
        if (polynomial is null)
        {
            return null;
        }

        BigInteger coefficientFactory = new ();
        GenPolynomialRing<BigInteger> univariateRing = new (coefficientFactory, 1);

        int variableCount = polynomial.NumberOfVariables();
        List<GenPolynomial<BigInteger>> degreeMatrix = new (variableCount);
        for (int i = 0; i < variableCount; i++)
        {
            degreeMatrix.Add(new GenPolynomial<BigInteger>(univariateRing));
        }

        if (polynomial.IsZero())
        {
            return degreeMatrix;
        }

        foreach (ExpVector exponent in polynomial.Terms.Keys)
        {
            degreeMatrix = ExpVectorAdd(degreeMatrix, exponent);
        }

        return degreeMatrix;
    }

    public static List<GenPolynomial<BigInteger>>? DegreeMatrix<C>(ICollection<GenPolynomial<C>> polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(polynomials);

        BasicLinAlg<GenPolynomial<BigInteger>> blas = new ();
        List<GenPolynomial<BigInteger>>? result = null;
        foreach (GenPolynomial<C> polynomial in polynomials)
        {
            List<GenPolynomial<BigInteger>>? matrix = DegreeMatrix(polynomial);
            if (matrix is null)
            {
                continue;
            }

            result = result is null ? matrix : blas.VectorAdd(result, matrix)!;
        }

        return result;
    }

    public static List<GenPolynomial<BigInteger>> ExpVectorAdd(List<GenPolynomial<BigInteger>> degreeMatrix, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(degreeMatrix);
        ArgumentNullException.ThrowIfNull(exponent);

        for (int i = 0; i < degreeMatrix.Count && i < exponent.Length(); i++)
        {
            GenPolynomial<BigInteger> polynomial = degreeMatrix[i];
            long value = exponent.GetVal(i);
            ExpVector unit = ExpVector.Create(1, 0, value);
            BigInteger coefficient = polynomial.Ring.GetOneCoefficient();
            degreeMatrix[i] = polynomial.Sum(coefficient, unit);
        }

        return degreeMatrix;
    }

    public static List<int> OptimalPermutation(List<GenPolynomial<BigInteger>> degreeMatrix)
    {
        ArgumentNullException.ThrowIfNull(degreeMatrix);
        List<int> permutation = new (degreeMatrix.Count);

        if (degreeMatrix.Count == 0)
        {
            return permutation;
        }

        if (degreeMatrix.Count == 1)
        {
            permutation.Add(0);
            return permutation;
        }

        SortedDictionary<GenPolynomial<BigInteger>, List<int>> map = new ();
        for (int index = 0; index < degreeMatrix.Count; index++)
        {
            GenPolynomial<BigInteger> row = degreeMatrix[index];
            if (!map.TryGetValue(row, out List<int>? indices))
            {
                indices = new List<int>(3);
                map[row] = indices;
            }

            indices.Add(index);
        }

        foreach (List<int> indices in map.Values)
        {
            foreach (int index in indices)
            {
                permutation.Add(index);
            }
        }

        return permutation;
    }

    public static List<int> InversePermutation(List<int> permutation)
    {
        if (permutation == null || permutation.Count <= 1)
        {
            return permutation ?? [];
        }

        List<int> inverse = new (permutation);
        for (int i = 0; i < permutation.Count; i++)
        {
            inverse[permutation[i]] = i;
        }

        return inverse;
    }

    public static string[] StringArrayPermutation(List<int> permutation, string[] array)
    {
        if (array == null || array.Length <= 1)
        {
            return array ?? [];
        }

        string[] result = new string[array.Length];
        int index = 0;
        foreach (int value in permutation)
        {
            result[index] = array[value];
            index++;
        }

        return result;
    }

    public static long[] LongArrayPermutation(List<int> permutation, long[] array)
    {
        if (array == null || array.Length <= 1)
        {
            return array ?? [];
        }

        long[] result = new long[array.Length];
        int index = 0;
        foreach (int value in permutation)
        {
            result[index] = array[value];
            index++;
        }

        return result;
    }

    public static ExpVector Permutation(List<int> permutation, ExpVector exponent)
    {
        if (exponent is null)
        {
            return exponent;
        }

        long[] values = LongArrayPermutation(permutation, exponent.GetVal());
        return ExpVector.Create(values);
    }

    public static GenPolynomial<C>? Permutation<C>(List<int> permutation, GenPolynomialRing<C> ring, GenPolynomial<C>? polynomial)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        if (polynomial is null)
        {
            return null;
        }

        GenPolynomial<C> result = new (ring);
        SortedDictionary<ExpVector, C> destination = result.Terms;
        foreach (KeyValuePair<ExpVector, C> entry in polynomial.Terms)
        {
            ExpVector exponent = Permutation(permutation, entry.Key);
            destination[exponent] = entry.Value;
        }

        return result;
    }

    public static List<GenPolynomial<C>>? Permutation<C>(List<int> permutation, GenPolynomialRing<C> ring, List<GenPolynomial<C>>? polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        if (polynomials == null || polynomials.Count == 0)
        {
            return polynomials;
        }

        List<GenPolynomial<C>> result = new (polynomials.Count);
        foreach (GenPolynomial<C> polynomial in polynomials)
        {
            GenPolynomial<C>? permuted = Permutation(permutation, ring, polynomial);
            if (permuted is not null)
            {
                result.Add(permuted);
            }
        }

        return result;
    }

    public static GenPolynomialRing<C>? Permutation<C>(List<int> permutation, GenPolynomialRing<C>? ring)
        where C : RingElem<C>
    {
        if (ring is null)
        {
            return null;
        }

        string[]? variables = ring.GetVars();
        if (variables is null || variables.Length <= 1)
        {
            return ring;
        }

        TermOrder termOrder = ring.Tord;
        if (termOrder.GetEvord2() != 0)
        {
            termOrder = new TermOrder(termOrder.GetEvord2());
        }

        long[][]? weight = termOrder.GetWeight();
        if (weight != null)
        {
            long[][] permuted = new long[weight.Length][];
            for (int i = 0; i < weight.Length; i++)
            {
                permuted[i] = LongArrayPermutation(permutation, weight[i]);
            }

            termOrder = new TermOrder(permuted);
        }

        string[] reversed = new string[variables.Length];
        for (int i = 0; i < reversed.Length; i++)
        {
            reversed[i] = variables[reversed.Length - 1 - i];
        }

        string[] permutedVariables = StringArrayPermutation(permutation, reversed);
        string[] finalVariables = new string[permutedVariables.Length];
        for (int i = 0; i < finalVariables.Length; i++)
        {
            finalVariables[i] = permutedVariables[finalVariables.Length - 1 - i];
        }

        return new GenPolynomialRing<C>(ring.CoFac, ring.Nvar, termOrder, finalVariables);
    }

    public static OptimizedPolynomialList<C> OptimizeTermOrder<C>(GenPolynomialRing<C> ring, List<GenPolynomial<C>> polynomials)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(polynomials);

        List<GenPolynomial<BigInteger>>? matrix = DegreeMatrix(polynomials);
        List<int> permutation = OptimalPermutation(matrix ?? []);

        GenPolynomialRing<C>? permutedRing = Permutation(permutation, ring);
        if (permutedRing is null)
        {
            throw new InvalidOperationException("Permutation of the polynomial ring returned null.");
        }

        List<GenPolynomial<C>>? permutedPolynomials = Permutation(permutation, permutedRing, polynomials);
        permutedPolynomials ??= [];

        return new OptimizedPolynomialList<C>(permutation, permutedRing, permutedPolynomials);
    }
}
