using NRedberry.Concurrent;

namespace NRedberry.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/FrobeniusSolver.java
 */

public sealed class FrobeniusSolver : IOutputPort<int[]>
{
    public FrobeniusSolver(params int[][] equations)
    {
        throw new NotImplementedException();
    }

    public int[] Take()
    {
        throw new NotImplementedException();
    }
}
