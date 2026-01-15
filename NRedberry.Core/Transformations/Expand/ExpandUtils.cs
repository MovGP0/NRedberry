using NRedberry.Concurrent;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandUtils.
/// </summary>
public sealed class ExpandPairPort : IOutputPort<Tensor>
{
    private readonly Sum sum1;
    private readonly Sum sum2;
    private readonly Tensor[] factors;
    private long index;

    public ExpandPairPort(Sum first, Sum second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        sum1 = first;
        sum2 = second;
        factors = Array.Empty<Tensor>();
    }

    public ExpandPairPort(Sum first, Sum second, Tensor[] factors)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(factors);

        sum1 = first;
        sum2 = second;
        this.factors = factors;
    }

    public Tensor Take()
    {
        long total = (long)sum1.Size * sum2.Size;
        if (index >= total)
        {
            return null!;
        }

        int firstIndex = (int)(index / sum2.Size);
        int secondIndex = (int)(index % sum2.Size);
        index++;
        if (factors.Length == 0)
        {
            return Tensors.Tensors.Multiply(sum1[firstIndex], sum2[secondIndex]);
        }

        return Tensors.Tensors.Multiply(
            TensorArrayUtils.AddAll(factors, sum1[firstIndex], sum2[secondIndex]));
    }
}

public static class ExpandUtils
{
    public static Tensor ExpandPairOfSums(Sum first, Sum second, Tensor[] factors, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(factors);
        ArgumentNullException.ThrowIfNull(transformations);

        ExpandPairPort port = new(first, second, factors);
        SumBuilder sum = new(first.Size * second.Size);
        Tensor? term;
        while ((term = port.Take()) is not null)
        {
            sum.Put(Apply(transformations, term));
        }

        return sum.Build();
    }

    public static Tensor ExpandPairOfSums(Sum first, Sum second, ITransformation[] transformations)
    {
        return ExpandPairOfSums(first, second, Array.Empty<Tensor>(), transformations);
    }

    public static Tensor ExpandProductOfSums(Product product, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(transformations);

        Tensor indexless = GetIndexlessSubProduct(product);
        Tensor data = GetDataSubProduct(product);
        bool expandIndexless = false;
        bool expandData = false;
        bool containsIndexlessSumNeededExpand = false;
        if (indexless is Sum && SumContainsIndexed(indexless))
        {
            containsIndexlessSumNeededExpand = true;
            expandIndexless = true;
            expandData = true;
        }

        if (indexless is Product indexlessProduct)
        {
            foreach (Tensor t in indexlessProduct)
            {
                if (t is Sum)
                {
                    if (SumContainsIndexed(t))
                    {
                        containsIndexlessSumNeededExpand = true;
                        expandData = true;
                        expandIndexless = true;
                        break;
                    }

                    expandIndexless = true;
                }
            }
        }

        if (!expandData)
        {
            if (data is Sum)
            {
                expandData = true;
            }
            else if (data is Product dataProduct)
            {
                foreach (Tensor t in dataProduct)
                {
                    if (t is Sum)
                    {
                        expandData = true;
                        break;
                    }
                }
            }
        }

        if (!expandData && !expandIndexless)
        {
            return product;
        }

        if (!expandData)
        {
            return Tensors.Tensors.Multiply(ExpandProductOfSums1(indexless, transformations, false), data);
        }

        if (!expandIndexless)
        {
            Tensor newData = ExpandProductOfSums1(data, transformations, true);
            if (newData is Sum sum)
            {
                return FastTensors.MultiplySumElementsOnFactorAndExpand(sum, indexless);
            }

            return ExpandIndexlessSubproduct.Transform(Tensors.Tensors.Multiply(indexless, newData));
        }

        if (!containsIndexlessSumNeededExpand)
        {
            indexless = ExpandProductOfSums1(indexless, transformations, false);
            data = ExpandProductOfSums1(data, transformations, true);
        }
        else
        {
            List<Tensor> dataList;
            if (data is Product dataProduct)
            {
                dataList = new List<Tensor>(dataProduct.ToArray());
            }
            else
            {
                dataList = new List<Tensor> { data };
            }

            if (indexless is Sum)
            {
                dataList.Add(indexless);
                indexless = Complex.One;
                data = ExpandProductOfSums1(dataList, transformations, true);
            }
            else
            {
                var indexlessList = new List<Tensor>(indexless.Size);
                expandIndexless = false;
                foreach (Tensor inTensor in (Product)indexless)
                {
                    if (SumContainsIndexed(inTensor))
                    {
                        dataList.Add(inTensor);
                    }
                    else
                    {
                        if (inTensor is Sum)
                        {
                            expandIndexless = true;
                        }

                        indexlessList.Add(inTensor);
                    }
                }

                if (expandIndexless)
                {
                    indexless = ExpandProductOfSums1(indexlessList, transformations, false);
                }
                else
                {
                    indexless = Tensors.Tensors.Multiply(indexlessList.ToArray());
                }

                data = ExpandProductOfSums1(dataList, transformations, true);
            }
        }

        if (data is Sum dataSum)
        {
            return FastTensors.MultiplySumElementsOnFactorAndExpand(dataSum, indexless);
        }

        return Tensors.Tensors.Multiply(indexless, data);
    }

