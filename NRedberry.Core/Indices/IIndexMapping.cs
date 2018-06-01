namespace NRedberry.Core.Indices
{
    /// <summary>
    /// This interface defines the mapping functionality for
    /// {@link cc.redberry.core.indices.Indices#applyIndexMapping(IndexMapping)} method.
    /// </summary>
    public interface IIndexMapping
    {
        /// <summary>
        /// Maps a <paramref name="from"/> index onto the other index
        /// </summary>
        /// <param name="from">from index</param>
        /// <returns>the mapping of <paramref name="from"/> index</returns>
        int Map(int from);
    }
}