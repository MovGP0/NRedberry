using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ApplyDiracDeltasTransformation.
/// </summary>
public sealed class ApplyDiracDeltasTransformation : TransformationToStringAble
{
    public static ApplyDiracDeltasTransformation Instance => throw new NotImplementedException();

    private ApplyDiracDeltasTransformation()
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private static bool ContainsDiracDeltas(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private static ITransformation CreateSubstitution(TensorField delta)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    private static bool Intersects(HashSet<int> first, HashSet<int> second)
    {
        throw new NotImplementedException();
    }
}
