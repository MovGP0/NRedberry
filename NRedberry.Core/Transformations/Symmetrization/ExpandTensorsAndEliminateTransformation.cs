using NRedberry.Tensors;
using NRedberry.Transformations.Expand;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ExpandTensorsAndEliminateTransformation.
/// </summary>
public sealed class ExpandTensorsAndEliminateTransformation : TransformationToStringAble
{
    public static ExpandTensorsAndEliminateTransformation Instance { get; } = new();

    private readonly ITransformation[] transformations = null!;

    private ExpandTensorsAndEliminateTransformation()
    {
        transformations = [EliminateMetricsTransformation.Instance];
    }

    public ExpandTensorsAndEliminateTransformation(params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        this.transformations = new ITransformation[1 + transformations.Length];
        this.transformations[0] = EliminateMetricsTransformation.Instance;
        Array.Copy(transformations, 0, this.transformations, 1, transformations.Length);
    }

    public ExpandTensorsAndEliminateTransformation(ExpandOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        transformations = options.Simplifications is null
            ? [EliminateMetricsTransformation.Instance]
            : [EliminateMetricsTransformation.Instance, options.Simplifications];
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        Tensor expanded = new ExpandTensorsTransformation(transformations).Transform(tensor);
        return ApplySequentially(expanded, transformations);
    }

    public static Tensor ExpandTensorsAndEliminate(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor ExpandTensorsAndEliminate(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandTensorsAndEliminateTransformation(transformations).Transform(tensor);
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "ExpandTensorsAndEliminate";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }

    private static Tensor ApplySequentially(Tensor tensor, ITransformation[] transformations)
    {
        Tensor current = tensor;
        foreach (ITransformation transformation in transformations)
        {
            current = transformation.Transform(current);
        }

        return current;
    }
}
