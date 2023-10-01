using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors;

public class Split
{
    public Tensor Factor { get; }
    public Tensor Summand { get; }

    public Split(Tensor factor, Tensor summand)
    {
        Factor = factor;
        Summand = summand;
    }

    public virtual TensorBuilder GetBuilder()
    {
        TensorBuilder builder = new SumBuilder();
        builder.Put(Summand);
        return builder;
    }

    public static Split SplitScalars(Tensor tensor)
    {
        if (tensor.Indices.GetFree().Count == 0) //case 2*a*b*c
        {
            return new SplitNumbers(tensor, Complex.One);
        }
        else //case 2*a*g_mn*g_cd
        {
            Tensor summand;
            Tensor factor;

            if (tensor is Product product)
            {
                ProductContent content = product.GetContent();
                factor = content.GetNonScalar();
                Tensor[] scalars = content.GetScalars();
                int dataLength = factor is Product prod
                    ? product.Data.Length - prod.Data.Length
                    : product.Data.Length == 0 ? 0 : product.Data.Length - 1;

                if (factor == null)
                    factor = Complex.One;

                // ... other logic for summand
                // You will need to implement logic similar to your Java code to fill in 'summand'

            }
            else
            {
                summand = Complex.One;
                factor = tensor;
            }

            return new Split(factor, summand);
        }
    }

    public static Split SplitIndexless(Tensor tensor)
    {
        if (tensor.Indices.Count == 0) //case 2*a*b*c
        {
            Complex complex;
            Tensor factor;

            if (tensor is Product product)
            {
                complex = product.Factor;

                // ... other logic for 'factor'
                // You will need to implement logic similar to your Java code to fill in 'factor'
            }
            else
            {
                complex = Complex.One;
                factor = tensor;
            }

            return new SplitNumbers(factor, complex);
        }
        else //case 2*a*g_mn*g_cd
        {
            Tensor summand;
            Tensor factor;

            if (tensor is Product product)
            {
                // ... logic for 'summand' and 'factor'
                // You will need to implement logic similar to your Java code to fill in 'summand' and 'factor'
            }
            else
            {
                summand = Complex.One;
                factor = tensor;
            }

            return new Split(factor, summand);
        }
    }

    public override string ToString()
    {
        return $"{Summand} * {Factor}";
    }
}