using NRedberry.Tensors;
using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandNumeratorTransformation.
/// </summary>
public sealed class ExpandNumeratorTransformation : AbstractExpandNumeratorDenominatorTransformation
{
    public static ExpandNumeratorTransformation Instance { get; } = new();

    private ExpandNumeratorTransformation()
    {
    }

    public ExpandNumeratorTransformation(ITransformation[] transformations)
        : base(transformations)
    {
    }

    public ExpandNumeratorTransformation(ExpandOptions options)
        : base(options)
    {
    }

    public static Tensor Expand(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandNumeratorTransformation(transformations).Transform(tensor);
    }

    protected override Tensor ExpandProduct(Tensor tensor)
    {
        NumeratorDenominator numDen = NumeratorDenominator.GetNumeratorAndDenominator(
            tensor,
            NumeratorDenominator.IntegerDenominatorIndicator);
        Tensor numerator = ExpandTransformation.Expand(numDen.Numerator, transformations);
        if (ReferenceEquals(numDen.Numerator, numerator))
        {
            return tensor;
        }

        return Tensors.Tensors.Multiply(numerator, Tensors.Tensors.Reciprocal(numDen.Denominator));
    }

    public override string ToString(OutputFormat outputFormat)
    {
        return "ExpandNumerator";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
