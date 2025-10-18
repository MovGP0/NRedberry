using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Transformations.Powerexpand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.powerexpand.PowerExpandTransformation.
/// </summary>
public sealed class PowerExpandTransformation : TransformationToStringAble
{
    public static PowerExpandTransformation Instance => throw new NotImplementedException();

    private readonly IIndicator<Tensor> indicator;

    private PowerExpandTransformation()
    {
        throw new NotImplementedException();
    }

    public PowerExpandTransformation(IIndicator<Tensor> indicator)
    {
        this.indicator = indicator ?? throw new ArgumentNullException(nameof(indicator));
        throw new NotImplementedException();
    }

    public PowerExpandTransformation(params SimpleTensor[] variables)
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

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
