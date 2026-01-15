using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandAllTransformation.
/// </summary>
public sealed class ExpandAllTransformation : AbstractExpandTransformation
{
    public static ExpandAllTransformation Instance { get; } = new();

    private ExpandAllTransformation()
        : base(Array.Empty<ITransformation>(), TraverseGuide.All)
    {
    }

    public ExpandAllTransformation(ITransformation[] transformations)
        : base(transformations, TraverseGuide.All)
    {
    }

    public ExpandAllTransformation(ITransformation[] transformations, TraverseGuide traverseGuide)
        : base(transformations, traverseGuide)
    {
    }

    public ExpandAllTransformation(ExpandOptions options)
        : base(options)
    {
    }

    public static Tensor Expand(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandAllTransformation(transformations).Transform(tensor);
    }

    protected override Tensor ExpandProduct(Product product, ITransformation[] transformations)
    {
        NumeratorDenominator numDen = NumeratorDenominator.GetNumeratorAndDenominator(
            product,
            NumeratorDenominator.IntegerDenominatorIndicator);
        Tensor denominator = numDen.Denominator;

        if (denominator is Product denominatorProduct)
        {
            denominator = ExpandUtils.ExpandProductOfSums(denominatorProduct, transformations);
        }

        bool denominatorExpanded = !ReferenceEquals(denominator, numDen.Denominator);
        denominator = Tensors.Tensors.Reciprocal(denominator);

        Tensor numerator = numDen.Numerator;
        Tensor result = Tensors.Tensors.Multiply(denominator, numerator);
        Tensor temp = result;
        if (result is Product resultProduct)
        {
            result = ExpandUtils.ExpandProductOfSums(resultProduct, transformations);
        }

        if (denominatorExpanded || !ReferenceEquals(result, temp))
        {
            return result;
        }

        return product;
    }

    public override string ToString(OutputFormat outputFormat)
    {
        return "ExpandAll";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
