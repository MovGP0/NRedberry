using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Functions;

namespace NRedberry.Core.Utils;

public static class TensorHashCalculator
{
    private static int _hashWithIndices(Tensor tensor, int[] indices)
    {
        if (tensor is SimpleTensor st)
        {
            var si = (SimpleIndices)st.Indices;
            short[] sInds = si.GetDiffIds();
            int hash = tensor.GetHashCode();

            for (int i = 0; i < si.Size(); ++i)
            {
                int pos;
                if ((pos = Arrays.BinarySearch(indices, si[i])) >= 0)
                {
                    hash += HashFunctions.JenkinWang32shift(sInds[i]) ^ (HashFunctions.JenkinWang32shift(pos) * 37);
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
                for (int i = pc.Size - 1; i >= 0; --i)
                {
                    hash += HashFunctions.JenkinWang32shift((int) pc.GetStretchId(i)) * _hashWithIndices(pc[i], indices);
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
        if (indices.Length == 0)
        {
            return tensor.GetHashCode();
        }
        Array.Sort(indices);
        return _hashWithIndices(tensor, indices);
    }

    public static int HashWithIndices(Tensor tensor, Indices.Indices indices)
        => HashWithIndices(tensor, (int[])indices.AllIndices.Copy());

    public static int HashWithIndices(Tensor tensor)
        => HashWithIndices(tensor, tensor.Indices.GetFree());
}