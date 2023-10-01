using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public class ArcSin : ScalarFunction
{
    public ArcSin(Tensor argument) : base(argument)
    {
    }

    public override Tensor Derivative()
    {
        return TensorExtensions.Pow(
            TensorExtensions.Sum(
                Complex.One,
                TensorExtensions.Pow(Argument, Complex.Two)
            ),
            Complex.MinusOneHalf
        );
    }

    protected override string FunctionName()
    {
        return "ArcSin";
    }

    protected override int Hash()
    {
        return 92837 * Argument.GetHashCode();
    }

    public override TensorBuilder GetBuilder()
    {
        return new ScalarFunctionBuilder(ArcSinFactory.Factory);
    }

    public override TensorFactory GetFactory()
    {
        return ArcSinFactory.Factory;
    }

    public sealed class ArcSinFactory : ScalarFunctionFactory
    {
        public static readonly ArcSinFactory Factory = new ArcSinFactory();

        private ArcSinFactory()
        {
        }

        public override Tensor Create(Tensor arg)
        {
            if (arg is Sin)
                return arg[0];
            if (TensorUtils.IsZero(arg))
                return Complex.Zero;
            if (TensorUtils.IsNumeric(arg))
                return ComplexUtils.ArcSin((Complex)arg);
            return new ArcSin(arg);
        }
    }
}
