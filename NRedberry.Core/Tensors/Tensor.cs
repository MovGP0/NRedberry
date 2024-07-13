using System;
using System.Collections;
using System.Collections.Generic;
using NRedberry.Contexts;
using NRedberry.Core.Indices;

namespace NRedberry.Core.Tensors;

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

    public abstract Tensor this[int i]
    {
        get;
    }

    public abstract int Size
    {
        get;
    }

    public Tensor Set(int i, Tensor tensor)
    {
        var size = Size;
        if (i >= size || i < 0) throw new IndexOutOfRangeException(nameof(i));
        if (tensor == null) throw new ArgumentNullException(nameof(tensor));

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
        if (from < 0 || from > to || to >= size)
            throw new IndexOutOfRangeException();
        var range = new Tensor[from - to];
        for (size = 0; from < to; ++size, ++from)
            range[size] = this[from];
        return range;
    }

    public Tensor[] ToArray()
    {
        return GetRange(0, Size);
    }

    public abstract string ToString(OutputFormat outputFormat);
    public override string ToString() => ToString(Context.Get().DefaultOutputFormat);

    /// <summary>
    /// For internal use.
    /// </summary>
    protected virtual string ToString<T>(OutputFormat mode) where T : Tensor => ToString(mode);

    public int CompareTo(Tensor other) => GetHashCode().CompareTo(other.GetHashCode());

    public abstract TensorBuilder GetBuilder();

    public abstract TensorFactory? GetFactory();

    public static Tensor Sum(Tensor[] contentToTensors)
    {
        throw new NotImplementedException();
    }

    public static Tensor MultiplyAndRenameConflictingDummies(Tensor[] contentToTensors)
    {
        throw new NotImplementedException();
    }

    public static Tensor Expression(Tensor toTensor, Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static SimpleTensor SimpleTensor(string name, SimpleIndices indices)
    {
        throw new NotImplementedException();
    }

    public static SimpleTensor SimpleTensor(int name, SimpleIndices indices)
    {
        throw new NotImplementedException();
    }
}