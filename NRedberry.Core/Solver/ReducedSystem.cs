using NRedberry.Tensors;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/ReducedSystem.java
 */

public sealed class ReducedSystem
{
    public ReducedSystem(Expression[] equations, SimpleTensor[] unknownCoefficients, Expression[] generalSolutions)
    {
        ArgumentNullException.ThrowIfNull(equations);
        ArgumentNullException.ThrowIfNull(unknownCoefficients);
        ArgumentNullException.ThrowIfNull(generalSolutions);

        Equations = equations;
        UnknownCoefficients = unknownCoefficients;
        GeneralSolutions = generalSolutions;
    }

    public Expression[] GetEquations()
    {
        return (Expression[])Equations.Clone();
    }

    public SimpleTensor[] GetUnknownCoefficients()
    {
        return (SimpleTensor[])UnknownCoefficients.Clone();
    }

    public Expression[] GetGeneralSolutions()
    {
        return (Expression[])GeneralSolutions.Clone();
    }

    private Expression[] Equations { get; }

    private SimpleTensor[] UnknownCoefficients { get; }

    private Expression[] GeneralSolutions { get; }
}
