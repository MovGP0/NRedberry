using System.Diagnostics;
using System.Text;
using NRedberry.Core.Utils;
using NRedberry.Numbers;

namespace NRedberry.Tensors;

public sealed class Sum : MultiTensor
{
    internal readonly Tensor[] Data;
    private readonly int hash;

    public Sum(Tensor[] data, Indices.Indices indices)
        : base(indices)
    {
        Debug.Assert(data.Length > 1);

        Data = data;
        TensorWrapper[] wrappers = new TensorWrapper[data.Length];
        for (int i = 0; i < data.Length; ++i)
        {
            wrappers[i] = new TensorWrapper(data[i]);
        }

        ArraysUtils.QuickSort(wrappers, data);
        hash = Hash0(data, indices);
    }

    public Sum(Indices.Indices indices, Tensor[] data, int hash)
        : base(indices)
    {
        Debug.Assert(data.Length > 1);
        Data = data;
        this.hash = hash;
    }

    public override int GetHashCode()
    {
        return hash;
    }

    public override Tensor this[int i] => Data[i];

    public override int Size => Data.Length;

    public override string ToString(OutputFormat outputFormat)
    {
        var sb = new StringBuilder();
        for (int i = 0; ; ++i)
        {
            string str = this[i].ToStringWith<Sum>(outputFormat);
            if ((str[0] == '-' || str[0] == '+') && sb.Length != 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            if (str.Contains('\'') && !outputFormat.PrintMatrixIndices)
            {
                return ToString(outputFormat.PrintMatrixIndicesAlways());
            }

            sb.Append(str);
            if (i == Size - 1)
            {
                return sb.ToString();
            }

            sb.Append('+');
        }
    }

    protected override string ToString<T>(OutputFormat outputFormat)
    {
        if (typeof(T) == typeof(Power) || typeof(T) == typeof(Product))
        {
            return "(" + ToString(outputFormat) + ")";
        }

        return ToString(outputFormat);
    }

    public override TensorBuilder GetBuilder()
    {
        return new SumBuilder(Data.Length);
    }

    public override TensorFactory? GetFactory()
    {
        return SumFactory.Factory;
    }

    protected override Tensor Remove1(int[] positions)
    {
        Tensor[] newData = new Tensor[Data.Length - positions.Length];
        int pointer = 0;
        int counter = -1;
        for (int i = 0; i < Data.Length; ++i)
        {
            if (pointer < positions.Length && i == positions[pointer])
            {
                ++pointer;
            }
            else
            {
                newData[++counter] = Data[i];
            }
        }

        if (newData.Length == 1)
        {
            return newData[0];
        }

        return new Sum(newData, Indices);
    }

    public override Tensor Remove(int position)
    {
        if (position >= Data.Length || position < 0)
        {
            throw new IndexOutOfRangeException();
        }

        if (Data.Length == 2)
        {
            return Data[1 - position];
        }

        Tensor[] newData = new Tensor[Data.Length - 1];
        Array.Copy(Data, 0, newData, 0, position);
        if (position < Data.Length - 1)
        {
            Array.Copy(Data, position + 1, newData, position, Data.Length - position - 1);
        }

        return new Sum(newData, Indices);
    }

    protected override Complex GetNeutral()
    {
        return Complex.Zero;
    }

    protected override Tensor Select1(int[] positions)
    {
        Tensor[] newData = new Tensor[positions.Length];
        int i = -1;
        foreach (int position in positions)
        {
            newData[++i] = Data[position];
        }

        return new Sum(newData, Indices);
    }

    public new Tensor Set(int i, Tensor tensor)
    {
        if (i >= Data.Length || i < 0)
        {
            throw new IndexOutOfRangeException();
        }

        Tensor old = Data[i];
        if (ReferenceEquals(old, tensor))
        {
            return this;
        }

        if (EqualsExactly(old, tensor))
        {
            return this;
        }

        if (TensorUtils.IsIndeterminate(tensor))
        {
            return tensor;
        }

        if (TensorUtils.IsZero(tensor))
        {
            return Remove(i);
        }

        Tensor[] newData = (Tensor[])Data.Clone();
        newData[i] = tensor;
        if (EqualsTensor(old, tensor))
        {
            return new Sum(newData, Indices);
        }

        return Tensors.Sum(newData);
    }

    private static int Hash0(Tensor[] data, Indices.Indices indices)
    {
        if (indices.Size() > 1)
        {
            return HashArray(data);
        }

        bool isSimplePoly = true;
        int[] termsHashes = new int[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            termsHashes[i] = data[i].GetHashCode();
            if (!IsSymbolicPoly(data[i]))
            {
                isSimplePoly = false;
            }
        }

        if (!isSimplePoly)
        {
            return HashArray(termsHashes);
        }

        int result = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] is Product product)
            {
                termsHashes[i] *= product.Factor.HashWithSign();
            }
            else if (data[i] is Complex complex)
            {
                termsHashes[i] *= complex.HashWithSign();
            }

            result = 31 * result + termsHashes[i];
        }

        unchecked
        {
            return result * result;
        }
    }

    private static bool IsSymbolicPoly(Tensor tensor)
    {
        if (tensor is Complex)
        {
            return true;
        }

        if (tensor is SimpleTensor)
        {
            return tensor.Indices.Size() <= 1;
        }

        if (tensor is Product || tensor is Power)
        {
            foreach (Tensor t in tensor)
            {
                if (!IsSymbolicPoly(t))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    private static int HashArray(Tensor[] data)
    {
        int result = 1;
        foreach (Tensor tensor in data)
        {
            result = 31 * result + tensor.GetHashCode();
        }

        return result;
    }

    private static int HashArray(int[] data)
    {
        int result = 1;
        foreach (int value in data)
        {
            result = 31 * result + value;
        }

        return result;
    }

    private static bool EqualsExactly(Tensor left, Tensor right)
    {
        return left.Equals(right);
    }

    private static bool EqualsTensor(Tensor left, Tensor right)
    {
        return left.Equals(right);
    }
}