    public static Tensor Apply(ITransformation[] transformations, Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        ArgumentNullException.ThrowIfNull(tensor);

        foreach (ITransformation transformation in transformations)
        {
            tensor = transformation.Transform(tensor);
        }

        return tensor;
    }

    public static Tensor ExpandProductOfSums(IEnumerable<Tensor> tensor, ITransformation[] transformations, bool indexed)
    {
        return ExpandProductOfSums1(tensor, transformations, indexed);
    }

    public static Tensor ExpandProductOfSums1(IEnumerable<Tensor> tensor, ITransformation[] transformations, bool indexed)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(transformations);

        ITransformation[] transformations1 = indexed
            ? PrependExpandIndexlessSubproduct(transformations)
            : transformations;
        int capacity = 10;
        bool isTensor = tensor is Tensor;
        if (tensor is Tensor tensorValue)
        {
            if (tensorValue is not Product)
            {
                return tensorValue;
            }

            capacity = tensorValue.Size;
        }

        List<Tensor> nonSums = new(capacity);
        List<Sum> sums = new(capacity);
        foreach (Tensor t in tensor)
        {
            if (t is Sum sum)
            {
                sums.Add(sum);
            }
            else
            {
                nonSums.Add(t);
            }
        }

        if (sums.Count == 0)
        {
            if (isTensor)
            {
                return (Tensor)tensor;
            }

            return Tensors.Tensors.Multiply(nonSums.ToArray());
        }

        if (sums.Count == 1)
        {
            if (indexed)
            {
                return MultiplySumElementsOnFactorAndExpand(
                    sums[0],
                    Tensors.Tensors.Multiply(nonSums.ToArray()),
                    transformations);
            }

            return MultiplySumElementsOnFactor(
                sums[0],
                Tensors.Tensors.Multiply(nonSums.ToArray()),
                transformations);
        }

