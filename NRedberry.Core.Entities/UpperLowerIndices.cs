namespace NRedberry;

public sealed class UpperLowerIndices
{
    public int[] Upper { get; }
    public int[] Lower { get; }

    public UpperLowerIndices(int[] upper, int[] lower)
    {
        Upper = upper;
        Lower = lower;
    }
}
