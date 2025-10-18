using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.LeviCivitaSimplifyTransformation.
/// </summary>
public sealed class LeviCivitaSimplifyTransformation : TransformationToStringAble
{
    private readonly ITransformation _simplifications;

    public LeviCivitaSimplifyTransformation(SimpleTensor leviCivita, bool minkowskiSpace)
    {
        throw new NotImplementedException();
    }

    public LeviCivitaSimplifyTransformation(SimpleTensor leviCivita, bool minkowskiSpace, ITransformation simplifications)
    {
        throw new NotImplementedException();
    }

    public LeviCivitaSimplifyTransformation(SimpleTensor leviCivita, bool minkowskiSpace, ITransformation simplifications, ITransformation overallSimplifications)
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
