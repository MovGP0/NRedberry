using BigInteger = System.Numerics.BigInteger;
using NRedberry;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Factor;

public sealed class FactorOutNumber : TransformationToStringAble
{
    public static FactorOutNumber Instance { get; } = new();

    private FactorOutNumber()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        var iterator = new FromChildToParentIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is not Sum sum)
            {
                continue;
            }

            bool hasComposite = false;
            for (int i = sum.Size - 1; i >= 0; --i)
            {
                if (IsComposite(GetFactor(sum[i])))
                {
                    hasComposite = true;
                    break;
                }
            }

            if (hasComposite)
            {
                continue;
            }

            var nums = new BigInteger[sum.Size];
            var dens = new BigInteger[sum.Size];
            bool allImaginary = true;

            for (int i = sum.Size - 1; i >= 0; --i)
            {
                Complex factor = GetFactor(sum[i])!;
                Real real = factor.Real;
                if (real.IsZero())
                {
                    real = factor.Imaginary;
                }
                else
                {
                    allImaginary = false;
                }

                var rat = (Rational)real;
                nums[i] = rat.Numerator;
                dens[i] = rat.Denominator;
            }

            BigInteger commonNum = Gcd(nums);
            BigInteger commonDen = Gcd(dens);
            if (commonNum.Equals(BigInteger.One)
                && commonDen.Equals(BigInteger.One)
                && !allImaginary)
            {
                continue;
            }

            Complex commonFactor = new(new Rational(commonNum, commonDen));
            if (allImaginary)
            {
                commonFactor = commonFactor.Multiply(Complex.ImaginaryOne);
            }

            iterator.Set(Tensors.Tensors.Multiply(
                commonFactor,
                FastTensors.MultiplySumElementsOnFactor(sum, commonFactor.Reciprocal())));
        }

        return iterator.Result();
    }

    private static BigInteger Gcd(params BigInteger[] array)
    {
        BigInteger result = array[0];
        for (int i = 1; i < array.Length; ++i)
        {
            result = BigInteger.GreatestCommonDivisor(result, array[i]);
        }

        return result;
    }

    private static bool IsComposite(Complex? value)
    {
        return value is null
            || !(value.Real.IsZero() || value.Imaginary.IsZero())
            || value.IsOneOrMinusOne()
            || value.IsNumeric();
    }

    private static Complex? GetFactor(Tensor tensor)
    {
        if (tensor is Product product)
        {
            return product.Factor;
        }

        return tensor as Complex;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "FactorOutNumber";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
