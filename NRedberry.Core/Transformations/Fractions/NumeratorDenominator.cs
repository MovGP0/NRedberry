using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Transformations.Fractions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.fractions.NumeratorDenominator.
/// </summary>
public sealed class NumeratorDenominator
{
    public Tensor Numerator { get; }
    public Tensor Denominator { get; }

    private NumeratorDenominator(Tensor numerator, Tensor denominator)
    {
        throw new NotImplementedException();
    }

    public static NumeratorDenominator Of(Tensor numerator, Tensor denominator)
    {
        throw new NotImplementedException();
    }

    public static NumeratorDenominator GetNumeratorAndDenominator(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static NumeratorDenominator GetNumeratorAndDenominator(Tensor tensor, IIndicator<Tensor> denominatorIndicator)
    {
        throw new NotImplementedException();
    }

    public static IList<Tensor> ExpandPower(Tensor power)
    {
        throw new NotImplementedException();
    }

    public static IIndicator<Tensor> DefaultDenominatorIndicator => throw new NotImplementedException();

    public static IIndicator<Tensor> IntegerDenominatorIndicator => throw new NotImplementedException();
}
