namespace NRedberry.Core.Combinatorics.Extensions;

public static class EnumeratorExtensions
{
    public static IEnumerator<T> GetEnumerator<T>(this T element)
    {
        yield return element;
    }
}
