using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.PassarinoVeltman.
/// </summary>
public static class PassarinoVeltman
{
    public static Expression GenerateSubstitution(int order, SimpleTensor loopMomentum, SimpleTensor[] externalMomentums)
    {
        throw new NotImplementedException();
    }

    public static Expression GenerateSubstitution(int order, SimpleTensor loopMomentum, SimpleTensor[] externalMomentums, ITransformation simplifications)
    {
        throw new NotImplementedException();
    }

    private static void Check(SimpleTensor loopMomentum, SimpleTensor[] externalMomentums)
    {
        throw new NotImplementedException();
    }

    private static void Check(IndexType type, SimpleTensor momentum)
    {
        throw new NotImplementedException();
    }

    private static Tensor[] CoefficientsList(Tensor tensor, SimpleTensor[] coefficients)
    {
        throw new NotImplementedException();
    }

    private static Monomial? GetFromProduct(Tensor tensor, SimpleTensor[] coefficients)
    {
        throw new NotImplementedException();
    }

    private record Monomial(Tensor Coefficient, Index index);
}
