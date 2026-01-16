using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/FastTensors.java
 */

public static class FastTensors
{
    public static Tensor MultiplySumElementsOnFactorAndResolveDummies(Sum sum, Tensor factor)
    {
        Tensor[] pair = Tensors.ResolveDummy(sum, factor);
        int i = pair[0] is Sum ? 0 : 1;
        return MultiplySumElementsOnFactor((Sum)pair[i], pair[1 - i], Array.Empty<ITransformation>());
    }

    public static Tensor MultiplySumElementsOnFactor(Sum sum, Tensor factor)
    {
        return MultiplySumElementsOnFactor(sum, factor, Array.Empty<ITransformation>());
    }

    public static Tensor MultiplySumElementsOnFactorAndExpand(Sum sum, Tensor factor)
    {
        if (factor is Sum && factor.Indices.Size() != 0)
        {
            throw new ArgumentException();
        }

        return MultiplySumElementsOnFactor(sum, factor, [ExpandUtils.ExpandIndexlessSubproduct]);
    }

    [Obsolete("Very unsafe method without checks.")]
    public static Tensor MultiplySumElementsOnFactors(Sum sum)
    {
        Tensor[] newSumData = new Tensor[sum.Size];
        for (int i = newSumData.Length - 1; i >= 0; --i)
        {
            newSumData[i] = Tensors.Multiply(CC.GenerateNewSymbol(), sum[i]);
        }

        return new Sum(newSumData, IndicesFactory.Create(newSumData[0].Indices.GetFree()));
    }

    private static Tensor MultiplySumElementsOnFactor(
        Sum sum,
        Tensor factor,
        ITransformation[] transformations)
    {
        if (TensorUtils.IsZero(factor))
        {
            return Complex.Zero;
        }

        if (TensorUtils.IsOne(factor))
        {
            return sum;
        }

        if (TensorUtils.HaveIndicesIntersections(sum, factor)
            || (sum.Indices.Size() == 0 && factor.Indices.Size() != 0))
        {
            return MultiplyWithBuilder(sum, factor, transformations);
        }

        return MultiplyWithFactory(sum, factor, transformations);
    }

    private static Tensor MultiplyWithBuilder(
        Sum sum,
        Tensor factor,
        params ITransformation[] transformations)
    {
        var builder = new SumBuilder(sum.Size);
        foreach (Tensor t in sum)
        {
            builder.Put(Transformation.ApplySequentially(Tensors.Multiply(t, factor), transformations));
        }

        return builder.Build();
    }

    private static Tensor MultiplyWithFactory(
        Sum sum,
        Tensor factor,
        params ITransformation[] transformations)
    {
        var newSumData = new System.Collections.Generic.List<Tensor>(sum.Size);
        bool reduced = false;
        for (int i = sum.Size - 1; i >= 0; --i)
        {
            Tensor temp = Transformation.ApplySequentially(
                Tensors.Multiply(factor, sum[i]),
                transformations);
            if (!TensorUtils.IsZero(temp))
            {
                newSumData.Add(temp);
                if (!reduced && IsReduced(sum[i], factor, temp))
                {
                    reduced = true;
                }
            }
        }

        if (newSumData.Count == 0)
        {
            return Complex.Zero;
        }

        if (newSumData.Count == 1)
        {
            return newSumData[0];
        }

        Tensor[] data = newSumData.ToArray();
        if (reduced)
        {
            return SumFactory.Factory.Create(data);
        }

        return new Sum(data, IndicesFactory.Create(newSumData[0].Indices.GetFree()));
    }

    private static bool IsReduced(Tensor initial, Tensor factor, Tensor result)
    {
        return !initial.GetType().Equals(result.GetType());
    }
}
