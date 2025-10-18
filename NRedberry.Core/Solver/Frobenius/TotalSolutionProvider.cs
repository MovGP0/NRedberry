using System;
using NRedberry.Core.Concurrent;

namespace NRedberry.Core.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/TotalSolutionProvider.java
 */

internal sealed class TotalSolutionProvider : IOutputPort<int[]>
{
    public TotalSolutionProvider(SolutionProvider[] providers)
    {
        throw new NotImplementedException();
    }

    public int[] Take()
    {
        throw new NotImplementedException();
    }
}
