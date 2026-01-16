namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/FbUtils.java
 */

public static class FbUtils
{
    public static IComparer<int[]> SolutionComparator { get; } = Comparer<int[]>.Create(
        (left, right) =>
        {
            if (left.Length != right.Length)
            {
                throw new ArgumentException("Solutions have different sizes.");
            }

            int result = 0;
            for (int i = 0; i < left.Length; ++i)
            {
                result = left[i].CompareTo(right[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return result;
        });

    public static IList<int[]> GetAllSolutions(params int[][] equations)
    {
        ArgumentNullException.ThrowIfNull(equations);

        List<int[]> solutions = [];
        FrobeniusSolver fbSolver = new(equations);
        int[]? solution;
        while ((solution = fbSolver.Take()) is not null)
        {
            solutions.Add(solution);
        }

        return solutions;
    }

    public static IEnumerator<int[]> Iterator(int[][] equations)
    {
        return Iterable(equations).GetEnumerator();
    }

    public static IEnumerable<int[]> Iterable(int[][] equations)
    {
        ArgumentNullException.ThrowIfNull(equations);

        FrobeniusSolver fbSolver = new(equations);
        int[]? solution;
        while ((solution = fbSolver.Take()) is not null)
        {
            yield return solution;
        }
    }
}
