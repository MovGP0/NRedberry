using System.Collections;

namespace NRedberry;

public sealed class TypeData
{
    public TypeData(int from, int length, BitArray? states)
    {
        From = from;
        Length = length;
        States = states == null ? null : (BitArray)states.Clone();
    }

    public int From { get; }
    public int Length { get; }
    public BitArray? States { get; }
}
