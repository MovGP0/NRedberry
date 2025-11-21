using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Reverse;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.reverse.SingleReverse.
/// </summary>
internal sealed class SingleReverse : ITransformation
{
    public IndexType Type { get; }

    public SingleReverse(IndexType type)
    {
        Type = type;
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor InverseOrderOfMatrices(Tensor tensor, IndexType type)
    {
        throw new NotImplementedException();
    }

    private static Tensor InverseOrderOfMatricesInternal(Tensor tensor, IndexType type)
    {
        throw new NotImplementedException();
    }

    private static bool IsMatrix(Tensor tensor, IndexType type)
    {
        throw new NotImplementedException();
    }
}
