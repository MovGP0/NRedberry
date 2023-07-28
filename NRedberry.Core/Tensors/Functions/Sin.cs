using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public sealed class Sin : ScalarFunction
{
    public Sin(Tensor argument) : base(argument) { }

    public override Tensor Derivative()
    {
        return new Cos(Argument);
    }

    protected override int Hash()
    {
        return 7 * Argument.GetHashCode();
    }

    protected override string FunctionName()
    {
        return "Sin";
    }

    public override TensorBuilder GetBuilder()
    {
        return new ScalarFunctionBuilder(SinFactory.Factory);
    }

    public override TensorFactory GetFactory()
    {
        return SinFactory.Factory;
    }

    public sealed class SinFactory : ScalarFunctionFactory
    {
        public static readonly SinFactory Factory = new();

        private SinFactory() { }

        public override Tensor Create1(Tensor arg)
        {
            if (arg is ArcSin)
                return arg[0];
            if (TensorUtils.IsZero(arg))
                return Complex.Zero;
            if (TensorUtils.IsNumeric(arg))
                return ComplexUtils.Sin((Complex)arg);
            return new Sin(arg);
        }
    }
}