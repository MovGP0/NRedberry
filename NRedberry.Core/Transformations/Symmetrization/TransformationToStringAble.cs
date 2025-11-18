using NRedberry.Contexts;

namespace NRedberry.Core.Transformations.Symmetrization;

/// <summary>
/// Represents a transformation that can provide a formatted textual description.
/// </summary>
public interface TransformationToStringAble : ITransformation
{
    string ToString(OutputFormat outputFormat);
}
