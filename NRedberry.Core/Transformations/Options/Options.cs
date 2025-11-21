namespace NRedberry.Transformations.Options;

/// <summary>
/// Marks a constructor parameter as carrying transformation options.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
public sealed class OptionsAttribute : Attribute
{
}
