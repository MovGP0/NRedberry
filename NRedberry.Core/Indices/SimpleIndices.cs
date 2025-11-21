namespace NRedberry.Indices;

/// <summary>
///  Additional specification for indices of simple tensors (see <see cref="Tensors.SimpleTensor"/>).
/// <see cref="SimpleIndices"/> preserves the relative ordering of indices with the same type.
/// </summary>
public interface SimpleIndices : Indices
{
    /// <summary>
    /// Gets or sets <see cref="IndicesSymmetries"/> of this <see cref="Indices"/>.
    /// </summary>
    IndicesSymmetries Symmetries { get; set; }

    /// <summary>
    /// Compares simple indices taking into account possible permutations according to the symmetries.
    /// </summary>
    /// <param name="indices>
    /// indices to compare with this
    /// </param>
    /// <returns>
    /// <value>true</value> if specified indices can be obtained via permutations (specified by symmetries) of this indices.
    /// </returns>
    bool EqualsWithSymmetries(SimpleIndices indices);

    /// <summary>
    /// Returns the structure of this indices.
    /// </summary>
    /// <returns>
    /// structure of this indices
    /// </returns>
    StructureOfIndices StructureOfIndices { get; }
}
