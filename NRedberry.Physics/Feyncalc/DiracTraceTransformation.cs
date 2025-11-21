using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.DiracTraceTransformation.
/// </summary>
public sealed class DiracTraceTransformation : AbstractFeynCalcTransformation
{
    private readonly ITransformation? _simplifyLeviCivita;
    private readonly bool _useCache;

    public DiracTraceTransformation(DiracOptions options)
        : base(DoLeviCivita(options), null)
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

    private static DiracOptions DoLeviCivita(DiracOptions options)
    {
        throw new NotImplementedException();
    }
}
