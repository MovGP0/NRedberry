using System;

namespace NRedberry.Core.Solver.Frobenius;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/frobenius/FinalSolutionProvider.java
 */

internal sealed class FinalSolutionProvider : SolutionProviderAbstract
{
    public FinalSolutionProvider(SolutionProvider provider, int position, int[] coefficients)
        : base(provider, position, coefficients)
    {
        throw new NotImplementedException();
    }

    public override int[] Take()
    {
        throw new NotImplementedException();
    }
}
