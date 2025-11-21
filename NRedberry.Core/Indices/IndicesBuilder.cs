using System.Collections;
using NRedberry.Tensors;

namespace NRedberry.Indices;

public sealed class IndicesBuilder : IEnumerable<int>, ICloneable<IndicesBuilder>
{
    private readonly List<int> data;

    public IndicesBuilder() => data = [];
    private IndicesBuilder(List<int> data) => this.data = data;
    private IndicesBuilder(params int[] data) => this.data = [..data];
    public IndicesBuilder(int capacity) => data = new List<int>(capacity);

    public IndicesBuilder Append(int index)
    {
        data.Add(index);
        return this;
    }

    public IndicesBuilder Append(params int[] indices)
    {
        data.AddRange(indices);
        return this;
    }

    public IndicesBuilder Append(IEnumerable<int> indices)
    {
        data.AddRange(indices);
        return this;
    }

    public IndicesBuilder Append(IndicesBuilder ib) => Append(ib.ToArray());

    public IndicesBuilder Append(params Tensor[] tensors)
    {
        foreach (var t in tensors)
        {
            Append(t);
        }

        return this;
    }

    public IndicesBuilder Append<T>(IEnumerable<T> tensors)
        where T:Tensor
    {
        foreach (var t in tensors)
        {
            Append(t);
        }

        return this;
    }

    public Indices Indices => IndicesFactory.Create(data.ToArray());

    public override string ToString() => Indices.ToString() ?? string.Empty;

    #region IEnumerable

    public IEnumerator<int> GetEnumerator() => data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region ICloneable

    public IndicesBuilder Clone() => new(data.ToArray());

    object ICloneable.Clone() => Clone();

    #endregion
}
