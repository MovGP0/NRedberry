namespace NRedberry.Transformations.Options;

/// <summary>
/// Indicates that the annotated constructor should be used when building a transformation from options.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor)]
public sealed class CreatorAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a value indicating whether the annotated constructor accepts a variable number of arguments.
    /// </summary>
    public bool Vararg { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the annotated constructor expects required arguments.
    /// </summary>
    public bool HasArgs { get; set; }
}
