using NRedberry.Core.Indices;

namespace NRedberry.Contexts;

/// <summary>
/// Specific implementation of <see cref="NameDescriptor"/> for Kronecker and metric tensors.
/// </summary>
internal sealed class NameDescriptorForMetricAndKronecker : NameDescriptor
{
    /// <summary>
    /// First name for Kronecker, second for metric
    /// Same instance as in NameManager
    /// </summary>
    private readonly string[] _names;

    public NameDescriptorForMetricAndKronecker(string[] names, byte type, int id)
        : base(CreateIndicesTypeStructures(type), id)
    {
        _names = names;
    }

    private static StructureOfIndices[] CreateIndicesTypeStructures(byte type)
    {
        var structures = new StructureOfIndices[1];
        structures[0] = !CC.IsMetric(type)
            ? StructureOfIndices.Create(type, 2, true, false)
            : StructureOfIndices.Create(type, 2);
        return structures;
    }

    /// <summary>
    /// First for Kronecker, second for metric
    /// </summary>
    public override NameAndStructureOfIndices[] GetKeys()
    {
        return
        [
            new(_names[0], IndexTypeStructures),
            new(_names[1], IndexTypeStructures)
        ];
    }

    public override string GetName(SimpleIndices? indices, OutputFormat outputFormat)
    {
        if (indices == null)
        {
            throw new ArgumentNullException(nameof(indices));
        }

        bool metric = IndicesUtils.HaveEqualStates(indices[0], indices[1]);
        return metric ? _names[1] : _names[0];
    }

    public override string ToString()
        => $"{_names[0]}:{IndexTypeStructures}";
}
