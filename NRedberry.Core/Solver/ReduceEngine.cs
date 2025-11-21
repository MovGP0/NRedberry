using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/ReduceEngine.java
 */

public static class ReduceEngine
{
    public static ReducedSystem ReduceToSymbolicSystem(Expression[] equations, SimpleTensor[] vars, ITransformation[] rules)
    {
        throw new NotImplementedException();
    }

    public static ReducedSystem ReduceToSymbolicSystem(Expression[] equations, SimpleTensor[] vars, ITransformation[] rules, bool[] symmetricForm)
    {
        throw new NotImplementedException();
    }
}
