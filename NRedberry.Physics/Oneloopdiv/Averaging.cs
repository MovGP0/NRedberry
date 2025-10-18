using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.Averaging.
/// </summary>
public sealed class Averaging : ITransformation
{
    private readonly SimpleTensor constantN = null!;

    public Averaging(SimpleTensor constantN)
    {
        throw new NotImplementedException();
    }

    private static Tensor Average(int[] indices)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private bool IsN(Tensor tensor)
    {
        throw new NotImplementedException();
    }
}
