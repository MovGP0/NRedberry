using System;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

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
        new NameAndStructureOfIndices(name, indexTypeStructures)
    ];

    private SimpleTensor? cachedSymbol;

    [Pure]
    public override string GetName(SimpleIndices? indices)
    {
        return Name;
    }

    [Pure]
    public override NameAndStructureOfIndices[] GetKeys()
    {
        return key;
    }

    public void SetCachedInstance(SimpleTensor symbol)
    {
        if (cachedSymbol != null)
        {
            throw new InvalidOperationException("Symbol is already created.");
        }
        cachedSymbol = symbol;
    }

    [Pure]
    public SimpleTensor GetCachedSymbol() => cachedSymbol;
}
