using NRedberry.Core.Concurrent;

namespace NRedberry.Core.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/SolutionProvider.java
 */

internal interface SolutionProvider : IOutputPort<int[]>
{
    bool Tick();

    int[] CurrentRemainders();
}
