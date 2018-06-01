namespace NRedberry.Core.Indices
{
    /// <summary>
    ///  Additional specification for indices of simple tensors (see <see cref="Tensors.SimpleTensor"/>).
    /// <see cref="ISimpleIndices"/> preserves the relative ordering of indices with the same type.
    /// </summary>
    public interface ISimpleIndices : IIndices
    {
        /// <summary>
        /// Returns <see cref="IndicesSymmetries"/> of this <see cref="IIndices"/>.
        /// </summary>
        /// <returns>
        /// symmetries of this indices
        /// </returns>
        IndicesSymmetries GetSymmetries();

        /// <summary>
        /// Sets indices symmetries to the specified
        /// </summary>
        void SetSymmetries(IndicesSymmetries symmetries);

        /// <summary>
        /// Compares simple indices taking into account possible permutations according to the symmetries.
        /// </summary>
        /// <param name="indices">
        /// indices to compare with this
        /// </param>
        /// <returns>
        /// <value>true</value> if specified indices can be obtained via permutations (specified by symmetries) of this indices.
        /// </returns>
        bool EqualsWithSymmetries(ISimpleIndices indices);

        /// <summary>
        /// Returns the structure of this indices.
        /// </summary>
        /// <returns>
        /// structure of this indices
        /// </returns>
        StructureOfIndices GetStructureOfIndices();
    }
}
