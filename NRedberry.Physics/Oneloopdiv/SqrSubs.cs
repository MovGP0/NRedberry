using System.Runtime.InteropServices;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Removes contracted square factors of a specified simple tensor from products.
/// </summary>
public sealed class SqrSubs : ITransformation
{
    private readonly string _name;

    public SqrSubs(SimpleTensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor.Indices.Size() != 1)
        {
            throw new ArgumentException("Expected a single index.", nameof(tensor));
        }

        _name = tensor.GetStringName();
    }

    public Tensor Transform(Tensor tensor)
    {
        if (tensor is not Product product)
        {
            return tensor;
        }

        Dictionary<int, (Queue<int> Lower, Queue<int> Upper)> positionsByIndex = new();
        for (int index = 0; index < product.Data.Length; ++index)
        {
            if (product.Data[index] is not SimpleTensor simpleTensor
                || !string.Equals(simpleTensor.GetStringName(), _name, StringComparison.Ordinal)
                || simpleTensor.Indices.Size() != 1)
            {
                continue;
            }

            int actualIndex = simpleTensor.SimpleIndices[0];
            int indexName = IndicesUtils.GetNameWithType(actualIndex);
            ref (Queue<int> Lower, Queue<int> Upper) entry = ref CollectionsMarshal.GetValueRefOrAddDefault(
                positionsByIndex,
                indexName,
                out bool exists);

            if (!exists)
            {
                entry = (new Queue<int>(), new Queue<int>());
            }

            if ((actualIndex & IndicesUtils.UpperRawStateInt) == 0)
            {
                entry.Lower.Enqueue(index);
            }
            else
            {
                entry.Upper.Enqueue(index);
            }
        }

        List<int> toRemove = [];
        foreach ((Queue<int> lower, Queue<int> upper) in positionsByIndex.Values)
        {
            while (lower.Count > 0 && upper.Count > 0)
            {
                toRemove.Add(lower.Dequeue());
                toRemove.Add(upper.Dequeue());
            }
        }

        if (toRemove.Count == 0)
        {
            return tensor;
        }

        toRemove.Sort();

        List<Tensor> factors = [];
        if (product.Factor != Complex.One)
        {
            factors.Add(product.Factor);
        }

        factors.AddRange(product.IndexlessData);
        for (int i = 0; i < product.Data.Length; ++i)
        {
            if (toRemove.BinarySearch(i) < 0)
            {
                factors.Add(product.Data[i]);
            }
        }

        return TensorFactory.Multiply(factors);
    }
}
