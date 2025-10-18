using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Powerexpand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.powerexpand.PowerUnfoldTransformation.
/// </summary>
public sealed class PowerUnfoldTransformation : ITransformation, TransformationToStringAble
{
    public static PowerUnfoldTransformation Instance => throw new NotImplementedException();

    private readonly SimpleTensor[] variables;

    public PowerUnfoldTransformation(SimpleTensor[] variables)
    {
        this.variables = variables ?? throw new ArgumentNullException(nameof(variables));
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
