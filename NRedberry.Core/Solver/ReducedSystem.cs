using NRedberry.Tensors;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/ReducedSystem.java
 */

public sealed class ReducedSystem
{
    public ReducedSystem(Expression[] equations, SimpleTensor[] unknownCoefficients, Expression[] generalSolutions)
    {
        throw new NotImplementedException();
    }

    public Expression[] GetEquations()
    {
        throw new NotImplementedException();
    }

    public SimpleTensor[] GetUnknownCoefficients()
    {
        throw new NotImplementedException();
    }

    public Expression[] GetGeneralSolutions()
    {
        throw new NotImplementedException();
    }
}
