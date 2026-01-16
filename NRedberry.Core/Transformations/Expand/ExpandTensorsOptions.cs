namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandTensorsOptions.
/// </summary>
public sealed class ExpandTensorsOptions : ExpandOptions
{
    public bool LeaveScalars { get; set; }

    public ExpandTensorsOptions()
    {
        LeaveScalars = false;
    }
}
