using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Port of cc.redberry.core.utils.stretces.StretchIteratorS.
/// </summary>
public sealed class StretchIteratorS : IEnumerator<Stretch>, IEnumerable<Stretch>
{
    private readonly short[] _values;
    private int _pointer;
    private Stretch? _current;

    /// <summary>
    /// Initializes a new iterator over short values.
    /// </summary>
    /// <param name="values">Value array.</param>
    public StretchIteratorS(short[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        _values = values;
    }

    /// <summary>
    /// Returns whether another stretch is available.
    /// </summary>
    /// <returns><c>true</c> if more stretches remain.</returns>
    public bool HasNext()
    {
        return _pointer < _values.Length;
    }

    /// <summary>
    /// Returns the next stretch.
    /// </summary>
    /// <returns>The next stretch.</returns>
    public Stretch Next()
    {
        int i = _pointer;
        int value = _values[i];
        int begin = i;
        while (++i < _values.Length && _values[i] == value)
        {
        }

        _pointer = i;
        return new Stretch(begin, i - begin);
    }

    public IEnumerator<Stretch> Iterator()
    {
        return this;
    }

    /// <summary>
    /// Unsupported removal operation.
    /// </summary>
    public void Remove()
    {
        throw new NotSupportedException("Remove is not supported.");
    }

    public bool MoveNext()
    {
        if (!HasNext())
        {
            _current = null;
            return false;
        }

        _current = Next();
        return true;
    }

    public void Reset()
    {
        _pointer = 0;
        _current = null;
    }

    public Stretch Current => _current ?? throw new InvalidOperationException("Enumeration has not started.");

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public IEnumerator<Stretch> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
