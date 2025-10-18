using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Solver;

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
