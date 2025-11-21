namespace NRedberry.Core.Utils.Stretces;

/// <summary>
/// Initializes a new stretch.
/// </summary>
/// <remarks>
/// Port of cc.redberry.core.utils.stretces.Stretch.
/// </remarks>
/// <param name="From">The starting index.</param>
/// <param name="Length">The span length.</param>
public sealed record Stretch(int From, int Length)
{
    /// <inheritdoc />
    public override string ToString() => $"Stretch{{from={From}, length={Length}}}";
}
