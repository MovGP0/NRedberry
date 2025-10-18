namespace NRedberry.Core.Transformations.Options;

/// <summary>
/// Represents a set of user-specified options that may require additional initialization.
/// </summary>
public interface IOptions
{
    /// <summary>
    /// Called once all option properties have been populated so that the implementation can perform validation.
    /// </summary>
    void TriggerCreate();
}
