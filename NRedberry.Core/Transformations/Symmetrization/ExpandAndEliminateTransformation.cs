using NRedberry.Tensors;
using NRedberry.Transformations.Expand;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ExpandAndEliminateTransformation.
/// </summary>
public sealed class ExpandAndEliminateTransformation : TransformationToStringAble
{
    public static ExpandAndEliminateTransformation Instance { get; } = new();

    private readonly ITransformation[] transformations = null!;

    private ExpandAndEliminateTransformation()
    {
        transformations = [EliminateMetricsTransformation.Instance];
    }

    public ExpandAndEliminateTransformation(params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        this.transformations = new ITransformation[1 + transformations.Length];
        this.transformations[0] = EliminateMetricsTransformation.Instance;
        Array.Copy(transformations, 0, this.transformations, 1, transformations.Length);
    }

    public ExpandAndEliminateTransformation(ExpandOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        transformations = options.Simplifications is null ? Array.Empty<ITransformation>() : [options.Simplifications];
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        Tensor expanded = ExpandTransformation.Expand(tensor, transformations);
        return ApplySequentially(expanded, transformations);
    }

    public static Tensor ExpandAndEliminate(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor ExpandAndEliminate(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandAndEliminateTransformation(transformations).Transform(tensor);
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "ExpandAndEliminate";
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
