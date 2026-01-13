using NRedberry.Indices;

namespace NRedberry.Contexts;

/// <summary>
/// Specific implementation of <see cref="NameDescriptor"/> for Kronecker and metric tensors.
/// </summary>
internal sealed class NameDescriptorForMetricAndKronecker(string[] names, byte type, int id)
    : NameDescriptor(CreateIndicesTypeStructures(type), id)
{
    /// <summary>
    /// First name for Kronecker, second for metric
    /// Same instance as in NameManager
    /// </summary>
    private readonly string[] _names = ValidateNames(names);

    private static StructureOfIndices[] CreateIndicesTypeStructures(byte type)
    {
        var structures = new StructureOfIndices[1];
        structures[0] = !CC.IsMetric(type)
            ? StructureOfIndices.Create(type, 2, true, false)
            : StructureOfIndices.Create(type, 2);
        return structures;
    }

    private static string[] ValidateNames(string[] names)
    {
        ArgumentNullException.ThrowIfNull(names);
        if (names.Length < 2)
        {
            throw new ArgumentException("Expected at least two names.", nameof(names));
        }

        return names;
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
        ArgumentNullException.ThrowIfNull(indices);

        bool metric = IndicesUtils.HaveEqualStates(indices[0], indices[1]);
        return metric ? _names[1] : _names[0];
    }

    public override string ToString()
        => $"{_names[0]}:{IndexTypeStructures}";
}
