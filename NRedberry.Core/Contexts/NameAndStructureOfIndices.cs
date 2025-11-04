using NRedberry.Core.Indices;

namespace NRedberry.Contexts;

/// <inheritdoc />
/// <summary>
/// Container for the structure of indices(see { @link cc.redberry.core.indices.StructureOfIndices}) of tensor
/// and its string name.Two simple tensors are considered to have different mathematical nature if and only if
/// their { @code IndicesTypeStructureAndName} are not equal.
/// </summary>
public sealed class NameAndStructureOfIndices : IEquatable<NameAndStructureOfIndices>
{
    /// <summary>
    /// Name of tensor.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Structure of tensor indices.
    /// </summary>
    public StructureOfIndices[] Structure { get; }

    ///<summary>
    /// Creates new instance of <see cref="NameAndStructureOfIndices"/>
    /// </summary>
    /// <param name="name">name of tensor</param>
    /// <param name="structure">structure of tensor indices</param>
    public NameAndStructureOfIndices(string name, StructureOfIndices[] structure)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Structure = structure;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is NameAndStructureOfIndices nameAndStructureOfIndices
            && Equals(nameAndStructureOfIndices);
    }

    public override int GetHashCode()
        => HashCode.Combine(Name, Structure);

    public bool Equals(NameAndStructureOfIndices? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return string.Equals(Name, other.Name)
            && Equals(Structure, other.Structure);
    }
}
