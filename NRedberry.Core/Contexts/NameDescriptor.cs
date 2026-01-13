using NRedberry.Indices;

namespace NRedberry.Contexts;

/// <summary>
/// Represents the unique type of a simple tensor or tensor field (unique name).
/// It holds the string name, the structure of indices, and argument structures (for tensor fields).
/// Two simple tensors differ in mathematical nature if and only if their name descriptors are not equal.
/// </summary>
public abstract class NameDescriptor
{
    /// <summary>
    /// First element is simple tensor indexTypeStructure, other appears for tensor fields
    /// </summary>
    public StructureOfIndices[] IndexTypeStructures { get; }

    public StructureOfIndices[] StructuresOfIndices => IndexTypeStructures;

    /// <summary>
    /// Unique simple tensor identifier.
    /// </summary>
    public int Id { get; }

    public IndicesSymmetries Symmetries { get; }
    public NameManager? NameManager { get; private set; }

    protected NameDescriptor(StructureOfIndices[] indexTypeStructures, int id)
    {
        ArgumentNullException.ThrowIfNull(indexTypeStructures);
        if (indexTypeStructures.Length == 0)
        {
            throw new ArgumentException();
        }

        Id = id;
        IndexTypeStructures = indexTypeStructures;
        Symmetries = IndicesSymmetries.Create(indexTypeStructures[0]);
    }

    public void RegisterInNameManager(NameManager nameManager)
    {
        ArgumentNullException.ThrowIfNull(nameManager);
        if (NameManager != null && !ReferenceEquals(NameManager, nameManager))
        {
            throw new InvalidOperationException("Already registered in another name manager.");
        }

        NameManager = nameManager;
    }

    /// <summary>
    /// Return symmetries of indices of tensors with this name descriptor.
    /// </summary>
    public IndicesSymmetries GetSymmetries()
    {
        return Symmetries;
    }

    /// <summary>
    /// Return <c>true</c> if this is a descriptor of tensor field.
    /// </summary>
    public bool IsField()
    {
        return IndexTypeStructures.Length != 1;
    }

    /// <summary>
    /// Returns structure of indices of tensors with this name descriptor
    /// </summary>
    [Pure]
    public StructureOfIndices GetStructureOfIndices()
    {
        return IndexTypeStructures[0];
    }

    /// <summary>
    /// Returns structure of indices of tensors with this name descriptor (first element in array) and
    /// structures of indices of their arguments (in case of tensor field)
    /// </summary>
    /// <returns>structure of indices of tensors and their arguments</returns>
    public StructureOfIndices[] GetStructuresOfIndices()
    {
        return IndexTypeStructures;
    }

    public abstract NameAndStructureOfIndices[] GetKeys();

    /// <summary>
    /// Returns string name of tensor. The argument can be <c>null</c> in case of metric or Kronecker.
    /// </summary>
    /// <param name="indices">indices (in case of metric or Kronecker) and null in other cases</param>
    /// <param name="format">output format</param>
    /// <returns>string name of tensor</returns>
    public abstract string GetName(SimpleIndices? indices, OutputFormat format);

    public override string ToString()
    {
        return $"{GetName(null, OutputFormat.Redberry)}:{IndexTypeStructures}";
    }

    /// <summary>
    /// Returns structure of indices of tensors with specified name descriptor
    /// </summary>
    /// <param name="nd">name descriptor</param>
    /// <returns>structure of indices of tensors with specified name descriptor</returns>
    [Pure]
    public static NameAndStructureOfIndices ExtractKey(NameDescriptor nd)
    {
        return nd.GetKeys()[0];
    }

    /// <summary>
    /// Returns structure of i-th arg indices of tensors with this name descriptor.
    /// </summary>
    /// <param name="arg"></param>
    /// <returns>structure of i-th arg indices indices of tensors with this name descriptor</returns>
    public StructureOfIndices GetArgStructuresOfIndices(int arg)
    {
        return IndexTypeStructures[arg + 1];
    }
}
