using System;

namespace NRedberry.Core.Tensors
{
    public static class TensorExtensions
    {
        public static Tensor Pow(this Tensor argument, Tensor power)
        {
            var pb = new PowerBuilder();
            pb.Put(argument);
            pb.Put(power);
            return pb.Build();
        }

        public static Tensor Multiply(params Tensor[] factors)
        {
            return ProductFactory.Factory.Create(factors);
        }

        public static Tensor Multiply(this Tensor left, Tensor right)
        {
            return Multiply(new []{ left, right });
        }
    }

    public static class Tensors
    {
        public static SimpleTensor ParseSimple(string expression)
        {
            var t = Parse(expression);
            if (t is SimpleTensor st)
            {
                return st;
            }

            throw new ArgumentException("Input tensor is not SimpleTensor.");
        }

        public static Tensor Parse(string expression)
        {
            return CC.Current.GetParseManager().Parse(expression);
        }
    }
}