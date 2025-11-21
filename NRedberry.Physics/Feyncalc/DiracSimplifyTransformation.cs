using NRedberry.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.DiracSimplifyTransformation.
/// </summary>
public sealed class DiracSimplifyTransformation : AbstractTransformationWithGammas
{
    private readonly SimplifyGamma5Transformation _simplifyGamma5;
    private readonly DiracSimplify0 _simplify0;
    private readonly DiracSimplify1 _simplify1;

    public DiracSimplifyTransformation(DiracOptions options)
        : base(options)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }
}
