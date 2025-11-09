namespace NRedberry.Core.Combinatorics;

public class PermutationsGenerator<T> : IEnumerator<T> where T : Permutation
{
    private IntPermutationsGenerator generator;

    public PermutationsGenerator(int dimension)
    {
        generator = new IntPermutationsGenerator(dimension);
    }

    public PermutationsGenerator(Permutation permutation)
    {
        throw new NotImplementedException();
    }

    public bool MoveNext()
    {
        return generator.MoveNext();
    }

    public T Current
    {
        get
        {
            generator.MoveNext();
            return (T)Activator.CreateInstance(typeof(T), generator.Permutation.Clone(), false);
        }
    }

    public void Reset()
    {
        generator.Reset();
    }

    object System.Collections.IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}
