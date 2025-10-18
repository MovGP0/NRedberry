using NRedberry.Core.Indices;
using NRedberry.Core.Numbers;
using NotImplementedException = sun.reflect.generics.reflectiveObjects.NotImplementedException;

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
        if (tensor.Indices.GetFree().Count() == 0)
        {
            // case 2*a*b*c
            return new SplitNumbers(tensor, Complex.One);
        }

        //case 2*a*g_mn*g_cd
        Tensor summand = null!;
        Tensor? factor;

        if (tensor is Product product)
        {
            ProductContent content = product.Content;
            factor = content.NonScalar;
            Tensor[] scalars = content.Scalars;

            int dataLength = factor is Product prod
                ? product.Data.Length - prod.Data.Length
                : product.Data.Length == 0
                    ? 0
                    : product.Data.Length - 1;

            if (factor == null)
            {
                factor = Complex.One;
            }
            if (dataLength == 0)
            {
                if (product.IndexlessData.Length == 0)
                {
                    summand = product.Factor;
                }
                else if (product.IndexlessData.Length == 1 && product.Factor == Complex.One)
                {
                    summand = product.IndexlessData[0];
                }
                else if (product.Factor.IsMinusOne() && product.IndexlessData.Length == 1 && product.IndexlessData[0] is Sum s)
                {
                    Tensor[] sumData = (Tensor[])s.Data.Clone();
                    for (int i = sumData.Length - 1; i >= 0; --i)
                    {
                        sumData[i] = Tensors.Negate(sumData[i]);
                    }
                    summand = new Sum(s.Indices, sumData, s.GetHashCode());
                }
                else
                {
                    summand = new Product(product.Factor, product.IndexlessData, [], ProductContent.EmptyInstance, IndicesFactory.EmptyIndices);
                }
            }
            else if (dataLength == 1 && product.IndexlessData.Length == 0 && product.Factor == Complex.One)
            {
                summand = scalars[0];
            }
            else
            {
                Tensor[] data = new Tensor[dataLength];
                var ib = new IndicesBuilder();
                dataLength = -1;
                foreach (Tensor t in scalars)
                {
                    if (t is Product)
                    {
                        foreach (Tensor d in t)
                        {
                            data[++dataLength] = d;
                            ib.Append(d);
                        }
                    }
                    else
                    {
                        data[++dataLength] = t;
                        ib.Append(t);
                    }
                }

                System.Diagnostics.Debug.Assert(dataLength == data.Length - 1);
                Array.Sort(data);
                summand = new Product(product.Factor, product.IndexlessData, data, null, ib.Indices);
            }
        }
        else
        {
            summand = Complex.One;
            factor = tensor;
        }

        return new Split(factor, summand);
    }

    public static Split SplitIndexless(Tensor tensor)
    {
        if (tensor.Indices.Count() == 0) //case 2*a*b*c
        {
            Complex complex;
            Tensor factor;

            if (tensor is Product product)
            {
                complex = product.Factor;

                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            else
            {
                summand = Complex.One;
                factor = tensor;
            }

            return new Split(factor, summand);
        }
    }

    public override string ToString() => $"{Summand} * {Factor}";
}