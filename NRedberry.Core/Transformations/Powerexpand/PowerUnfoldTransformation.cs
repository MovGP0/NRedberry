using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Powerexpand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.powerexpand.PowerUnfoldTransformation.
/// </summary>
public sealed class PowerUnfoldTransformation : ITransformation, TransformationToStringAble
{
    public static PowerUnfoldTransformation Instance { get; } = new([]);

    private readonly SimpleTensor[] variables;

    public PowerUnfoldTransformation(SimpleTensor[] variables)
    {
        this.variables = variables ?? throw new ArgumentNullException(nameof(variables));
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        return tensor;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return variables.Length == 0
            ? "PowerUnfold"
            : $"PowerUnfold[{string.Join(", ", variables.Select(tensor => tensor.ToString(outputFormat)))}]";
    }

    public override string ToString()
    {
        return ToString(NRedberry.Tensors.CC.GetDefaultOutputFormat());
    }
}
