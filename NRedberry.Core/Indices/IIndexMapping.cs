namespace NRedberry.Indices;

/// <summary>
/// Defines mapping functionality for <see cref="Indices.ApplyIndexMapping(IIndexMapping)"/>.
/// </summary>
public interface IIndexMapping
{
    /// <summary>
    /// Maps a <paramref name="from"/> index onto another index.
    /// </summary>
    /// <param name="from">Source index.</param>
    /// <returns>The mapped index.</returns>
    int Map(int from);
}
