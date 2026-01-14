using NRedberry.Indices;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Transformations.Symmetrization;
using NRedberry.Utils;

namespace NRedberry.Tensors;

public abstract class AbstractSumBuilder : TensorBuilder
{
    protected readonly Dictionary<int, List<FactorNode>> Summands;
    protected Complex Complex = Complex.Zero;
    protected Indices.Indices? Indices;
    protected int[]? SortedNames;

    protected AbstractSumBuilder()
        : this(7)
    {
    }

    protected AbstractSumBuilder(int initialCapacity)
    {
        Summands = new Dictionary<int, List<FactorNode>>(initialCapacity);
    }

    protected AbstractSumBuilder(
        Dictionary<int, List<FactorNode>> summands,
        Complex complex,
        Indices.Indices? indices,
        int[]? sortedNames)
    {
        ArgumentNullException.ThrowIfNull(summands);

        Summands = summands;
        Complex = complex;
        Indices = indices;
        SortedNames = sortedNames;
    }

    public virtual Tensor Build()
    {
        if (Complex.IsNaN() || Complex.IsInfinite())
        {
            return Complex;
        }

        List<Tensor> sum = new();
        var isNumeric = Complex.IsNumeric();

        foreach (var nodes in Summands.Values)
        {
            foreach (var node in nodes)
            {
                if (isNumeric)
                {
                    Tensor summand = TensorExtensions.Multiply(
                        ToNumericITransformation.ToNumeric(node.Build()),
                        ToNumericITransformation.ToNumeric(node.Factor));
                    if (summand is NRedberry.Numbers.Complex complexSummand)
                    {
                        Complex = Complex.Add(complexSummand);
                    }
                    else
                    {
                        sum.Add(summand);
                    }
                }
                else
                {
                    Tensor summand = TensorExtensions.Multiply(node.Build(), node.Factor);
                    if (!TensorUtils.IsZero(summand))
                    {
                        sum.Add(summand);
                    }
                }
            }
        }

        if (sum.Count == 0)
        {
            return Complex;
        }

        if (!Complex.IsZero())
        {
            sum.Add(Complex);
        }

        if (sum.Count == 1)
        {
            return sum[0];
        }

        return new Sum(sum.ToArray(), Indices ?? IndicesFactory.EmptyIndices);
    }

    protected abstract Split Split(Tensor tensor);

    public virtual void Put(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (Complex.IsNaN())
        {
            return;
        }

        if (Complex.IsNumeric())
        {
            tensor = ToNumericITransformation.ToNumeric(tensor);
        }

        if (TensorUtils.IsZero(tensor))
        {
            return;
        }

        if (TensorUtils.IsIndeterminate(tensor))
        {
            Complex = Complex.Add((NRedberry.Numbers.Complex)tensor);
            return;
        }

        if (Complex.IsInfinite())
        {
            if (tensor is NRedberry.Numbers.Complex complexTensor)
            {
                Complex = Complex.Add(complexTensor);
            }

            return;
        }

        if (Indices is null)
        {
            Indices = IndicesFactory.Create(tensor.Indices.GetFree());
            SortedNames = Indices.AllIndices.ToArray();
            Array.Sort(SortedNames);
        }
        else if (!Indices.EqualsRegardlessOrder(tensor.Indices.GetFree()))
        {
            throw new TensorException(
                $"Inconsistent indices in sum. Expected: {Indices} Actual: {tensor.Indices.GetFree()}",
                tensor);
        }

        if (tensor is Sum)
        {
            foreach (Tensor s in tensor)
            {
                Put(s);
            }

            return;
        }

        if (tensor is NRedberry.Numbers.Complex complex)
        {
            Complex = Complex.Add(complex);
            return;
        }

        Split split = Split(tensor);

        int hash = TensorHashCalculator.HashWithIndices(split.Factor, SortedNames!);
        if (!Summands.TryGetValue(hash, out var factorNodes))
        {
            List<FactorNode> fns = new() { new FactorNode(split.Factor, split.GetBuilder()) };
            Summands[hash] = fns;
        }
        else
        {
            bool? comparison = null;
            foreach (var node in factorNodes)
            {
                comparison = CompareFactors(split.Factor, node.Factor);
                if (comparison is null)
                {
                    continue;
                }

                if (comparison.Value)
                {
                    node.Put(Tensors.Negate(split.Summand), split.Factor);
                }
                else
                {
                    node.Put(split.Summand, split.Factor);
                }

                break;
            }

            if (comparison is null)
            {
                factorNodes.Add(new FactorNode(split.Factor, split.GetBuilder()));
            }
        }
    }

    public abstract TensorBuilder Clone();

    protected static bool? CompareFactors(Tensor u, Tensor v)
    {
        return IndexMappings.Compare1(u, v);
    }
}
