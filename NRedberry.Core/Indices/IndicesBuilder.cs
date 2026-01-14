using System.Collections;
using NRedberry.Tensors;

namespace NRedberry.Indices;

/// <summary>
/// Builder of unordered indices, producing <see cref="SortedIndices"/> results.
/// </summary>
public sealed class IndicesBuilder : IEnumerable<int>, ICloneable<IndicesBuilder>
{
    private readonly List<int> data;

    public IndicesBuilder()
    {
        data = [];
    }

    public IndicesBuilder(int capacity)
    {
        data = new List<int>(capacity);
    }

    private IndicesBuilder(List<int> data)
    {
        this.data = data;
    }

    private IndicesBuilder(params int[] data)
    {
        this.data = [..data];
    }

    public IndicesBuilder Append(int index)
    {
        data.Add(index);
        return this;
    }

    public IndicesBuilder Append(params int[] indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        data.AddRange(indices);
        return this;
    }

    public IndicesBuilder Append(int[][] indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        foreach (int[] array in indices)
        {
            ArgumentNullException.ThrowIfNull(array);
            data.AddRange(array);
        }

        return this;
    }

    public IndicesBuilder Append(IEnumerable<int> indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        data.AddRange(indices);
        return this;
    }

    public IndicesBuilder Append(Indices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        data.AddRange(indices.AllIndices);
        return this;
    }

    public IndicesBuilder Append(IndicesBuilder ib)
    {
        ArgumentNullException.ThrowIfNull(ib);
        return Append(ib.ToArray());
    }

    public IndicesBuilder Append(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return Append(tensor.Indices);
    }

    public IndicesBuilder Append(params Tensor[] tensors)
    {
        ArgumentNullException.ThrowIfNull(tensors);
        foreach (Tensor tensor in tensors)
        {
            Append(tensor);
        }

        return this;
    }

    public IndicesBuilder Append<T>(IEnumerable<T> tensors)
        where T : Tensor
    {
        ArgumentNullException.ThrowIfNull(tensors);
        foreach (Tensor tensor in tensors)
        {
            Append(tensor);
        }

        return this;
    }

    public Indices Indices => new SortedIndices(ToArray());

    public int[] ToArray()
    {
        return data.ToArray();
    }

    public override string ToString() => Indices.ToString() ?? string.Empty;

    #region IEnumerable

    public IEnumerator<int> GetEnumerator() => data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region ICloneable

    public IndicesBuilder Clone() => new(new List<int>(data));

    object ICloneable.Clone() => Clone();

    #endregion
}
