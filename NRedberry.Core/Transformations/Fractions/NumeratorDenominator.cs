using NRedberry.Core.Utils;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Fractions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.fractions.NumeratorDenominator.
/// </summary>
public sealed class NumeratorDenominator
{
    public Tensor Numerator { get; }
    public Tensor Denominator { get; }

    private NumeratorDenominator(Tensor numerator, Tensor denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public static NumeratorDenominator Of(Tensor numerator, Tensor denominator)
    {
        ArgumentNullException.ThrowIfNull(numerator);
        ArgumentNullException.ThrowIfNull(denominator);
        return new NumeratorDenominator(numerator, denominator);
    }

    public static NumeratorDenominator GetNumeratorAndDenominator(Tensor tensor)
    {
        return GetNumeratorAndDenominator(tensor, DefaultDenominatorIndicator);
    }

    public static NumeratorDenominator GetNumeratorAndDenominator(Tensor tensor, IIndicator<Tensor> denominatorIndicator)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(denominatorIndicator);

        if (denominatorIndicator.Is(tensor))
        {
            return new NumeratorDenominator(Complex.One, Tensors.Tensors.Reciprocal(tensor));
        }

        if (tensor is Power && tensor[1] is Sum)
        {
            IList<Tensor> powers = ExpandPower(tensor);
            TensorBuilder denominator = new ScalarsBackedProductBuilder();
            TensorBuilder numerator = new ScalarsBackedProductBuilder();
            foreach (Tensor power in powers)
            {
                if (denominatorIndicator.Is(power))
                {
                    denominator.Put(Tensors.Tensors.Reciprocal(power));
                }
                else
                {
                    numerator.Put(power);
                }
            }

            return new NumeratorDenominator(numerator.Build(), denominator.Build());
        }

        if (tensor is not Product)
        {
            return new NumeratorDenominator(tensor, Complex.One);
        }

        TensorBuilder denominators = new ScalarsBackedProductBuilder();
        Tensor temp = tensor;
        for (int i = tensor.Size - 1; i >= 0; --i)
        {
            Tensor t = tensor[i];
            if (denominatorIndicator.Is(t))
            {
                Tensor exponent = Tensors.Tensors.Negate(t[1]);
                denominators.Put(Tensors.Tensors.Pow(t[0], exponent));
                if (temp is Product tempProduct)
                {
                    temp = tempProduct.Remove(i);
                }
                else
                {
                    temp = Complex.One;
                }
            }
        }

        return new NumeratorDenominator(temp, denominators.Build());
    }

    public static IList<Tensor> ExpandPower(Tensor power)
    {
        ArgumentNullException.ThrowIfNull(power);

        var powers = new List<Tensor>(power[1].Size);
        foreach (Tensor exponent in power[1])
        {
            powers.Add(Tensors.Tensors.Pow(power[0], exponent));
        }

        return powers;
    }

    public static IIndicator<Tensor> DefaultDenominatorIndicator { get; } = new DefaultDenominatorIndicator();

    public static IIndicator<Tensor> IntegerDenominatorIndicator { get; } = new IntegerDenominatorIndicator();
}

internal sealed class DefaultDenominatorIndicator : IIndicator<Tensor>
{
    public bool Is(Tensor tensor)
    {
        if (tensor is Power)
        {
            Tensor exponent = tensor[1];
            if (exponent is Complex complex)
            {
                return NumberUtils.IsRealNegative(complex);
            }

            if (exponent is Product product)
            {
                return product.Factor.IsMinusOne();
            }
        }

        return false;
    }
}

internal sealed class IntegerDenominatorIndicator : IIndicator<Tensor>
{
    public bool Is(Tensor tensor)
    {
        return TensorUtils.IsNegativeIntegerPower(tensor);
    }
}
