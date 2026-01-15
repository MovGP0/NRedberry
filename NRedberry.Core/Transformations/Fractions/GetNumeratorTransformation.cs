using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Fractions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.fractions.GetNumeratorTransformation.
/// </summary>
public sealed class GetNumeratorTransformation : ITransformation, TransformationToStringAble
{
    public static GetNumeratorTransformation Instance { get; } = new();

    private GetNumeratorTransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return NumeratorDenominator.GetNumeratorAndDenominator(tensor).Numerator;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "Numerator";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
