namespace NRedberry.Core.Combinatorics
{
    /// <summary>
    /// This interface is common for all combinatorial iterators.
    /// </summary>
    public interface IIntCombinatorialPort : IOutputPortUnsafe<int[]>
    {
        /// <summary>
        /// Resets the iteration
        /// </summary>
        void Reset();

        /**
         * Returns the reference to the current iteration element.
         *
         * @return the reference to the current iteration element
         */
        int[] GetReference();

        /**
         * Calculates and returns the next combination or null, if no more combinations exist.
         *
         * @return the next combination or null, if no more combinations exist
         */
        //int[] Take();
    }
}