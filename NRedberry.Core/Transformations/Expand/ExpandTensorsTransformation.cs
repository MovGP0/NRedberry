using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Numbers;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Port of cc.redberry.core.transformations.expand.ExpandTensorsTransformation.
/// </summary>
public sealed class ExpandTensorsTransformation : TransformationToStringAble
{
    public static ExpandTensorsTransformation Instance { get; } = new();

    private readonly bool leaveScalars;
    private readonly ITransformation[] transformations = [];
    private readonly TraverseGuide traverseGuide = null!;

    private ExpandTensorsTransformation()
        : this(Array.Empty<ITransformation>())
    {
    }

    public ExpandTensorsTransformation(params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        leaveScalars = false;
        this.transformations = transformations;
        traverseGuide = TraverseGuide.All;
    }

    public ExpandTensorsTransformation(bool leaveScalars, params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        this.leaveScalars = leaveScalars;
        this.transformations = transformations;
        traverseGuide = TraverseGuide.All;
    }

    public ExpandTensorsTransformation(bool leaveScalars, ITransformation[] transformations, TraverseGuide traverseGuide)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        ArgumentNullException.ThrowIfNull(traverseGuide);
        this.leaveScalars = leaveScalars;
        this.transformations = transformations;
        this.traverseGuide = traverseGuide;
    }

    public ExpandTensorsTransformation(ExpandTensorsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        leaveScalars = options.LeaveScalars;
        transformations = [options.Simplifications ?? Transformation.Identity];
        traverseGuide = options.TraverseGuide ?? TraverseGuide.All;
    }

    public static Tensor Expand(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        return new ExpandTensorsTransformation(transformations).Transform(tensor);
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        SubstitutionIterator iterator = new(tensor, traverseGuide);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Product product)
            {
                iterator.UnsafeSet(ExpandProduct(product));
            }
        }

        return iterator.Result();
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "ExpandTensors";
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    private Tensor ExpandProduct(Product product)
    {
        List<Tensor> ns;
        List<Sum> sums;
        if (leaveScalars)
        {
            ProductContent content = product.Content;
            ns = new List<Tensor>(content.Size);
            ns.Add(GetIndexlessSubProduct(product));
            sums = new List<Sum>(content.Size);
            foreach (Tensor t in content)
            {
                if (t is Sum sum)
                {
                    sums.Add(sum);
                }
                else
                {
                    ns.Add(t);
                }
            }
        }
        else
        {
            ns = new List<Tensor>(product.Size);
            sums = new List<Sum>(product.Size);
            foreach (Tensor t in product)
            {
                if (ExpandUtils.SumContainsIndexed(t))
                {
                    sums.Add((Sum)t);
                }
                else
                {
                    ns.Add(t);
                }
            }
        }

        if (sums.Count == 0)
        {
            return product;
        }

        if (sums.Count == 1)
        {
            return ExpandUtils.MultiplySumElementsOnFactor(sums[0], Tensors.Tensors.Multiply(ns), transformations);
        }

        Tensor? baseTensor = sums[0];
        for (int i = 1, size = sums.Count; ; ++i)
        {
            if (i == size - 1)
            {
                if (baseTensor == null)
                {
                    return ExpandUtils.MultiplySumElementsOnFactor(sums[i], Tensors.Tensors.Multiply(ns), transformations);
                }

                return ExpandUtils.ExpandPairOfSums((Sum)baseTensor, sums[i], ns.ToArray(), transformations);
            }

            if (baseTensor == null)
            {
                baseTensor = sums[i];
                continue;
            }

            baseTensor = ExpandUtils.ExpandPairOfSums((Sum)baseTensor, sums[i], transformations);
            if (baseTensor is not Sum)
            {
                ns.Add(baseTensor);
                baseTensor = null;
            }
        }
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
}
