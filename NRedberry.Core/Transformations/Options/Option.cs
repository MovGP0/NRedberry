namespace NRedberry.Transformations.Options;

/// <summary>
/// Marks a constructor parameter or a public field as a configurable option.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OptionAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the option name expected in configuration sources.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the positional index for list-based option binding.
    /// </summary>
    public int Index { get; set; }
}
