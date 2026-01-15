using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandTransformation.
/// </summary>
public sealed class ExpandTransformation : AbstractExpandTransformation
{
    public static ExpandTransformation Instance { get; } = new();

    private ExpandTransformation()
    {
    }

    public ExpandTransformation(ITransformation[] transformations)
        : base(transformations)
    {
    }

    public ExpandTransformation(ITransformation[] transformations, TraverseGuide traverseGuide)
        : base(transformations, traverseGuide)
    {
    }

    public ExpandTransformation(ExpandOptions options)
        : base(options)
    {
    }

    public static Tensor Expand(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandTransformation(transformations).Transform(tensor);
    }

    protected override Tensor ExpandProduct(Product product, ITransformation[] transformations)
    {
        return ExpandUtils.ExpandProductOfSums(product, transformations);
    }

    public override string ToString(OutputFormat outputFormat)
    {
        return "Expand";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
