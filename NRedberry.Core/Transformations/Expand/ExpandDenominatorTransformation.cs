using NRedberry.Tensors;
using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandDenominatorTransformation.
/// </summary>
public sealed class ExpandDenominatorTransformation : AbstractExpandNumeratorDenominatorTransformation
{
    public static ExpandDenominatorTransformation Instance { get; } = new();

    private ExpandDenominatorTransformation()
    {
    }

    public ExpandDenominatorTransformation(ITransformation[] transformations)
        : base(transformations)
    {
    }

    public ExpandDenominatorTransformation(ExpandOptions options)
        : base(options)
    {
    }

    public static Tensor Expand(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandDenominatorTransformation(transformations).Transform(tensor);
    }

    protected override Tensor ExpandProduct(Tensor tensor)
    {
        NumeratorDenominator numDen = NumeratorDenominator.GetNumeratorAndDenominator(
            tensor,
            NumeratorDenominator.IntegerDenominatorIndicator);
        Tensor denominator = ExpandTransformation.Expand(numDen.Denominator, transformations);
        if (ReferenceEquals(numDen.Denominator, denominator))
        {
            return tensor;
        }

        return Tensors.Tensors.Multiply(numDen.Numerator, Tensors.Tensors.Reciprocal(denominator));
    }

    public override string ToString(OutputFormat outputFormat)
    {
        return "ExpandDenominator";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
