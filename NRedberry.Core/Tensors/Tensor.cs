using System.Collections;
using NRedberry.Contexts;
using NRedberry.Indices;

namespace NRedberry.Tensors;

public abstract class Tensor : IComparable<Tensor>, IEnumerable<Tensor>
{
    public abstract Indices.Indices Indices { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual IEnumerator<Tensor> GetEnumerator()
    {
        return new BasicTensorIterator(this);
    }

    public abstract Tensor this[int i] { get; }

    public abstract int Size { get; }

    public Tensor Set(int i, Tensor tensor)
    {
        var size = Size;
        if (i >= size || i < 0)
        {
            throw new IndexOutOfRangeException(nameof(i));
        }

        ArgumentNullException.ThrowIfNull(tensor);

        var builder = GetBuilder();
        for (var j = 0; j < size; ++j)
        {
            builder.Put(j == i ? tensor : this[j]);
        }

        return builder.Build();
    }

    public Tensor[] GetRange(int from, int to)
    {
        var size = Size;
        if (from < 0 || from > to || to > size)
        {
            throw new IndexOutOfRangeException();
        }

        var range = new Tensor[to - from];
        for (size = 0; from < to; ++size, ++from)
        {
            range[size] = this[from];
        }

        return range;
    }

    public Tensor[] ToArray()
    {
        return GetRange(0, Size);
    }

    public abstract string ToString(OutputFormat outputFormat);

    public override string ToString()
    {
        return ToString(Context.Get().DefaultOutputFormat);
    }

    /// <summary>
    /// For internal use.
    /// </summary>
    protected virtual string ToString<T>(OutputFormat mode) where T : Tensor
    {
        return ToString(mode);
    }

    public string ToStringWith<T>(OutputFormat mode) where T : Tensor
    {
        return ToString<T>(mode);
    }

    public int CompareTo(Tensor? other)
    {
        if (other is null)
        {
            return 1;
        }

        return GetHashCode().CompareTo(other.GetHashCode());
    }

    public abstract TensorBuilder GetBuilder();

    public abstract TensorFactory? GetFactory();

    public static Tensor Sum(Tensor[] contentToTensors)
    {
        ArgumentNullException.ThrowIfNull(contentToTensors);
        return SumFactory.Factory.Create(contentToTensors);
    }

    public static Tensor MultiplyAndRenameConflictingDummies(Tensor[] contentToTensors)
    {
        ArgumentNullException.ThrowIfNull(contentToTensors);
        return ProductFactory.Factory.Create(ResolveDummy(contentToTensors));
    }

    public static Tensor Expression(Tensor toTensor, Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(toTensor);
        ArgumentNullException.ThrowIfNull(tensor);
        return ExpressionFactory.Instance.Create(toTensor, tensor);
    }

    public static SimpleTensor SimpleTensor(string name, SimpleIndices indices)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(indices);

        var descriptor = CC.NameManager.MapNameDescriptor(name, indices.StructureOfIndices);
        if (indices.Size() == 0)
        {
            var simpleDescriptor = (NameDescriptorForSimpleTensor)descriptor;
            if (simpleDescriptor.CachedSymbol is null)
            {
                var st = new SimpleTensor(descriptor.Id, indices);
                simpleDescriptor.CachedSymbol = st;
                return st;
            }

            return simpleDescriptor.CachedSymbol;
        }

        return new SimpleTensor(
            descriptor.Id,
            IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices));
    }

    public static SimpleTensor SimpleTensor(int name, SimpleIndices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);

        var descriptor = CC.GetNameDescriptor(name);
        if (descriptor is null)
        {
            throw new ArgumentException("This name is not registered in the system.");
        }

        if (!descriptor.GetStructureOfIndices().IsStructureOf(indices))
        {
            throw new ArgumentException(
                $"Specified indices ( {indices} )are not indices of specified tensor ( {descriptor} ).");
        }

        if (indices.Size() == 0)
        {
            var simpleDescriptor = (NameDescriptorForSimpleTensor)descriptor;
            if (simpleDescriptor.CachedSymbol is null)
            {
                var st = new SimpleTensor(descriptor.Id, indices);
                simpleDescriptor.CachedSymbol = st;
                return st;
            }

            return simpleDescriptor.CachedSymbol;
        }

        return new SimpleTensor(
            name,
            IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices));
    }

    private static Tensor[] ResolveDummy(params Tensor[] factors)
    {
        var result = new Tensor[factors.Length];
        HashSet<int> forbidden = [];
        List<Tensor> toResolve = [];
        for (int i = factors.Length - 1; i >= 0; --i)
        {
            var factor = factors[i];
            if (factor is MultiTensor || factor.Indices.GetFree().Size() == 0)
            {
                toResolve.Add(factor);
                forbidden.UnionWith(IndicesUtils.GetIndicesNames(factor.Indices.GetFree()));
            }
            else
            {
                forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(factor));
                result[i] = factor;
            }
        }

        int toResolvePosition = toResolve.Count;
        for (int i = factors.Length - 1; i >= 0; --i)
        {
            if (result[i] is null)
            {
                var factor = toResolve[--toResolvePosition];
                var newFactor = ApplyIndexMapping.RenameDummy(factor, forbidden.ToArray());
                forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(newFactor));
                result[i] = newFactor;
            }
        }

        return result;
    }
}
