using System.Collections;

namespace NRedberry.Core.Combinatorics;

/// <summary>
/// Wraps <see cref="IntPermutationsGenerator"/> to yield <see cref="Symmetry"/> instances.
/// </summary>
/// <typeparam name="T">Permutation subtype.</typeparam>
public sealed class PermutationsGenerator<T> : IEnumerator<T>
    where T : Permutation
{
    private readonly IntPermutationsGenerator _generator;
    private T? _current;

    public PermutationsGenerator(int dimension) => _generator = new IntPermutationsGenerator(dimension);

    public PermutationsGenerator(Permutation permutation) => _generator = new IntPermutationsGenerator(permutation.OneLine());

    public T Current
    {
        get
        {
            if (_current == null)
            {
                throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
            }

            return _current;
        }
    }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (!_generator.MoveNext())
        {
            _current = default;
            return false;
        }

        var next = _generator.Next();
        _current = (T)(Permutation)new Symmetry(next, false, true);
        return true;
    }

    public void Reset()
    {
        _generator.Reset();
        _current = default;
    }

    public void Dispose()
    {
    }
}