        Tensor? baseTensor = sums[0];
        for (int i = 1, size = sums.Count; ; ++i)
        {
            if (i == size - 1)
            {
                if (baseTensor is null)
                {
                    if (indexed)
                    {
                        return MultiplySumElementsOnFactorAndExpand(
                            sums[i],
                            Tensors.Tensors.Multiply(nonSums.ToArray()),
                            transformations);
                    }

                    return MultiplySumElementsOnFactor(
                        sums[i],
                        Tensors.Tensors.Multiply(nonSums.ToArray()),
                        transformations);
                }

                return ExpandPairOfSums((Sum)baseTensor, sums[i], nonSums.ToArray(), transformations1);
            }

            if (baseTensor is null)
            {
                baseTensor = sums[i];
                continue;
            }

            baseTensor = ExpandPairOfSums((Sum)baseTensor, sums[i], transformations1);
            if (baseTensor is not Sum)
            {
                nonSums.Add(baseTensor);
                baseTensor = null;
            }
        }
    }

    public static Tensor MultiplySumElementsOnFactorAndExpand(Sum sum, Tensor factor, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(sum);
        ArgumentNullException.ThrowIfNull(factor);
        ArgumentNullException.ThrowIfNull(transformations);

        if (TensorUtils.IsZero(factor))
        {
            return Complex.Zero;
        }

        if (TensorUtils.IsOne(factor))
        {
            return sum;
        }

        if (factor is Sum && factor.Indices.Size() != 0)
        {
            throw new ArgumentException("Factor must be indexless.", nameof(factor));
        }

        if (TensorUtils.HaveIndicesIntersections(sum, factor))
        {
            SumBuilder sb = new(sum.Size);
            foreach (Tensor t in sum)
            {
                sb.Put(Apply(transformations, ExpandIndexlessSubproduct.Transform(Tensors.Tensors.Multiply(t, factor))));
            }

            return sb.Build();
        }

        return Apply(transformations, FastTensors.MultiplySumElementsOnFactorAndExpand(sum, factor));
    }

    public static Tensor MultiplySumElementsOnFactor(Sum sum, Tensor factor, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(sum);
        ArgumentNullException.ThrowIfNull(factor);
        ArgumentNullException.ThrowIfNull(transformations);

        if (TensorUtils.IsZero(factor))
        {
            return Complex.Zero;
        }

        if (TensorUtils.IsOne(factor))
        {
            return sum;
        }

        if (TensorUtils.HaveIndicesIntersections(sum, factor))
        {
            SumBuilder sb = new(sum.Size);
            foreach (Tensor t in sum)
            {
                sb.Put(Apply(transformations, Tensors.Tensors.Multiply(t, factor)));
            }

            return sb.Build();
        }

        return Apply(transformations, FastTensors.MultiplySumElementsOnFactor(sum, factor));
    }

    public static bool IsExpandablePower(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return tensor is Power && tensor[0] is Sum && TensorUtils.IsInteger(tensor[1]);
    }

    public static bool SumContainsIndexed(Tensor tensor)
    {
        if (tensor is not Sum sum)
        {
            return false;
        }

        foreach (Tensor s in sum)
        {
            if (s.Indices.Size() != 0)
            {
                return true;
            }
        }

        return false;
    }

    public static Tensor ExpandSymbolicPower(Sum argument, int power, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(argument);
        ArgumentNullException.ThrowIfNull(transformations);

        int i;
        Tensor temp = argument;
        for (i = power - 1; i >= 1; --i)
        {
            temp = ExpandPairOfSums((Sum)temp, argument, transformations);
            if (temp is not Sum)
            {
                temp = Tensors.Tensors.Multiply(temp, Apply(transformations, Tensors.Tensors.Pow(argument, i - 1)));
                break;
            }
        }

        return temp;
    }

    public static Tensor ExpandPower(Sum argument, int power, int[] forbiddenIndices, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(argument);
        ArgumentNullException.ThrowIfNull(forbiddenIndices);
        ArgumentNullException.ThrowIfNull(transformations);

        int i;
        Tensor temp = argument;
        HashSet<int> forbidden = new(forbiddenIndices);
        HashSet<int> argIndices = TensorUtils.GetAllIndicesNamesT(argument);
        forbidden.EnsureCapacity(forbidden.Count + argIndices.Count * power);
        forbidden.UnionWith(argIndices);
        for (i = power - 1; i >= 1; --i)
        {
            Tensor renamed = ApplyIndexMapping.RenameDummy(argument, forbidden.ToArray(), forbidden);
            temp = ExpandPairOfSums((Sum)temp, (Sum)renamed, transformations);
            if (temp is not Sum)
            {
                temp = Tensors.Tensors.Multiply(temp, Apply(transformations, Tensors.Tensors.Pow(argument, i - 1)));
                break;
            }
        }

        return temp;
    }

    public static ITransformation ExpandIndexlessSubproduct { get; } = new ExpandIndexlessSubproductTransformation();

    private static ITransformation[] PrependExpandIndexlessSubproduct(ITransformation[] transformations)
    {
        var result = new ITransformation[transformations.Length + 1];
        result[0] = ExpandIndexlessSubproduct;
        Array.Copy(transformations, 0, result, 1, transformations.Length);
        return result;
    }

    private static Tensor GetIndexlessSubProduct(Product product)
    {
        if (product.IndexlessData.Length == 0)
        {
            return product.Factor;
        }

        if (product.Factor == Complex.One && product.IndexlessData.Length == 1)
        {
            return product.IndexlessData[0];
        }

        if (product.Factor == Complex.One)
        {
            return Tensors.Tensors.Multiply(product.IndexlessData);
        }

        Tensor[] factorsWithScalar = new Tensor[product.IndexlessData.Length + 1];
        factorsWithScalar[0] = product.Factor;
        Array.Copy(product.IndexlessData, 0, factorsWithScalar, 1, product.IndexlessData.Length);
        return Tensors.Tensors.Multiply(factorsWithScalar);
    }

    private static Tensor GetDataSubProduct(Product product)
    {
        if (product.Data.Length == 0)
        {
            return Complex.One;
        }

        if (product.Data.Length == 1)
        {
            return product.Data[0];
        }

        return Tensors.Tensors.Multiply(product.Data);
    }
}

internal sealed class ExpandIndexlessSubproductTransformation : ITransformation
{
    public Tensor Transform(Tensor tensor)
    {
        if (tensor is not Product product)
        {
            return tensor;
        }

        if (product.IndexlessData.Length == 0)
        {
            return tensor;
        }

        Tensor indexless;
        if (product.IndexlessData.Length == 0)
        {
            indexless = product.Factor;
        }
        else if (product.Factor == Complex.One && product.IndexlessData.Length == 1)
        {
            indexless = product.IndexlessData[0];
        }
        else if (product.Factor == Complex.One)
        {
            indexless = Tensors.Tensors.Multiply(product.IndexlessData);
        }
        else
        {
            Tensor[] factorsWithScalar = new Tensor[product.IndexlessData.Length + 1];
            factorsWithScalar[0] = product.Factor;
            Array.Copy(product.IndexlessData, 0, factorsWithScalar, 1, product.IndexlessData.Length);
            indexless = Tensors.Tensors.Multiply(factorsWithScalar);
        }

        return Tensors.Tensors.Multiply(ExpandTransformation.Expand(indexless), Tensors.Tensors.Multiply(product.Data));
    }
}
