using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Fractions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.fractions.GetDenominatorTransformation.
/// </summary>
public sealed class GetDenominatorTransformation : ITransformation, TransformationToStringAble
{
    public static GetDenominatorTransformation Instance { get; } = new();

    private GetDenominatorTransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return NumeratorDenominator.GetNumeratorAndDenominator(tensor).Denominator;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "Denominator";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
