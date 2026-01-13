using NRedberry.Indices;

namespace NRedberry.Contexts;

/// <summary>
/// Container for the structure of indices of a tensor and its string name.
/// Two simple tensors are considered to have different mathematical nature
/// if and only if their name and structure are not equal.
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
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(structure);

        Name = name;
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
    {
        HashCode hashCode = new();
        hashCode.Add(Name, StringComparer.Ordinal);
        foreach (var entry in Structure)
        {
            hashCode.Add(entry);
        }

        return hashCode.ToHashCode();
    }

    public bool Equals(NameAndStructureOfIndices? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Name, other.Name, StringComparison.Ordinal)
            && Structure.SequenceEqual(other.Structure);
    }

    public static bool operator ==(NameAndStructureOfIndices? left, NameAndStructureOfIndices? right)
        => Equals(left, right);

    public static bool operator !=(NameAndStructureOfIndices? left, NameAndStructureOfIndices? right)
        => !Equals(left, right);
}
