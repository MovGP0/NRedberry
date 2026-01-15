using NRedberry.Indices;
using NRedberry.Numbers;

namespace NRedberry.Tensors;

/// <summary>
/// Representation of mathematical power <i>A^B</i>.
/// </summary>
public sealed class Power : Tensor
{
    private readonly Tensor _argument;
    private readonly Tensor _power;

    public Power(Tensor argument, Tensor power)
    {
        ArgumentNullException.ThrowIfNull(argument);
        ArgumentNullException.ThrowIfNull(power);

        _argument = argument;
        _power = power;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (37 * _argument.GetHashCode()) + _power.GetHashCode();
        }
    }

    public override Indices.Indices Indices => IndicesFactory.EmptyIndices;

    public override Tensor this[int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                    return _argument;

                case 1:
                    return _power;

                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, "must be 0 or 1");
            }
        }
    }

    public override int Size => 2;

    public override string ToString(OutputFormat outputFormat)
    {
        if (outputFormat.Is(OutputFormat.WolframMathematica))
        {
            return _argument.ToStringWith<Power>(outputFormat) + "^" + _power.ToStringWith<Power>(outputFormat);
        }

        if (outputFormat.Is(OutputFormat.LaTeX))
        {
            if (TensorUtils.IsRealNegativeNumber(_power))
            {
                string suffix = TensorUtils.IsMinusOne(_power)
                    ? string.Empty
                    : "^" + ((Complex)_power).Abs().ToString(outputFormat);

                return "\\frac{1}{" + _argument.ToStringWith<Power>(outputFormat) + suffix + "}";
            }

            return _argument.ToStringWith<Power>(outputFormat) + "^{" + _power.ToString(outputFormat) + "}";
        }

        return _argument.ToStringWith<Power>(outputFormat) + "**" + _power.ToStringWith<Power>(outputFormat);
    }

    public override TensorBuilder GetBuilder()
    {
        return new PowerBuilder();
    }

    public override TensorFactory GetFactory()
    {
        return PowerFactory.Factory;
    }
}
