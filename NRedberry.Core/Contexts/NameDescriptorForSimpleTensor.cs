using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Contexts;

/// <summary>
/// Implementation of <see cref="NameDescriptor"/> for any simple tensor, except Kronecker and metric tensor.
/// </summary>
internal sealed class NameDescriptorForSimpleTensor(
    string name,
    StructureOfIndices[] indexTypeStructures,
    int id)
    : NameDescriptor(indexTypeStructures, id)
{
    public string Name { get; } = name;

    private readonly NameAndStructureOfIndices[] key =
    [
        new(name, indexTypeStructures)
    ];

    [Pure]
    public override string GetName(SimpleIndices? indices, OutputFormat outputFormat)
    {
        return Name;
    }

    [Pure]
    public override NameAndStructureOfIndices[] GetKeys()
    {
        return key;
    }

    [Pure]
    public SimpleTensor CachedSymbol
    {
        get;
        set
        {
            if (field is not null)
            {
                throw new InvalidOperationException("Symbol is already created.");
            }

            field = value;
        }
    }
}
