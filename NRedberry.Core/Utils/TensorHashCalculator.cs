using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;

namespace NRedberry.Utils;

public static class TensorHashCalculator
{
    private static int _hashWithIndices(Tensor tensor, int[] indices)
    {
        if (tensor is SimpleTensor st)
        {
            var si = (SimpleIndices)st.Indices;
            short[] sInds = si.GetDiffIds();
            int hash = tensor.GetHashCode();

            int pos;
            for (int i = 0; i < si.Size(); ++i)
            {
                if ((pos = Arrays.BinarySearch(indices, si[i])) >= 0)
                {
                    hash += HashFunctions.JenkinWang32shift(sInds[i])
                        ^ (HashFunctions.JenkinWang32shift(pos) * 37);
                }
            }

            return HashFunctions.JenkinWang32shift(hash);
        }

        if (tensor is ScalarFunction sf)
        {
            return sf.GetHashCode();
        }

        {
            int hash = tensor.GetHashCode();
            if (tensor is Product product)
            {
                ProductContent pc = product.Content;
                if (pc.Size == 1)
                {
                    int dataHash = _hashWithIndices(pc[0], indices);
                    return product.Factor.IsOneOrMinusOne()
                        ? dataHash
                        : dataHash * product.Factor.GetHashCode();
                }

                // TODO consider noncommutative operation using stretch ids.
                for (int i = pc.Size - 1; i >= 0; --i)
                {
                    hash += HashFunctions.JenkinWang32shift((int)pc.GetStretchId(i))
                        * _hashWithIndices(pc[i], indices);
                }

                return hash;
            }

            foreach (Tensor t in tensor)
            {
                hash ^= _hashWithIndices(t, indices);
            }

            return hash;
        }
    }

    public static int HashWithIndices(Tensor tensor, int[] indices)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indices);

        if (indices.Length == 0)
        {
            return tensor.GetHashCode();
        }

        Array.Sort(indices);
        return _hashWithIndices(tensor, indices);
    }

    public static int HashWithIndices(Tensor tensor, Indices.Indices indices)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indices);

        return HashWithIndices(tensor, indices.AllIndices.ToArray());
    }

    public static int HashWithIndices(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        return HashWithIndices(tensor, tensor.Indices.GetFree());
    }

    public static int NonTopologicalHash(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor.GetType() == typeof(SimpleTensor))
        {
            return tensor.GetHashCode();
        }

        int hashCode = tensor.GetType().GetHashCode();
        foreach (Tensor current in tensor)
        {
            hashCode = 13 * hashCode + NonTopologicalHash(current);
        }

        return hashCode;
    }
}
